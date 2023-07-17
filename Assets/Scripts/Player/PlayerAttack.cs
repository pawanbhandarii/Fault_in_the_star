using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //reference to the player animator
    public Animator animator;
    public AudioSource audio;
    public AudioClip attackSFX;

    public Transform attackPoint; // takes reference to the attack point
    public float attackRange = 0.3f; // notes down the attack range
    public LayerMask enemyLayers; // Keeps the enemies
    public int damageDone = 20; //damage done when damage is dealt
    public float attackRate = 2f;
    float nextAttackTime = 0f;


    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Attack"))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        } 
    }

    void Attack()
    {
        //Plays attack animatins
        animator.SetTrigger("Attack");

        //Play Sound
        audio.PlayOneShot(attackSFX);
        
        //Detect enemies within the attack range
        Collider2D[] hitenemies= Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage the emeny
            foreach (Collider2D enemy in hitenemies)
            {
                Debug.Log(enemy.name);
                enemy.GetComponentInParent<EnemyController>().TakeDamage(damageDone);
            }
        
    }

    private void OnDrawGizmos()
    {
        //If attack point is not assigned, its returns in order to minimize error
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange); // Draws a circle in the scene view for convience sake
    }
}
