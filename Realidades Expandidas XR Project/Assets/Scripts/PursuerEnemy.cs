using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

public class PursuerEnemy : MonoBehaviour, IShineable
{
    [SerializeField] private GameManager _gm;

    [SerializeField] private float _detectionRange = 8f;
    [SerializeField] private float _killRange = 2f;

    [SerializeField] private float _despawnTime = 10f;

    [SerializeField] private float _normalSpeed = 1.5f;
    [SerializeField] private float _runSpeed = 5f;

    [SerializeField, ReadOnly] private Transform _target;
    private Timer _despawnTimer;

    private NavMeshAgent _agent;

    [SerializeField] private AudioClip screamer;

    private bool _sawPlayer = false;
    public bool SawPlayer {
        get => _sawPlayer;

        set
        {
            if (value != _sawPlayer)
            {
                _playerReferencePosition = _target.position;
            }

            _sawPlayer = value;
        }
    }

    [SerializeField] private bool _isSpawned = false;
    private bool IsSpawned
    {
        get => _isSpawned;
        set
        {
            if (value != _isSpawned)
            {
                _despawnTimer.ResetTimer();
            }

            _isSpawned = value;
        }
    }

    private Vector3 _playerReferencePosition;
    private Vector3 _destination;
    private Coroutine _patrolCoroutine;

    private void Start()
    {
        _target = FindAnyObjectByType<PlayerWallBlock>().GetComponentInChildren<Camera>().transform;

        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        _despawnTimer = new Timer(_despawnTime, Timer.TimerReset.Manual);

        // Subscribe to timer events
        _despawnTimer.OnTimerDone += DespawnEnemy;
        gameObject.GetComponent<AudioSource>().Play();

        IsSpawned = true;
    }

    private void OnDisable()
    {
        // Unsubscribe from timer events
        _despawnTimer.OnTimerDone -= DespawnEnemy;

    }

    private void Update()
    {
        if (IsSpawned)
        {
            // Check if player is in range and in line of sight
            SawPlayer = CheckForPlayer();

            if (SawPlayer)
            {
                FollowPlayer();

                if (Vector3.Distance(transform.position, _playerReferencePosition) <= _killRange)
                {
                    // Kill Player

                    AttackPlayer();

                    SawPlayer = false;
                    IsSpawned = false;
                }
            }
            else
            {
                Patrol();

                _despawnTimer.CountTimer();
            }
        }

        LookAtDestination();
    }

    private Vector3 RandomNavmeshLocation(float radiusMin, float radiusMax, bool awayFromPlayer = false)
    {
        Debug.Log($"{gameObject.name} is chosing new point");

        float radius = Random.Range(radiusMin,radiusMax);

        Vector3 direction;

        if (awayFromPlayer)
        {
            direction = _target.forward * radius;
            direction += transform.position;
        }
        else
        {
            direction = Random.insideUnitSphere * radius;
        }

        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(direction, out hit, radius, 1)) {
            finalPosition = hit.position;            
        }

        return finalPosition;
    }

    private void LookAtDestination()
    {
        Vector3 pos = _agent.steeringTarget;
        pos.y = transform.position.y;

        transform.LookAt(pos);
    }

    private bool CheckForPlayer()
    {
        bool result = false;
        Vector3 pos = _target.position;
        pos.y = transform.position.y;

        if (Vector3.Distance(transform.position, pos) <= _detectionRange)
        {
            Debug.DrawRay(transform.position, (_target.position - transform.position).normalized * _detectionRange, Color.red);

            if (Physics.Raycast(transform.position, (_target.position - transform.position).normalized, out RaycastHit hit, _detectionRange))
            {
                Debug.DrawLine(transform.position, hit.point, Color.green);

                if (hit.collider.gameObject.GetComponent<Camera>() != null)
                {
                    result = true;
                    Debug.Log("Saw Player");
                    Debug.DrawLine(transform.position, hit.point, Color.blue);
                }
            }
        }
        return result;
    }

    private void FollowPlayer()
    {
        _agent.speed = _runSpeed;

        Vector3 pos = _target.position;
        pos.y = transform.position.y;

        _agent.SetDestination(pos);
    }

    private void Patrol()
    {
        _agent.speed = _normalSpeed;

        if (_patrolCoroutine == null) _patrolCoroutine = StartCoroutine(PatrolCR());
    }

    private IEnumerator PatrolCR()
    {
        Vector3 pos = RandomNavmeshLocation(15f, 20f);

        _agent.SetDestination(pos);
        _destination = pos;

        Debug.Log($"Patrolling to position: {pos}");


        yield return new WaitForSeconds(Random.Range(5f, 10f));

        _patrolCoroutine = null;
    }

    private void RunAway()
    {
        SawPlayer = false;
        IsSpawned = false;

        Debug.Log("Run Away");

        

        StartCoroutine(RunAwayCR());
    }

    private IEnumerator RunAwayCR()
    {
        Vector3 pos = RandomNavmeshLocation(15f, 20f, true);

        _agent.SetDestination(pos);
        _destination = pos;

        yield return new WaitUntil(() => Vector3.Distance(transform.position, pos) <= _agent.stoppingDistance);

        DespawnEnemy();
    }


    private void AttackPlayer()
    {
        Debug.Log("Attack Player");

        StartCoroutine(AttackPlayerCR());
    }

    private IEnumerator AttackPlayerCR()
    {
        Vector3 pos = _target.position;
        pos.y = transform.position.y;

        _agent.SetDestination(pos);
        _destination = pos;

        yield return new WaitUntil(() => Vector3.Distance(transform.position, pos) <= _agent.stoppingDistance);

        gameObject.GetComponent<AudioSource>().PlayOneShot(screamer);
        _gm?.LoseGame();
        DespawnEnemy();
    }

    public void DespawnEnemy()
    {
        SawPlayer = false;
        IsSpawned = false;
        
        //gameObject.SetActive(false);
        _gm?.DespawnPursuerEnemy();

        Debug.Log("Despawn Enemy");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
        Gizmos.DrawWireSphere(transform.position, _killRange);
    }

    public void Shine()
    {
        RunAway();
        gameObject.GetComponent<AudioSource>().PlayOneShot(screamer);
    }
}
