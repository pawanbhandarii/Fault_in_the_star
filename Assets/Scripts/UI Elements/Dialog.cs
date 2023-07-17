using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentence;
    private int index=0;
    public float typingSpeed;
    public GameObject continueButton;
    public GameObject dialogueBoxImage;

    private void Update()
    {
        
        if (textDisplay.text == sentence[index])
        {
            continueButton.SetActive(true);
        }
    }
    void Type()
    {
        textDisplay.text = sentence[index];
    }

    public void NextSentecne()
    {
        continueButton.SetActive(false);
        if(index <sentence.Length - 1)
        {
            index++;
            textDisplay.text = "";
            Type();
        }
        else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
            dialogueBoxImage.SetActive(false);
            index = 0;
        }
    }

    public void StartAgain()
    {
        textDisplay.text = "";
        dialogueBoxImage.SetActive(true);
        Type();
    }
}
