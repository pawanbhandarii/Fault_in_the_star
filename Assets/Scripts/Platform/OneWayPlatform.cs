using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private int playerLayer;
    private int platformLayer;

    private void Awake()
    {
        platformLayer = this.gameObject.layer;

        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        Physics2D.IgnoreLayerCollision(platformLayer, playerLayer, (Input.GetKey(KeyCode.DownArrow)||(Input.GetKey(KeyCode.S)))||(Input.GetKey(KeyCode.UpArrow) || (Input.GetKey(KeyCode.W))));
    }
   

}
