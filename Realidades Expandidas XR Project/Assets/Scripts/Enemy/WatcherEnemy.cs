using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WatcherEnemy : MonoBehaviour, IShineable
{
    [SerializeField] private GameManager _gm;

    [SerializeField] private float _watchingTime = 5f;
    [SerializeField] private float _detectionRange = 5f;
    [SerializeField] private float _movementThreshold = 1f;


    [SerializeField] private float _despawnTime = 10f;

    [SerializeField] private float _runSpeed = 5f;

    private Transform _target;
    private Timer _watchingTimer;
    private Timer _despawnTimer;

    private NavMeshAgent _agent;

    private bool _sawPlayer = false;
    [SerializeField] private AudioClip screamer;

    public bool SawPlayer {
        get => _sawPlayer;

        set
        {
            if (value != _sawPlayer)
            {
                _playerReferencePosition = _target.position;
                _watchingTimer.ResetTimer();
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

    private void Start()
    {
        _target = FindAnyObjectByType<PlayerWallBlock>().GetComponentInChildren<Camera>().transform;

        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _runSpeed;
    }

    private void OnEnable()
    {
        _watchingTimer = new Timer(_watchingTime, Timer.TimerReset.Manual);
        _despawnTimer = new Timer(_despawnTime, Timer.TimerReset.Manual);
        gameObject.GetComponent<AudioSource>().Play();

        // Subscribe to timer events
        _watchingTimer.OnTimerDone += RunAway;
        _despawnTimer.OnTimerDone += DespawnEnemy;

        IsSpawned = true;
    }

    private void OnDisable()
    {
        // Unsubscribe from timer events
        _watchingTimer.OnTimerDone -= RunAway;
        _despawnTimer.OnTimerDone -= DespawnEnemy;
    }

    private void Update()
    {
        if (IsSpawned)
        {
            LookAtPlayer();

            // Check if player is in range and in line of sight
            SawPlayer = CheckForPlayer();

            if (SawPlayer)
            {
                // Count watching timer
                _watchingTimer.CountTimer();

                if (CheckForPlayerMovement())
                {
                    // Kill Player

                    AttackPlayer();

                    SawPlayer = false;
                    IsSpawned = false;
                }
            }
            else _despawnTimer.CountTimer();
        }
        else if (_agent.hasPath)
        {
            LookAtDestination();
        }
        
    }

    private Vector3 RandomNavmeshLocation(float radiusMin, float radiusMax)
    {
        Debug.Log($"{gameObject.name} is chosing new point");

        float radius = Random.Range(radiusMin,radiusMax);

        Vector3 direction = _target.forward * radius;
        direction += transform.position;

        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(direction, out hit, radius, 1)) {
            finalPosition = hit.position;            
        }

        return finalPosition;
    }


    private void LookAtPlayer()
    {
        Vector3 pos = _target.position;
        pos.y = transform.position.y;

        transform.LookAt(pos);
    }

    private void LookAtDestination()
    {
        Vector3 pos = _destination;
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

    private bool CheckForPlayerMovement()
    {
        return Vector3.Distance(_playerReferencePosition, _target.position) >= _movementThreshold;
    }


    private void RunAway()
    {
        SawPlayer = false;
        IsSpawned = false;

        gameObject.GetComponent<AudioSource>().PlayOneShot(screamer);

        Debug.Log("Run Away");

        StartCoroutine(RunAwayCR());
    }

    private IEnumerator RunAwayCR()
    {
        Vector3 pos = RandomNavmeshLocation(15f, 20f);

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
        DespawnEnemy();
        _gm?.LoseGame();
    }

    public void DespawnEnemy()
    {
        SawPlayer = false;
        IsSpawned = false;
        
        //gameObject.SetActive(false);
        _gm?.DespawnWatcherEnemy();

        Debug.Log("Despawn Enemy");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }

    public void Shine()
    {
        AttackPlayer();
    }
}
