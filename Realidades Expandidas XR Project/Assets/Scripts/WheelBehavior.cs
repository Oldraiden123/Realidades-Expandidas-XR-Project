using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class WheelBehavior : MonoBehaviour, IFixable
{
    private XRKnob xrKnob;
    private EnemySpawner enemySpawner;
    private bool hasFixed;

    [SerializeField] private float wheelMeter = 0f;
    [SerializeField] private float maxMeter = 100f;
    [SerializeField] private float wheelMeterRate = 0.01f;
    float savedValue;

    [SerializeField] private List<AudioClip> sounds = new List<AudioClip>();
    private AudioSource audioSource;

    private float soundTimer;
    [SerializeField] private float soundCooldown = 1.5f;



    private void Awake()
    {
        xrKnob = GetComponent<XRKnob>();
        enemySpawner = GetComponent<EnemySpawner>();
        audioSource = GetComponent<AudioSource>();
        xrKnob.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsFixed() && !hasFixed)
        {
            enemySpawner.RollForEnemySpawn();
            hasFixed = true;
            xrKnob.maxAngle = 0;
            Debug.Log("Success");
            audioSource.enabled = false;
        }
    }

    public void WheelTurned()
    {
        if (xrKnob == null) return;

        if (soundTimer > soundCooldown)
        {
            soundTimer = 0;
            audioSource.clip = sounds[Random.Range(0, sounds.Count)];
            audioSource.PlayOneShot(audioSource.clip);
        }
        else
        {
            soundTimer += Time.deltaTime;
        } 

        if (xrKnob.value > savedValue)
        {
            wheelMeter += wheelMeterRate;
            savedValue = xrKnob.value;

        }
    }

    public void UnFix()
    {
        savedValue = 0;
        xrKnob.enabled = true;
        hasFixed = false;
    }

    public bool IsFixed()
    {
        return wheelMeter >= maxMeter;
    }
}
