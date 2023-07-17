using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    AudioSource audio;
    public AudioClip doorSFX;
    private bool isPressed = false;

    private void Start()
    {
        audio=GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPressed && (collision.gameObject.tag == "Box"))
        {
            isPressed = true;
            GetComponent<SpriteRenderer>().enabled = false;
            audio.PlayOneShot(doorSFX);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isPressed && (collision.gameObject.tag == "Box"))
        {
            isPressed = false;
            GetComponent<SpriteRenderer>().enabled = true;
            audio.PlayOneShot(doorSFX);
        }
    }

    public bool Pressed()
    {
        return isPressed;
    }
}
