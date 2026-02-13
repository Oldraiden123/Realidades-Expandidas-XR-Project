using Unity.XR.CoreUtils;
using UnityEngine;
    using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class VignetteBehavior : MonoBehaviour
{
    private Material vignetteMat;
    public bool isVignetteOn = true;
    public bool wasVignetteTurnedOn = false;
    private string storedScene;
    private bool loadSceneAfterClosing;
    [SerializeField] private float vignetteSpeed = 3f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vignetteMat = GetComponent<MeshRenderer>().material;

    }

    // Update is called once per frame
    void Update()
    {
        if (isVignetteOn)
        {
            OpenVignette();
        }
        else if (wasVignetteTurnedOn)
        {

            CloseVignette();
        }
    }

    void OpenVignette()
    {
        float vignetteStrength = vignetteMat.GetFloat("_Aperture");
        vignetteMat.SetFloat("_Aperture", (vignetteStrength += vignetteSpeed * Time.deltaTime));

        if (vignetteMat.GetFloat("_Aperture") >= 1f)
        {
            isVignetteOn = false;
            vignetteMat.SetFloat("_Aperture", 1);
        }
    }

    void CloseVignette()
    {
        float vignetteStrength = vignetteMat.GetFloat("_Aperture");
        vignetteMat.SetFloat("_Aperture", (vignetteStrength -= vignetteSpeed * Time.deltaTime));

        if (vignetteMat.GetFloat("_Aperture") <= 0f)
        {
            wasVignetteTurnedOn = false;
            vignetteMat.SetFloat("_Aperture", 0);
            if (loadSceneAfterClosing)
            {
                loadSceneAfterClosing = false;
                SceneManager.LoadScene(storedScene, LoadSceneMode.Single);
            }

        }
    }

    public void ChangeSceneAfterVignette(string sceneToLoad)
    {
        wasVignetteTurnedOn = true;
        loadSceneAfterClosing = true;
        storedScene = sceneToLoad;
    }

}
