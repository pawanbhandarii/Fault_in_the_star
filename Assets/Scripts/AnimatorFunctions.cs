using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    AudioSource audio;
    public bool disableOnce;

    private void Start()
    {
        audio = gameObject.AddComponent<AudioSource>();
    }
    public void PlaySound(AudioClip whichSound)
    {
        audio.PlayOneShot(whichSound);
    }
}
