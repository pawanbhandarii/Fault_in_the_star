using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //player controls
    [Range(0.0f, 10f)] //creates a slider in the editor and sets limit from 0 to 10
    public float moveSpeed = 3f;

    public float jumpForce = 600f;

    //player health
    public int playerHealth = 100;

    //LayerMask to Detemine what is ground for the player
    public LayerMask whatIsGround;

    //Transform just below the feet for checking if the player is grounded
    public Transform groundCheck;

    //can player move?
    [HideInInspector]
    public bool playerCanMove = true;

    // raycast distance for climbing
    public float rayDistance = 5;

    //Layermask for checking what is ladder
    public LayerMask whatIsLadder;

    public GameObject hitbox;

    public GameObject healthBar;


    //SFXs
    public AudioClip coinSFX;
    public AudioClip deathSFX;
    public AudioClip fallSFX;
    public AudioClip jumpSFX;
    public AudioClip victorySFX;
    public AudioClip hurtSFX;
    public AudioClip healthSFX;

    //private variables

    //stores refernces to component of the gameObject

    Transform transform;
    Rigidbody2D rigidbody;
    Animator animator;
    AudioSource audio;



    // stores player motion
    float horizonalVelocity;
    float verticalVelocity;

    // player tracking
    bool facingRight = true;
    bool isGrounded = false;
    bool isRunning = false;
    bool canDoubleJump = false;
    bool isClimbing = false;

    //stores the laayer the player is set in
    int playerLayer;

    //stores the layer the platform is set in
    int platformLayer;

    //stores the collider for raycasts
    RaycastHit2D hitInfo;

    

    private void Awake()
    {
        //gets reference to the transform component of the gameObject
        transform = gameObject.GetComponent<Transform>();

        //gets reference to the rigidbody2d component of the gameObject
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        if (rigidbody == null)
        {
            Debug.LogError("RigidBody2D Component missing"); //Display error in the console if the rigidbody component is missing
        }

        //gets reference to the animator component of the gameObject
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator Component missing"); //Display error in the console if the animator component is missing
        }

        audio = GetComponent<AudioSource>();
        if (audio == null)
        {
            Debug.LogWarning("Audio Component missing"); //Display error in the console if the animator component is missing
            audio = gameObject.AddComponent<AudioSource>();
        }

        playerLayer = this.gameObject.layer;

        platformLayer = LayerMask.NameToLayer("Platform");

        healthBar.GetComponent<HealthBar>().SetMaxHealth(playerHealth);

        FindObjectOfType<GameManager>().SetHealth(playerHealth);
        FindObjectOfType<GameManager>().ResfreshHealth(playerHealth);
        

    }

    // Update is called once per frame
    void Update()
    {
        //exiits if the player cannot move or the game is paused
        if(!playerCanMove || Time.timeScale == 0f)
        {
            return;
        }

        //determines the horizonal velocity based on the horizontal input given by the user
        horizonalVelocity = Input.GetAxis("Horizontal");

        //Determine if running based on the horizonal movemnt
        if (horizonalVelocity != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        //set the running animation state
        animator.SetBool("Running", isRunning);

        //gets the veelocity form the rigidbody
        verticalVelocity = rigidbody.velocity.y;

        //Check to see if the character is grounded by raycasting from the player
        //to check whether ground Check has colided with gameObject in whatIsGround layer

        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, whatIsGround);
        
        //Set ground state animation 
        animator.SetBool("Grounded", isGrounded);

        //checking if the player is jumping
        if (isGrounded)
        {
            canDoubleJump = true;
        }
        if(isGrounded && Input.GetButtonDown("Jump"))
        {
            DoJump();
        }else if(canDoubleJump && Input.GetButtonDown("Jump"))
        {
            DoJump();
            canDoubleJump = false;
            
        }

        //if the payer has released jump in mid air and the player doesnot fall
        //then the verticl velocity is set to 0 and the player startss to fall from gravity

        if(Input.GetButtonUp("Jump")&& verticalVelocity > 0f)
        {
            verticalVelocity = 0f;
        }

        //Change the rigid bdy velocity
        rigidbody.velocity=new Vector2(horizonalVelocity * moveSpeed, verticalVelocity);

        // if moving up then don't collide with platform layer
        // this allows the player to jump up through things on the platform layer
        // NOTE: requires the platforms to be on a layer named "Platform"
        Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, (verticalVelocity > 0.0f));

       

    }

    private void FixedUpdate()
    {
        //Climbing a ladder

        //Checking the input from the user to ascend the ladder
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            hitInfo = Physics2D.Raycast(transform.position, Vector2.up, rayDistance, whatIsLadder);
        }
        //Checking the input from the user to descend the ladder
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            hitInfo = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, whatIsLadder);
        }

        //If the collider is a ladder then enabling it can climb
        if (hitInfo.collider != null)
        {
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
            {
                isClimbing = true;
            }
        }
        else
        {
            isClimbing = false;
        }

        //If the player is climbing then set the gravity scale to 0 so that the player doesnot drop and the vertical velocity to axis input
        if (isClimbing == true && hitInfo.collider != null)
        {
            verticalVelocity = Input.GetAxisRaw("Vertical");
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, verticalVelocity * moveSpeed);
            rigidbody.gravityScale = 0;
        }
        else
        {
            rigidbody.gravityScale = 3; // if not climbing revert back the gravity scale
        }

        animator.SetBool("Climbing", isClimbing); //set animation for climbing
    }

    void LateUpdate()
    {
        //get the current scale
        Vector3 localScale = transform.localScale;

        if (GetComponent<PlayerPush>().Pushing())
        {
            return;
        }
        else
        {
            if (horizonalVelocity > 0)//checking if moving right
            {
                facingRight = true;
            }
            else if (horizonalVelocity < 0)//checking if moving left
            {
                facingRight = false;
            }

            //check to see if scale x is right for the player
            // if not multiply by -1 to flip the sprite

            if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            {
                localScale.x *= -1;
            }

            //update the scale
            transform.localScale = localScale;
        }
        

        
    }

    //Detect collision and descide what to do
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            this.transform.parent = collision.transform; // The collided object is a moving platform then make the playr child of the platform
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "MovingPlatform")
        {
            this.transform.parent = null; // The collided object is a moving platform then remove the playr child of the platform
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.up * transform.localScale * rayDistance);
        }
        //Checking the input from the user to descend the ladder
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.down * transform.localScale * rayDistance);
        }
        
        
    }
    //Jumps the player
    void DoJump()
    {
        //resets current verticsl motion to 0 prior to jump
        verticalVelocity = 0f;

        //adds a force in upward direction
        rigidbody.AddForce(new Vector2(0,jumpForce));

        //PlaySound
        PlaySound(jumpSFX);
    }

    //do what to free the player

    void FreezeMotion()
    {
        playerCanMove = false;
        rigidbody.velocity = new Vector2(0, 0);
        rigidbody.isKinematic = true;
    }

    // do what to unfreeze the player

    void UnFreezeMotion()
    {
        playerCanMove = true;
        rigidbody.isKinematic = false;
    }

    //Play Sound
    public void PlaySound(AudioClip clip)
    {
        audio.PlayOneShot(clip);
    }

    //apply Damage to the Player

    public void AppplyDamage(int damage)
    {
        if (playerCanMove)
        {
            playerHealth -= damage; //apply damage to player's health
            if (playerHealth > 0)
            {
                animator.SetTrigger("Hurt");
                PlaySound(hurtSFX);
            }
        }

        GameManager.gm.ResfreshHealth(playerHealth);
        healthBar.GetComponent<HealthBar>().SetHealth(playerHealth);
        if (playerHealth <= 0) // player health is 0 the Kill the player 
        {
            PlaySound(deathSFX);
            StartCoroutine(KillPlayer());
        }
    }
    
    //If Player falls to death
    public void FallDead()
    {
        if (playerCanMove)
        {
            playerHealth = 0;
            GameManager.gm.ResfreshHealth(playerHealth);
            healthBar.GetComponent<HealthBar>().SetHealth(playerHealth);
            PlaySound(fallSFX);
            StartCoroutine(KillPlayer());
        }
    }

     //Kill Player Courintine Starts
    IEnumerator KillPlayer()
    {
        if (playerCanMove)
        {
            FreezeMotion();

            animator.SetTrigger("Death");

            yield return new WaitForSeconds(3.0f);

            print("hihi");
            if (GameManager.gm)
            {
                GameManager.gm.ResetGame();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void Respawn(Vector3 spawnloc)
    {
        UnFreezeMotion();
        playerHealth = 100;
        transform.parent = null;
        transform.position = spawnloc;
        animator.SetTrigger("Respawn");
        
    }

    public void Victory()
    {
        PlaySound(victorySFX);
        FreezeMotion();
        animator.SetTrigger("Victory");

        //do the game manager level compete stuff, if it is available
        if (GameManager.gm)
        {
            GameManager.gm.LoadVictoryPanel();
        }

    }

    public void CollectCoin(int amount)
    {
        PlaySound(coinSFX);
        //add the points through the game manager
        if (GameManager.gm)
        {
            GameManager.gm.AddPoints(amount);
        }
    }

    public void IncreaseHealth(int health_value)
    {
        playerHealth += health_value;
        healthBar.GetComponent<HealthBar>().SetHealth(playerHealth);
    }

    public bool Running()
    {
        return isRunning;
    }

    public bool Facing()
    {
        return facingRight;
    }

    public void SetHorizonal(float value)
    {
        horizonalVelocity = value;
    }

    public void DisableAttack()
    {
        hitbox.SetActive(false);
    }

    public void EnableAttack()
    {
        
        hitbox.SetActive(true);
    }
}
