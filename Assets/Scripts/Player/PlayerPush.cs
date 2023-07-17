using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public Animator animator;
    public float boxRayDistance = 1f;
    public LayerMask whatIsBox;
    public AudioSource audio;
    public AudioClip pushSFX;

    bool isPushing = false;
    GameObject box;
    PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        //Physics2D.queriesStartInColliders = false;
        RaycastHit2D hitinfo=Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, boxRayDistance, whatIsBox);
        
        if(hitinfo.collider !=null && hitinfo.collider.gameObject.tag=="Box" && Input.GetKeyDown(KeyCode.E))
        {
            isPushing = true;
            box = hitinfo.collider.gameObject;
            box.GetComponent<FixedJoint2D>().enabled = true;
            box.GetComponent<BoxPull>().SetState(true);
            box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
            
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            isPushing = false;
            box.GetComponent<FixedJoint2D>().enabled = false;
            box.GetComponent<BoxPull>().SetState(false);
        }
        animator.SetBool("Pushing", isPushing);

        if (isPushing && !audio.isPlaying)
        {
            audio.clip = pushSFX;
            audio.Play();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * boxRayDistance);
    }

    public bool Pushing()
    {
        return isPushing;
    }
}
