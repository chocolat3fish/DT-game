using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* For double jumps:
 * vars for keeping track of total possible jumps and amount done so far
 * cancels jump action if already jumped to capacity
 * increments current jumps after a jump, resets on ground collision
 * also has option for less velocity if jump was not the first.
 */

public class PlayerControls : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private CameraMovement cameraMovement;

    public float range = 1;

    public float moveSpeed;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    public int totalJumps;
    private Rigidbody2D playerRigidbody;

    //reliant on PersistantGameManager and PlayerMonitor
    public float playerDamage;
    public float attackSpeed;
    

    private BoxCollider2D rightDetector;
    private BoxCollider2D leftDetector;
    public float nextAttack;
    private Vector2 detectorPos;
    public float currentHealth;
    public float totalHealth;
    public float defence;

    private Vector2 playerInput;
    private bool canJump;
    private bool shouldJump;
    private int currentJumps;

    [HideInInspector] public float timeOfAttack = 0;

    private bool facingRight;
    private bool facingLeft;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        cameraMovement = FindObjectOfType<CameraMovement>();
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        leftDetector = gameObject.transform.Find("Left Detector").GetComponent<BoxCollider2D>();
        rightDetector = gameObject.transform.Find("Right Detector").GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

        
        // stops player from flipping everywhere
        playerRigidbody.freezeRotation = true;
        leftDetector.enabled = false;
        rightDetector.enabled = false;

        currentJumps = 0;

        currentHealth = totalHealth;
    }


    void Update()
    {
        // Recieves player input as x and y vector
        playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //checks if player pressed jump key, tells fixed update to do a jump
        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            canJump = false;
            shouldJump = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            //on attack press, shoot function and set cooldown
            StartCoroutine(Detect());
            
            
        }
        //changes x axis speed and keeps current y axis velocity
        if (playerInput != Vector2.zero)
        {
            playerRigidbody.velocity = new Vector2(playerInput.x * moveSpeed, playerRigidbody.velocity.y);
        }
        //tells animator what the speed is as a positive value so it can then activate the running/walking animation
        animator.SetFloat("Speed", Mathf.Abs(playerRigidbody.velocity.x));

        //makes the character face the correct direction. and offests the camera depening on which way you are moving
        //determines whether facing left or right
        if (playerRigidbody.velocity.x < -0.01)
        {
            spriteRenderer.flipX = true;
            cameraMovement.offset.x = -2f;

            facingRight = false;
            facingLeft = true;

        }
        if (playerRigidbody.velocity.x > 0.01)
        {
            spriteRenderer.flipX = false;
            cameraMovement.offset.x = 2f;

            facingLeft = false;
            facingRight = true;

        }

        //if player moving exceedingly fast, pushes the camera ahead to keep player visible
        if (playerRigidbody.velocity.y < -(jumpSpeed + 2))
        {
            cameraMovement.offset.y = -4.5f;
        }
        if (playerRigidbody.velocity.y > jumpSpeed + 2)
        {
            cameraMovement.offset.y = 4.5f;
        }
        if (playerRigidbody.velocity.y < 0.05f && playerRigidbody.velocity.y > -0.05)
        {
            cameraMovement.offset.y = 0f;
        }



        //performs jump if was pressed and haven't jumped too many times, adds upward force.

        if (currentJumps >= totalJumps)
        {
            shouldJump = false;
        }
        else if (shouldJump)
        {
            //tells animator to start jumping animation
            animator.SetBool("IsJumping", true);

            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
            playerRigidbody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            shouldJump = false;
            canJump = true;
            currentJumps++;
        }
        else if (shouldJump && currentJumps > 0)
        {
            playerRigidbody.AddForce(Vector2.up * doubleJumpSpeed, ForceMode2D.Impulse);
            shouldJump = false;
            canJump = true;
            currentJumps++;
        }

        playerDamage = PersistantGameManager.Instance.currentWeapon.itemDamage;
        attackSpeed = PersistantGameManager.Instance.currentWeapon.itemSpeed;
        range = PersistantGameManager.Instance.currentWeapon.itemRange;
        defence = PersistantGameManager.Instance.currentArmour.defence;
        
    }

    //detects if player hits ground, which re enables ability to jump
    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
        shouldJump = false;
        currentJumps = 0;
        // tells animator to stop playing Jump animation
        animator.SetBool("IsJumping", false);


    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        animator.SetBool("IsJumping", true);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        animator.SetBool("IsJumping", false);
    }

    //shoots projectile
    IEnumerator Detect()
    {
        if (facingLeft)
        {
            leftDetector.enabled = true;
            yield return null;
            leftDetector.enabled = false;
        }
        if (facingRight)
        {
            rightDetector.enabled = true;
            yield return null;
            rightDetector.enabled = false;

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            EnemyMonitor enemy = collision.gameObject.GetComponent<EnemyMonitor>();
            float newPlayerDamage = calculatePlayerDamage();
            enemy.currentHealth -= newPlayerDamage;
        }

    }
    private float calculatePlayerDamage()
    {
        float timeSinceAttack = Time.time - timeOfAttack;
        if(timeSinceAttack > attackSpeed)
        {
            timeOfAttack = Time.time;
            return playerDamage * 1.2f;
        }
        float newPlayerDamage = playerDamage * (timeSinceAttack / attackSpeed);
        timeOfAttack = Time.time;
        return newPlayerDamage;
    }

}