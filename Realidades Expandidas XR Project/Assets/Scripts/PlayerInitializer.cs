using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInitializer : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (GameObject rig in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(rig != this.gameObject)
            {
                GameObject.Destroy(rig);
            }
        }

        Vector3 startPos = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
        transform.position = startPos;
        
    }
}
