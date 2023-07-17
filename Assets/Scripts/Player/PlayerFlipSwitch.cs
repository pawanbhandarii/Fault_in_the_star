using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlipSwitch : MonoBehaviour
{
    public float switchRayDistance = 1f;
    public LayerMask whatIsSwitch;
    public AudioClip switchSfx;
    public AudioSource audio;

    bool canInteract = true;
    GameObject Switch;
    void Update()
    {
       RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, switchRayDistance,whatIsSwitch);
        if(canInteract && hitinfo.collider !=null && hitinfo.collider.gameObject.tag=="Switch" && Input.GetKeyDown(KeyCode.F))
        {
            canInteract = false;
            Switch=hitinfo.collider.gameObject;
            Switch.GetComponent<LeverSwitch>().Switch();
            audio.PlayOneShot(switchSfx);
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            canInteract = true;
        }
    }
}
