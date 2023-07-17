using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //define the max health of the enemy
    public float maxHealth = 100f;
    float currentHealth;
    public Animator animator;
    public AudioSource audio;
    public AudioClip hurtSFX;
    public AudioClip deadSFX;
    public GameObject hitbox;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    //Takes damage
    public void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            //Hurt Animation
            animator.SetTrigger("Hurt");
            //Damage taken 
            currentHealth -= damage;
            audio.PlayOneShot(hurtSFX);
        }
        
        if (currentHealth <= 0)
        {
            StartCoroutine(KillEnemy());
            audio.PlayOneShot(deadSFX);
        }
    }

    IEnumerator KillEnemy()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(false);
            hitbox.GetComponent<Collider2D>().enabled = false;
        }
        GetComponentInChildren<Collider2D>().enabled = false;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
        this.enabled = false;
    }

}
