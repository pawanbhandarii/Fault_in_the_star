using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public GameObject camreaOne;
    public GameObject cameraTwo;
    public bool switchToOne;
    public bool switchToTwo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchCameras()
    {
        if (switchToOne)
        {
            cameraTwo.SetActive(false);
            cameraTwo.GetComponent<AudioListener>().enabled = false;
            camreaOne.SetActive(true);
            camreaOne.GetComponent<AudioListener>().enabled = true;
        }
        else if (switchToTwo)
        {
            camreaOne.SetActive(false);
            camreaOne.GetComponent<AudioListener>().enabled = false;
            cameraTwo.SetActive(true);
            cameraTwo.GetComponent<AudioListener>().enabled = true;
            
        }
        else
        {
            return;
        }
    }
}
