using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using System.Collections;
using System.Collections.Generic;

public class SlidersManager : MonoBehaviour
{
    [SerializeField]private List<XRSlider> xrSliders = new List<XRSlider> ();   
    [SerializeField] private int currentlySolvedSliders = 0;
    [SerializeField] private int totalSliders = 9;





    void Start()
    {
        
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




}
