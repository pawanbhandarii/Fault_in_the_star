using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.WSA.Input;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuButtonConroller menuButtonConroller;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    public int thisIndex;

    private void Update()
    {
        if (menuButtonConroller.index == thisIndex)
        {
            animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1)
            {
                animator.SetBool("pressed", true);
            }
            else if(animator.GetBool("pressed"))
            {
                animator.SetBool("pressed", false);
                animatorFunctions.disableOnce = true;
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }
    }

    private void OnMouseEnter()
    {
        animator.SetBool("selected", true);

        if (Input.GetMouseButtonDown(0)){
            animator.SetBool("pressed", true);
        }
        else
        {
            animator.SetBool("pressed", false);
        }
    }

    private void OnMouseExit()
    {
        animator.SetBool("selected", false);
    }
}
