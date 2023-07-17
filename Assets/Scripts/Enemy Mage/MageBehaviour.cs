using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBehaviour : MonoBehaviour
{
    #region Public Variables
    public float shootDistance;
    public float retreatDistance;
    public float retreatSpeed;
    public float moveSpeed;
    public float timer;
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;
    public GameObject hitbox;
    public int enemyHealth;
    [HideInInspector] public bool enemyCanMove = true;
    public GameObject projectile;
    public AudioSource audio;
    #endregion

    #region Private variables
    private Animator animator;
    private float distance;
    private bool shootMode;
    private bool cooling;
    private float intTimer;
    #endregion

    private void Awake()
    {
        SelectTarget();
        intTimer = timer;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCanMove)
        {
            if (!shootMode)
            {
                Move();
            }

            if(!InsideofLimits() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
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
        Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

        if (distance > shootDistance)
        {
            StopShoot();
        }
        else if (distance <= shootDistance &&  cooling==false)
        {
            Shoot();

        }
        if (cooling)
        {
            CoolDown();
            animator.SetBool("Attack", false);
        }

    }

    void Move()
    {
        animator.SetBool("Running", true);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        }
    }

    void Shoot()
    {
        transform.position = this.transform.position;
        timer = intTimer;
        shootMode = true;

        animator.SetBool("Running",false);
        animator.SetBool("Attack", true);
    }

    void StopShoot()
    {
        cooling = false;
        shootMode = false;
        animator.SetBool("Attack", false);
    }

    void CoolDown()
    {
        timer -= Time.deltaTime;

        if(timer <=0 && cooling && shootMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
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
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180f;
            
        }
        else
        {
            rotation.y = 0f;
            
        }
        transform.eulerAngles = rotation;
    }

    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.transform.position.x;
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    public void Fire()
    {
        Instantiate(projectile, hitbox.transform.position, hitbox.transform.rotation);
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
