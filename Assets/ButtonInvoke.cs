using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInvoke : MonoBehaviour
{
    public MenuButtonConroller menuButtonConroller;
    public MenuButton menuButton;
    public bool isInvoke=false;
    public Button button;
    public float timer;
    public float intTimer = 2f;
    Animator animator;
    // Update is called once per frame

    private void Start()
    {
        isInvoke = false;
        timer = intTimer;
        menuButton = GetComponent<MenuButton>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (menuButtonConroller.index == menuButton.thisIndex)
        {
            if (!isInvoke)
            {
                if (Input.GetAxis("Submit") == 1)
                {
                    isInvoke = true;
                    button.onClick.Invoke();
                }
            }
            else
            {
                if (timer <= 0)
                {
                    isInvoke = false;
                    timer = intTimer;
                }
                else
                {
                    timer -= Time.deltaTime;
                }
            }
        } 
    }
}
