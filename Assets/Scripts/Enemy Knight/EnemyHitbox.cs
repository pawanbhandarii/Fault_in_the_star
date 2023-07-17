using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int damage;

    public bool haveAttacked = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!haveAttacked && collision.gameObject.CompareTag("Player"))
        {
            haveAttacked = true;
            collision.GetComponent<PlayerController>().AppplyDamage(damage);
        }        
    }

  
}
