using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;

    bool hasEntered = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canvas.SetActive(true);
            GetComponentInChildren<Dialog>().enabled = true;
            GetComponentInChildren<Dialog>().StartAgain();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canvas.SetActive(false);
            GetComponentInChildren<Dialog>().enabled = false;
        }
    }
}

