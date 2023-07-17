using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageHotZone : MonoBehaviour
{
    private MageBehaviour enemy_parent;
    private bool inRange;
    private Animator animator;

    private void Awake()
    {
        enemy_parent = GetComponentInParent<MageBehaviour>();
        animator = GetComponentInParent<Animator>();
    }
    private void Update()
    {
        if (inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            enemy_parent.Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
            gameObject.SetActive(false);
            enemy_parent.triggerArea.SetActive(true);
            enemy_parent.inRange = false;
            enemy_parent.SelectTarget();
        }
    }
}
