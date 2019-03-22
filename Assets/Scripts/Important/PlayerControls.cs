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
    public float magicCooldown;
    public float TimeOfItemOneUse, itemOneCooldown = 1;
    public float TimeOfItemTwoUse, itemTwoCooldown = 1;


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
    private bool givenTripleJump;

    [HideInInspector] public float timeOfAttack, timeOfMagic;

    private bool facingRight;
    private bool facingLeft;

    private bool useFireball;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        cameraMovement = FindObjectOfType<CameraMovement>();
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        leftDetector = gameObject.transform.Find("Left Detector").GetComponent<BoxCollider2D>();
        rightDetector = gameObject.transform.Find("Right Detector").GetComponent<BoxCollider2D>();
        EnemyMonitor[] enemyColliders = FindObjectsOfType<EnemyMonitor>();
        foreach (EnemyMonitor m in enemyColliders)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), m.GetComponent<BoxCollider2D>());
        }

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

        if (PersistantGameManager.Instance.tripleJump == true && !givenTripleJump)
        {
            totalJumps += 1;
            givenTripleJump = true;
        }

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

        if (Input.GetKeyDown(KeyCode.Q) && PersistantGameManager.Instance.fireball)
        {
            //on attack press, fireball function and set cooldown
            StartCoroutine(Fireball());
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

        if (Input.GetKeyDown(KeyCode.H) && PersistantGameManager.Instance.amountOfItems[PersistantGameManager.Instance.equippedItemOne] > 0 && Time.time > (TimeOfItemOneUse + itemOneCooldown))
        {
            TimeOfItemOneUse = Time.time;
            UseItem(PersistantGameManager.Instance.equippedItemOne);
        }
        if (Input.GetKeyDown(KeyCode.J) && PersistantGameManager.Instance.amountOfItems[PersistantGameManager.Instance.equippedItemTwo] > 0 && Time.time > (TimeOfItemTwoUse + itemTwoCooldown))
        {
            TimeOfItemTwoUse = Time.time;
            UseItem(PersistantGameManager.Instance.equippedItemTwo);
        }


        playerDamage = PersistantGameManager.Instance.currentWeapon.itemDamage;
        attackSpeed = PersistantGameManager.Instance.currentWeapon.itemSpeed;
        range = PersistantGameManager.Instance.currentWeapon.itemRange;
        defence = PersistantGameManager.Instance.currentArmour.defence;

    }

    //detects if player hits ground, which re enables ability to jump
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            canJump = true;
            shouldJump = false;
            currentJumps = 0;
            // tells animator to stop playing Jump animation
            animator.SetBool("IsJumping", false);

        }
        else if (collision.gameObject.tag == "Wall" && PersistantGameManager.Instance.gripWalls)
        {
            collision.collider.sharedMaterial = (PhysicsMaterial2D)Resources.Load("PhysicsMaterials/WallGrippy");
            canJump = true;
            shouldJump = false;
            currentJumps = 0;
            // tells animator to stop playing Jump animation
            animator.SetBool("IsJumping", false);

        }
        else if (collision.gameObject.tag == "Wall" && !PersistantGameManager.Instance.gripWalls)
        {
            collision.collider.sharedMaterial = (PhysicsMaterial2D)Resources.Load("PhysicsMaterials/WallSlippery");
            shouldJump = false;

        }




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

    IEnumerator Fireball()
    {
        if (facingLeft)
        {
            useFireball = true;
            leftDetector.enabled = true;
            yield return null;
            useFireball = false;
            leftDetector.enabled = false;


        }
        if (facingRight)
        {
            useFireball = true;
            rightDetector.enabled = true;
            yield return null;
            useFireball = false;
            rightDetector.enabled = false;

        }
    }

    public float CalculatePlayerHealing()
    {
        return playerDamage * PersistantGameManager.Instance.currentLeechMultiplier;
    }


    public float CalculatePlayerDamage()
    {
        if (useFireball)
        {
            return (CalculateFireballDamage() * PersistantGameManager.Instance.currentAttackMultiplier);
        }
        else
        {
            float timeSinceAttack = Time.time - timeOfAttack;
            if (timeSinceAttack > attackSpeed)
            {
                return playerDamage * 1.2f * PersistantGameManager.Instance.currentAttackMultiplier;
            }
            float newPlayerDamage = playerDamage * (timeSinceAttack / attackSpeed);
            timeOfAttack = Time.time;
            return newPlayerDamage * PersistantGameManager.Instance.currentAttackMultiplier;
        }
    }

    private float CalculateFireballDamage()
    {
        float timeSinceMagic = Time.time - timeOfMagic;
        float magicDamage;
        if (timeSinceMagic > magicCooldown)
        {
            timeOfMagic = Time.time;
            magicDamage = playerDamage * 2f;
            return magicDamage * 2f;

        }
        return magicDamage = 0;


    }
    private void UseItem(string type)
    {
        if(type == "20%H" && Time.timeScale != 0 && PersistantGameManager.Instance.player.currentHealth != PersistantGameManager.Instance.player.totalHealth)
        { 
                currentHealth += totalHealth * 0.2f;
                if (currentHealth > totalHealth)
                {
                    currentHealth = totalHealth;
                }
                PersistantGameManager.Instance.amountOfItems["20%H"] -= 1;
                PersistantGameManager.Instance.healthPotionUseTime = Time.time;
            
        }
        else if (type == "50%H" && Time.timeScale != 0 && PersistantGameManager.Instance.player.currentHealth != PersistantGameManager.Instance.player.totalHealth)
        {

                currentHealth += totalHealth * 0.5f;
                if (currentHealth > totalHealth)
                {
                    currentHealth = totalHealth;
                }
                PersistantGameManager.Instance.amountOfItems["50%A"] -= 1;
                PersistantGameManager.Instance.healthPotionUseTime = Time.time;

        }
        else if (type == "100%H" && Time.timeScale != 0 && PersistantGameManager.Instance.player.currentHealth != PersistantGameManager.Instance.player.totalHealth)
        {

                currentHealth = totalHealth;
                PersistantGameManager.Instance.amountOfItems["100%H"] -= 1;
                PersistantGameManager.Instance.healthPotionUseTime = Time.time;


        }

        if (type == "20%A" && Time.timeScale != 0)
        {
            if (!PersistantGameManager.Instance.potionIsActive)
            {
                PersistantGameManager.Instance.activePotionType = "Attack";
                PersistantGameManager.Instance.potionIsActive = true;
                PersistantGameManager.Instance.currentAttackMultiplier = 1.2f;
                PersistantGameManager.Instance.timeOfAttackMultiplierChange = Time.time;
                PersistantGameManager.Instance.amountOfItems["20%A"] -= 1;
                PersistantGameManager.Instance.potionCoolDownTime = 30;
            }
        }
        else if (type == "50%A" && Time.timeScale != 0)
        {
            if (!PersistantGameManager.Instance.potionIsActive)
            {
                PersistantGameManager.Instance.activePotionType = "Attack";
                PersistantGameManager.Instance.potionIsActive = true;
                PersistantGameManager.Instance.currentAttackMultiplier = 1.5f;
                PersistantGameManager.Instance.timeOfAttackMultiplierChange = Time.time;
                PersistantGameManager.Instance.amountOfItems["50%A"] -= 1;
                PersistantGameManager.Instance.potionCoolDownTime = 30;
            }
        }
        else if (type == "100%A" && Time.timeScale != 0)
        {
            if (!PersistantGameManager.Instance.potionIsActive)
            {
                PersistantGameManager.Instance.activePotionType = "Attack";
                PersistantGameManager.Instance.potionIsActive = true;
                PersistantGameManager.Instance.currentAttackMultiplier = 2;
                PersistantGameManager.Instance.timeOfAttackMultiplierChange = Time.time;
                PersistantGameManager.Instance.amountOfItems["100%A"] -= 1;
                PersistantGameManager.Instance.potionCoolDownTime = 30;

            }
        }

        if (type == "20%L" && Time.timeScale != 0)
        {
            if (!PersistantGameManager.Instance.potionIsActive)
            {
                PersistantGameManager.Instance.activePotionType = "Leech";
                PersistantGameManager.Instance.potionIsActive = true;
                PersistantGameManager.Instance.currentLeechMultiplier = 0.2f;
                PersistantGameManager.Instance.timeOfLeechMultiplierChange = Time.time;
                PersistantGameManager.Instance.amountOfItems["20%L"] -= 1;
                PersistantGameManager.Instance.potionCoolDownTime = 30;
            }
        }
        else if (type == "50%L" && Time.timeScale != 0)
        {
            if (!PersistantGameManager.Instance.potionIsActive)
            {
                PersistantGameManager.Instance.activePotionType = "Leech";
                PersistantGameManager.Instance.potionIsActive = true;
                PersistantGameManager.Instance.currentLeechMultiplier = 0.5f;
                PersistantGameManager.Instance.timeOfLeechMultiplierChange = Time.time;
                PersistantGameManager.Instance.amountOfItems["50%L"] -= 1;
                PersistantGameManager.Instance.potionCoolDownTime = 30;
            }
        }
        else if (type == "100%L" && Time.timeScale != 0)
        {
            if (!PersistantGameManager.Instance.potionIsActive)
            {
                PersistantGameManager.Instance.activePotionType = "Leech";
                PersistantGameManager.Instance.potionIsActive = true;
                PersistantGameManager.Instance.currentLeechMultiplier = 1;
                PersistantGameManager.Instance.timeOfLeechMultiplierChange = Time.time;
                PersistantGameManager.Instance.amountOfItems["100%L"] -= 1;
                PersistantGameManager.Instance.potionCoolDownTime = 30;

            }
        }

    }
}
