using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    private Enemy_Behaviour enemy_parent;

    private void Awake()
    {
        enemy_parent = GetComponentInParent<Enemy_Behaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemy_parent.target = collision.transform;
            enemy_parent.inRange = true;
            enemy_parent.hotZone.SetActive(true);
        }
    }
}
