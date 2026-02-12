    using UnityEngine;
    using UnityEngine.SceneManagement;

public class VignetteBehavior : MonoBehaviour
{
    private Material vignetteMat;
    public bool isVignetteOn = true;
    public bool wasVignetteTurnedOn = false;
    private string storedScene;
    private bool loadSceneAfterClosing;
    [SerializeField] private float vignetteSpeed = 3f;
    private Transform storedPosition;
    private bool movePlayerAfterSceneClosing;
    private GameObject player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vignetteMat = GetComponent<MeshRenderer>().material;
        player = GameObject.FindWithTag("Player");
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
            if (movePlayerAfterSceneClosing)
            {
                movePlayerAfterSceneClosing = false;
                wasVignetteTurnedOn = false;
                isVignetteOn = true;
                player.transform.position = storedPosition.position;
            }
        }
    }

    public void ChangeSceneAfterVignette(string sceneToLoad)
    {
        wasVignetteTurnedOn = true;
        loadSceneAfterClosing = true;
        storedScene = sceneToLoad;
    }

    public void MovePlayerAfterVignette(Transform targetPosition)
    {
        wasVignetteTurnedOn = true;
        movePlayerAfterSceneClosing = true;
        storedPosition = targetPosition;
    }
}
