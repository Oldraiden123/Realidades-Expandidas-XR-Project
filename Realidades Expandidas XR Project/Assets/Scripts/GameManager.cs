using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int difficultyLevel = 0;
    public int maxDifficultyLevel = 3;
    [SerializeField] private GameObject pursuerEnemy;
    [SerializeField] private GameObject enemyStorage;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPursuerEnemy (Transform spawnLocation)
    {
        pursuerEnemy.transform.parent = null;
        pursuerEnemy.transform.position = spawnLocation.position;
        pursuerEnemy.transform.rotation = spawnLocation.rotation;
        foreach(Transform child in spawnLocation)
        {
            pursuerEnemy.GetComponent<PursuerEnemy>().waypoints.Add(child);
        }
        pursuerEnemy.SetActive(true);
    }

    public void DespawnPursuerEnemy()
    {
        pursuerEnemy.transform.position = enemyStorage.transform.position;
        pursuerEnemy.transform.parent = enemyStorage.transform;
        pursuerEnemy.GetComponent<PursuerEnemy>().ResetEnemy();
        pursuerEnemy.SetActive(false);
    }
}
