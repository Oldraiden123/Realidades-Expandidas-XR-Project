using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnLocation;
    [SerializeField] private GameManager gm;
    private enum EnemyType { Pursuer, Watcher }
    [SerializeField] private EnemyType enemyType;


    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    private void Update()
    {
        /* if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnEnemy();
        }*/
    }

    public void RollForEnemySpawn()
    {
        Debug.Log("Rolling for enemy spawn...");

        gm.DifficultyLevel++;

        if (Random.Range(gm.DifficultyLevel, gm.maxDifficultyLevel + 1) == gm.maxDifficultyLevel)
        {
            SpawnEnemy();
        }
        
    }
    public void SpawnEnemy()
    {
        if (enemyType == EnemyType.Pursuer)
        {
            gm.SpawnPursuerEnemy(enemySpawnLocation);
            Debug.Log($"Spawn Pursuer Enemy at {enemySpawnLocation.position}");

        }
        else if (enemyType == EnemyType.Watcher)
        {
            gm.SpawnWatcherEnemy(enemySpawnLocation);
            Debug.Log($"Spawn Watchers Enemy at {enemySpawnLocation.position}");
        }

    }
}
