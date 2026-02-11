using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInitializer : MonoBehaviour
{

    public float playerHeight;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        GameObject.Find("XR Origin (XR Rig)").GetComponent<XROrigin>().CameraYOffset = playerHeight;
    }
}
