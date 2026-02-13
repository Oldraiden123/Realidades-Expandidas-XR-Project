using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{

    [SerializeField] private VignetteBehavior vignetteBehavior;
    
    void Start()
    {
        vignetteBehavior = GameObject.FindWithTag("Player").GetComponentInChildren<VignetteBehavior>();
    }

    public void LoadMainLevel()
    {
        
        vignetteBehavior.ChangeSceneAfterVignette("MapTest");
    }

    public void LoadMainMenu()
    {
        vignetteBehavior.ChangeSceneAfterVignette("StartMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
