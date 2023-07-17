using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverSwitch : MonoBehaviour
{

    public bool isOn = false;
    public Animator animator;


    public void Switch()
    {
        if (!isOn)
        {
            isOn = true;
            animator.SetTrigger("On");
        }
        else
        {
            isOn = false;
            animator.SetTrigger("Off");
        }
    }

}
