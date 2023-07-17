using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTriggerArea : MonoBehaviour
{
    private MageBehaviour enemy_parent;

    private void Awake()
    {
        enemy_parent = GetComponentInParent<MageBehaviour>();
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
