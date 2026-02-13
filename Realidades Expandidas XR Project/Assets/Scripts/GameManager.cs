using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int difficultyLevel = 0;
    public int DifficultyLevel
    {
        get => difficultyLevel;
        set
        {
            if (value < 0) difficultyLevel = 0;
            else if (value > maxDifficultyLevel) difficultyLevel = maxDifficultyLevel;
            else difficultyLevel = value;
        }
    }
    public int maxDifficultyLevel = 5;
    [SerializeField] private GameObject pursuerEnemy;
    [SerializeField] private GameObject watcherEnemy;
    [SerializeField] private GameObject enemyStorage;
    [SerializeField, MinMaxSlider(1,8)] private Vector2Int _unfixableAmountRange;

    private List<Fixable> _fixables = new List<Fixable>();

    [SerializeField] private VignetteBehavior vignetteBehaviour;


    private bool hasWon;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetFixables();
        SetBrokenFixables();

        vignetteBehaviour = GameObject.FindWithTag("Player").GetComponentInChildren<VignetteBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckFixables() && !hasWon)
        {
            hasWon = true;
            WinGame();
            Debug.Log("All fixables are fixed!");
        }
    }

    private void GetFixables()
    {
        _fixables.Clear();
        _fixables.AddRange(FindObjectsByType<Fixable>(0));
    }

    private void SetBrokenFixables()
    {
        int unfixableAmount = Random.Range(_unfixableAmountRange.x, _unfixableAmountRange.y);

        var tmp = new List<Fixable>(_fixables);
        
        for (int i = 0; i < unfixableAmount; i++)
        {
            Fixable fixable = tmp[Random.Range(0, tmp.Count)];
            fixable.UnFix();

            tmp.Remove(fixable);
        }
    }
    private bool CheckFixables()
    {
        foreach (Fixable fixable in _fixables)
        {
            if (!fixable.IsFixed)
            {
                return false;
            }
        }

        return true;
    }

    public void SpawnPursuerEnemy (Transform spawnLocation)
    {
        pursuerEnemy.transform.parent = null;
        pursuerEnemy.transform.position = spawnLocation.position;
        pursuerEnemy.transform.rotation = spawnLocation.rotation;
        pursuerEnemy.SetActive(true);
    }

    public void DespawnPursuerEnemy()
    {
        pursuerEnemy.transform.position = enemyStorage.transform.position;
        pursuerEnemy.transform.parent = enemyStorage.transform;
        pursuerEnemy.SetActive(false);
    }

    public void SpawnWatcherEnemy(Transform spawnLocation)
    {
        watcherEnemy.transform.parent = null;
        watcherEnemy.transform.position = spawnLocation.position;
        watcherEnemy.transform.rotation = spawnLocation.rotation;
        pursuerEnemy.SetActive(true);
    }

    public void DespawnWatcherEnemy()
    {
        watcherEnemy.transform.position = enemyStorage.transform.position;
        watcherEnemy.transform.parent = enemyStorage.transform;
        watcherEnemy.GetComponent<WatcherEnemy>().DespawnEnemy();
        pursuerEnemy.SetActive(false);
    }
    public void WinGame()
    {
        vignetteBehaviour.ChangeSceneAfterVignette("WinMenu");

    }
    public void LoseGame()
    {
        vignetteBehaviour.ChangeSceneAfterVignette("LoseMenu");

    }
}
