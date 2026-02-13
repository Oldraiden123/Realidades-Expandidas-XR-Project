using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightIntermitent : MonoBehaviour
{

    [Header("Intensity Settings")]
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 3f;

    [Header("Speed Settings")]
    [SerializeField] private float speed = 1f;

    [Header("Mode")]
    [SerializeField] private bool usePingPong = true;   // Smooth back and forth
    [SerializeField] private bool useSineWave = false;  // Optional smoother curve

    private Light targetLight;
    private float timeOffset;

    void Awake()
    {
        targetLight = GetComponent<Light>();
        timeOffset = Random.value * 10f; // Prevents identical syncing
    }

    void Update()
    {
        float t;

        if (useSineWave && targetLight.enabled)
        {
            t = (Mathf.Sin((Time.time + timeOffset) * speed) + 1f) / 2f;
        }
        else if (usePingPong && targetLight.enabled)
        {
            t = Mathf.PingPong((Time.time + timeOffset) * speed, 1f);
        }
        else if (!useSineWave && !usePingPong && targetLight.enabled)
        {
            t = Mathf.Lerp(0f, 1f, (Time.time + timeOffset) * speed);
        }

        //targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }
}
