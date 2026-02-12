using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class WheelBehavior : MonoBehaviour, IFixable
{
    private XRKnob xrKnob;
    private EnemySpawner enemySpawner;

    [SerializeField] private float wheelMeter = 0f;
    [SerializeField] private float maxMeter = 100f;
    [SerializeField] private float wheelMeterRate = 0.01f;
    float savedValue;


    private void Start()
    {
        xrKnob = GetComponent<XRKnob>();
        enemySpawner = GetComponent<EnemySpawner>();

        xrKnob.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(wheelMeter >= maxMeter)
        {
            enemySpawner.SpawnEnemy();
            xrKnob.maxAngle = 0;
            Debug.Log("Success");
        }
    }

    public void WheelTurned()
    {
        if(xrKnob.value > savedValue)
        {
            wheelMeter += wheelMeterRate;
            savedValue = xrKnob.value;
        }
    }

    public void UnFix()
    {
        savedValue = 0;
        xrKnob.enabled = true;
    }

    public bool IsFixed()
    {
        return wheelMeter >= maxMeter;
    }
}
