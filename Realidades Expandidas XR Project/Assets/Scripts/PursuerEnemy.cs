using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PursuerEnemy : MonoBehaviour
{

    [SerializeField] private bool isDefeated;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private GameManager gm;
    public List<Transform> waypoints = new List<Transform>();
    [SerializeField] private Quaternion targetDirection;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float distance;
    [SerializeField] private float distanceBuffer = 0.5f;

    [SerializeField] private int listIndex;

    private float watchTimer;
    [SerializeField] private float maxWatchTimer = 10f;

    private Transform player;
    private SphereCollider detectionTrigger;



    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        detectionTrigger = gameObject.GetComponent<SphereCollider>();
        detectionTrigger.enabled = true;
    }

    private void OnEnable()
    {
        detectionTrigger.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (listIndex >= waypoints.Count)
        {
            gm.DespawnPursuerEnemy();
        }
        if (isDefeated)
        {
            if(listIndex == 0)
            {
                UpdateTarget();
            }
            if(transform.rotation != targetDirection)
            {
                RotateEnemy();
            }
            else
            {
                MoveEnemy();
            }
            
            
        }
    }

    private void UpdateTarget()
    {
        
        targetPosition = waypoints[listIndex].position;
        targetDirection = Quaternion.LookRotation((targetPosition - transform.position).normalized);

    }

    private void RotateEnemy()
    {
        
        transform.rotation = Quaternion.Lerp(transform.rotation, targetDirection, Time.deltaTime * rotationSpeed);
    }

    private void MoveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * movementSpeed);
        distance = Vector3.Distance(waypoints[listIndex].position, transform.position);
        if (distance <= distanceBuffer)
        {

            listIndex++;
            UpdateTarget();
        }
    }

    public void ResetEnemy()
    {
        detectionTrigger.enabled = false;
        watchTimer = 0;
        isDefeated = false;
        listIndex = 0;
        waypoints.Clear();
    
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            if (watchTimer >= maxWatchTimer)
            {
                AttackPlayer();
            }
            watchTimer += Time.deltaTime;
        }
    }

    public void DefeatEnemy()
    {
        isDefeated = true;
        detectionTrigger.enabled = false;
        watchTimer = 0;
    }

    private void AttackPlayer()
    {
        //kill player
        targetPosition = player.position;
        detectionTrigger.enabled = false;
        watchTimer = 0;
    }

}
