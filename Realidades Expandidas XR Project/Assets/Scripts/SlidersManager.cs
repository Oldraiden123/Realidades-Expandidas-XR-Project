using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using System.Collections;
using System.Collections.Generic;

public class SlidersManager : MonoBehaviour, IFixable
{
    [SerializeField]private List<XRSlider> xrSliders = new List<XRSlider> ();   
    [SerializeField] private int currentlySolvedSliders = 0;
    [SerializeField] private int totalSliders = 9;

    private void MessSliders()
    {
        foreach(XRSlider xrSlider in xrSliders)
        {
            xrSlider.value = 0;
            xrSlider.enabled = true;
        }

        int sliderNum = Random.Range(3, xrSliders.Count);

        for (int i = 0; i < sliderNum; i++)
        {
            XRSlider xrSlider = xrSliders[Random.Range(0, xrSliders.Count)];

            xrSlider.value = Random.Range(0f, 1f);
        }
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
                    xrSlider.GetComponent<AudioSource>().Play();
                    currentlySolvedSliders++;
                }
            }            
        }

        if(currentlySolvedSliders == totalSliders)
        {
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
