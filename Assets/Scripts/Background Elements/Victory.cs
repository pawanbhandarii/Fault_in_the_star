using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    public bool taken = false;
    public GameObject explosion;

    //if the player touches the victory and it has not already been taken
    //then the player has reached the victory point of the level

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player" && !taken && collision.gameObject.GetComponent<PlayerController>().playerCanMove){
            taken = true;
            if (explosion)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            collision.gameObject.GetComponent<PlayerController>().Victory();

            Destroy(this.gameObject);
        }
    }
}
