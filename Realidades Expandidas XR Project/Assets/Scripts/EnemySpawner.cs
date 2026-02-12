using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnLocation;
    [SerializeField] private GameManager gm;
    private enum EnemyType { Pursuer, Watcher}
    [SerializeField] private EnemyType enemyType;


    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnEnemy();
        }
    }

    public void RollForEnemySpawn()
    {
        if (Random.Range(gm.difficultyLevel, gm.maxDifficultyLevel) == gm.maxDifficultyLevel)
        {
            SpawnEnemy();
        }
    }
    public void SpawnEnemy()
    {
        if (enemyType == EnemyType.Pursuer)
        {
            gm.SpawnPursuerEnemy(enemySpawnLocation);   

        }
    }
}
