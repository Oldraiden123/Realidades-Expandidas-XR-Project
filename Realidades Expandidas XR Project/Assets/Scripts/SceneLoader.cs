using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{

    [SerializeField] private VignetteBehavior vignetteBehavior;
    
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
