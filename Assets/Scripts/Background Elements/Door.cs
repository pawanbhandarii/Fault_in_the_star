using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public GameObject keySwitch;
    public Transform waypoint;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (keySwitch.GetComponent<Switch>().Pressed())
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }
        animator.SetBool("Closed", isOpen); //Play Aniamtion
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((isOpen) && (collision.gameObject.tag == "Player"))
        {
            collision.transform.position = waypoint.position;
            if (GetComponent<SwitchCamera>() != null)
            {
                GetComponent<SwitchCamera>().SwitchCameras();
            }
        }
    }
}
