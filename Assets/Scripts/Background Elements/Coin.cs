using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public GameObject explosion;
    bool isTaken = false;

    //if the player touches the coin which is not taken and the player can move (is not dead or victory)
    // then the player can take the coin

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.gameObject.tag == "Player") && (!isTaken) && (collision.gameObject.GetComponent<PlayerController>().playerCanMove))
        {
            //mark coin as taken
            isTaken = true;

            //if explosion is provided then instantiate
            if (explosion)
            {
                Instantiate(explosion,transform.position, transform.rotation);
            }

            //do the player colect coin thing
            collision.gameObject.GetComponent<PlayerController>().CollectCoin(coinValue);

            //destroy the coin;
            Destroy(this.gameObject);
        } 
    }
}
