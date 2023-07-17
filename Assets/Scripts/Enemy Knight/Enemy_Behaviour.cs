using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behaviour : MonoBehaviour
{
    #region Public Variables
    public float attackDistacne; // Minimum Distance for attack
    public float moveSpeed;
    public float timer; //Timer for cooldown of an attack
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange; //Checks if player is in range
    public GameObject hotZone;
    public GameObject triggerArea;
    public GameObject hitbox;
    public int enemyHealth;
    [HideInInspector] public bool enemyCanMove=true;
    public AudioSource audio;

    #endregion

    #region Private Variables

    private Animator animator;
    private float distance; //Stores the distance between enemy and player
    private bool attackMode;
    
    private bool cooling;
    private float intTimer;
    
    #endregion

    private void Awake()
    {
        SelectTarget();
        intTimer = timer;  // Stores the inital value of timer
        animator = GetComponent<Animator>();

    }
    void Update()
    {
        if (enemyCanMove)
        {
            if (!attackMode)
            {
                Move();
            }

            if (!InsideofLimits() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                SelectTarget();
            }

            if (inRange)
            {
                EnemyLogic();
            }
        }
        
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if(distance > attackDistacne)
        {
            
            StopAttack();
        }

        else if (attackDistacne >= distance && cooling == false)
        {
            Attack();
        }
        if (cooling)
        {
            Cooldown();
            animator.SetBool("Attack", false);
        }
    }

    private void Move()
    {
        animator.SetBool("Running", true);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        }
    }

    void Attack()
    {
        hitbox.SetActive(true);
        timer = intTimer; //Reset timer when Playr enter Attack range
        attackMode = true; // To check if Enemy can still attack or not

        animator.SetBool("Running", false);
        animator.SetBool("Attack", true);

    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if(timer <=0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }
    void StopAttack()
    {
        hitbox.SetActive(false);
        cooling = false;
        attackMode = false;
        animator.SetBool("Attack", false);
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if(distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if(transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }
        transform.eulerAngles = rotation;
    }

    public void TriggerDamage()
    {
        hitbox.GetComponent<EnemyHitbox>().haveAttacked = false;
    }
    
    public void FreezeMotion()
    {
        enemyCanMove = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void UnFreezeMotion()
    {
        enemyCanMove = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

    public void ApplyDamage(int damage)
    {
        if (enemyCanMove)
        {
            enemyHealth -= damage;
            if (enemyHealth > 0)
            {
                animator.SetTrigger("Hurt");
            }
            
        }
        if (enemyHealth <= 0)
        {
            StartCoroutine(KillEnemy());
        }

    }

    IEnumerator KillEnemy()
    {
        if (enemyCanMove)
        {
            FreezeMotion();

            animator.SetTrigger("Death");

            yield return new WaitForSeconds(3.0f);

            Destroy(this.gameObject);
        }
       
    }

    public void PlaySound(AudioClip clip)
    {
        audio.PlayOneShot(clip);
    }
}
