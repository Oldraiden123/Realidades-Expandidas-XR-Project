using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using System.Collections;
using System.Collections.Generic;

public class SlidersManager : MonoBehaviour, IFixable
{
    [SerializeField] private List<XRSlider> xrSliders = new List<XRSlider>();
    [SerializeField] private List<AudioClip> sounds = new List<AudioClip>();
    [SerializeField] private int currentlySolvedSliders = 0;
    [SerializeField] private int totalSliders = 9;
    private bool hasFixed;
    private EnemySpawner enemySpawner;


    void Awake()
    {
        enemySpawner = GetComponent<EnemySpawner>();

        foreach(XRSlider xrSlider in xrSliders)
        {
            xrSlider.gameObject.GetComponent<AudioSource>().clip = sounds[Random.Range(0,sounds.Count)]; 
            xrSlider.value = 0;
            xrSlider.enabled = true;
        }

        hasFixed = true;
    }


    private void MessSliders()
    {
        int sliderNum = Random.Range(3, xrSliders.Count);

        for (int i = 0; i < sliderNum; i++)
        {
            XRSlider xrSlider = xrSliders[Random.Range(0, xrSliders.Count)];

            xrSlider.value = Random.Range(0.2f, 1f);
        }

        hasFixed = false;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(XRSlider xrSlider in xrSliders) 
        {
            if (xrSlider.enabled) 
            {
                if (xrSlider.value == 0)
                {
                    xrSlider.enabled = false;
                    currentlySolvedSliders++;
                }
            }            
        }

        if(IsFixed() && !hasFixed)
        {
            enemySpawner.RollForEnemySpawn();
            hasFixed = true;
            Debug.Log("Sliders solved");
        }
    }

    public void UnFix()
    {
        currentlySolvedSliders = 0;
        MessSliders();
    }

    public bool IsFixed()
    {
        return currentlySolvedSliders == totalSliders;
    }
}
