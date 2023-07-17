using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerControllerScript : MonoBehaviour
{
    Text text;
    float theTime;

     bool isPlaying;
     bool isRest;
    private void Start()
    {
        text = GetComponent<Text>();
    }
    private void Update()
    {
        if (isRest)
        {
            isRest = false;
            text.text = "Time : 00:00";
        }
        else if (isPlaying)
        {
            theTime += Time.deltaTime;
            string minutes = Mathf.Floor((theTime % 3600) / 60).ToString("00");
            string seconds = (theTime % 60).ToString("00");
            text.text = ("Time : "+minutes + ":" + seconds);
        }
        else
        {
            return;
        }
        
    }
    public void StartTime()
    {
        isPlaying = true;
    }

    public void StopTime()
    {
        isPlaying = false;
        if (GameManager.gm)
        {
            GameManager.gm.GetPlayedTime(theTime);
        }
    }

    public void ResetTime()
    {
        isRest = true;
    }
}
