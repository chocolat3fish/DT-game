using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


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

    public PlayerStats playerStats;

    public string deathScene;

    //reliant on PersistantGameManager and PlayerMonitor
    public float playerDamage;
    public double attackSpeed;

    public float magicCooldown;

    public float TimeOfItemOneUse, itemOneCooldown = 1;
    public float TimeOfItemTwoUse, itemTwoCooldown = 1;


    private BoxCollider2D rightDetector;
    private BoxCollider2D leftDetector;
    public float nextAttack;
    private Vector2 detectorPos;
    private float _currentHealth;
    public float currentHealth
    {
        get { return _currentHealth; }
        set
        {
            if (!PersistantGameManager.Instance.GodMode)
            {
                _currentHealth = value;
            }
            if (value < _currentHealth)
            {
                //StartCoroutine(Shake());
            }

        }
    }
    public float attackTime;
    public float stockHealth;
    public float totalHealth;


    private Vector2 playerInput;
    private bool canJump;
    private bool shouldJump;
    [HideInInspector]
    public int currentJumps;
    private bool givenTripleJump;

    [HideInInspector] public float timeOfAttack, timeOfMagic = 0;

    private bool facingRight;
    private bool facingLeft;

    private bool useFireball;

    private static System.Random random = new System.Random();

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        cameraMovement = FindObjectOfType<CameraMovement>();
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        leftDetector = gameObject.transform.Find("Left Detector").GetComponent<BoxCollider2D>();
        rightDetector = gameObject.transform.Find("Right Detector").GetComponent<BoxCollider2D>();
        EnemyMonitor[] enemyColliders = FindObjectsOfType<EnemyMonitor>();
        StartCoroutine(HealthRegen());
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
        stockHealth = (float)(54f * Math.Pow(PersistantGameManager.Instance.playerStats.playerLevel, 2) + 10f);
        totalHealth = stockHealth + (stockHealth * PersistantGameManager.Instance.totalHealthMulti);
        currentHealth = totalHealth;
    }


    void Update()
    {


        if (currentHealth <= 0)
        {
            // when player dies, loads the game over scene
            SceneManager.LoadScene(deathScene);
        }

        if (Input.GetMouseButtonDown(1))
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            playerRigidbody.velocity = new Vector2(0, 0);
            /*Vector2 mousePos = Input.mousePosition;
            Vector2 newPos = mousePos / (new Vector2(18, 10));
            newPos = newPos -
            */

        }

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

        if (Input.GetKeyDown(KeyCode.Q) && PersistantGameManager.Instance.hasSmite && Time.time >= timeOfMagic + magicCooldown)
        {
            //on attack press, fireball function and set cooldown
            StartCoroutine(Fireball());
            if (PersistantGameManager.Instance.skillLevels["SmiteDuration"] > 0)
            {
                //activate smite's duration and record time.
                PersistantGameManager.Instance.timeOfAbility = Time.time;
                timeOfMagic = Time.time;
                SmiteDuration();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && PersistantGameManager.Instance.damageResist && Time.time >= timeOfMagic + magicCooldown)
        {
            //activate damage resist and record time
            PersistantGameManager.Instance.timeOfAbility = Time.time;
            timeOfMagic = Time.time;
            ResistDamage();
        }


        //changes x axis speed and keeps current y axis velocity
        if (playerInput != Vector2.zero)
        {
            playerRigidbody.velocity = new Vector2(playerInput.x * moveSpeed * PersistantGameManager.Instance.moveSpeedMulti, playerRigidbody.velocity.y);
        }
        //tells animator what the speed is as a positive value so it can then activate the running/walking animation
        animator.SetFloat("Speed", Mathf.Abs(playerRigidbody.velocity.x));


        //Increases damage resist while moving based on the related skill.
        if (playerRigidbody.velocity.x > 0.01 && PersistantGameManager.Instance.skillLevels["DefenceWithMovement"] > 0)
        {
            if (PersistantGameManager.Instance.currentActiveAbility == "Turtle")
            {
                //float stockDamageResist = PersistantGameManager.Instance.turtleResistMulti;
                //float newMoveResist =  ( PersistantGameManager.Instance.movementResistMulti * (playerRigidbody.velocity.x / 10));

                //PersistantGameManager.Instance.damageResistMulti = (stockDamageResist * newMoveResist);
                PersistantGameManager.Instance.damageResistMulti = (1 - PersistantGameManager.Instance.movementResistMulti * (playerRigidbody.velocity.x / 10)) * (PersistantGameManager.Instance.turtleResistMulti * PersistantGameManager.Instance.turtleMultiMulti);
            }
            else
            {
                PersistantGameManager.Instance.damageResistMulti =  1 - PersistantGameManager.Instance.movementResistMulti * (playerRigidbody.velocity.x / 10);
            }

        }

        if (playerRigidbody.velocity.x < -0.01 && PersistantGameManager.Instance.skillLevels["DefenceWithMovement"] > 0)
        {
            if (PersistantGameManager.Instance.currentActiveAbility == "Turtle")
            {
                //float stockDamageResist = PersistantGameManager.Instance.turtleResistMulti;
                //float newMoveResist = (PersistantGameManager.Instance.movementResistMulti * (playerRigidbody.velocity.x / 10) * -1);

                //PersistantGameManager.Instance.damageResistMulti = (stockDamageResist * newMoveResist);
                PersistantGameManager.Instance.damageResistMulti = (1 - PersistantGameManager.Instance.movementResistMulti * (playerRigidbody.velocity.x / 10) * -1) * (PersistantGameManager.Instance.turtleResistMulti * PersistantGameManager.Instance.turtleMultiMulti);
            }

            else
            {
                PersistantGameManager.Instance.damageResistMulti = 1 - PersistantGameManager.Instance.movementResistMulti * (playerRigidbody.velocity.x / 10) * -1;
            }
        }

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
            cameraMovement.offset.y = -5f;
        }
        //if fast downward movement suddenly snaps to upward velocity (fixes old problem)
        if (playerRigidbody.velocity.y < -(jumpSpeed + 2) && Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            cameraMovement.offset.y = 2f;
        }
        //if falling but not too fast so you can see the ground
        if (playerRigidbody.velocity.y < -jumpSpeed)
        {
            cameraMovement.offset.y = -4f;
        }
        // if moving up, camera goes ahead of you
        if (playerRigidbody.velocity.y > jumpSpeed + 2)
        {
            cameraMovement.offset.y = 2f;
        }
        //if not enough velocity to warrant a camera offset
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
            playerRigidbody.AddForce(Vector2.up * jumpSpeed * PersistantGameManager.Instance.jumpHeightMulti, ForceMode2D.Impulse);
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
        /*
        if (Input.GetKeyDown(KeyCode.H) && PersistantGameManager.Instance.amountOfConsumables[PersistantGameManager.Instance.equippedItemOne] > 0 && Time.time > (TimeOfItemOneUse + itemOneCooldown))
        {
            TimeOfItemOneUse = Time.time;
            UseItem(PersistantGameManager.Instance.equippedItemOne);
        }
        if (Input.GetKeyDown(KeyCode.J) && PersistantGameManager.Instance.amountOfConsumables[PersistantGameManager.Instance.equippedItemTwo] > 0 && Time.time > (TimeOfItemTwoUse + itemTwoCooldown))
        {
            TimeOfItemTwoUse = Time.time;
            UseItem(PersistantGameManager.Instance.equippedItemTwo);
        }
        */



        playerDamage = PersistantGameManager.Instance.currentWeapon.itemDamage;
        attackSpeed = PersistantGameManager.Instance.currentWeapon.trueItemSpeed;
        range = PersistantGameManager.Instance.currentWeapon.itemRange;

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

    public void ResistDamage()
    {
        PersistantGameManager.Instance.damageResistMulti = PersistantGameManager.Instance.turtleResistMulti * PersistantGameManager.Instance.turtleMultiMulti;
        PersistantGameManager.Instance.currentActiveAbility = "Turtle";
        PersistantGameManager.Instance.abilityIsActive = true;
        PersistantGameManager.Instance.abilityDuration = PersistantGameManager.Instance.damageResistDuration;
    }

    public void SmiteDuration()
    {
        PersistantGameManager.Instance.currentAttackMultiplier = 1 + PersistantGameManager.Instance.smiteDurationMulti;
        PersistantGameManager.Instance.currentActiveAbility = "Smite";
        PersistantGameManager.Instance.abilityIsActive = true;
        PersistantGameManager.Instance.abilityDuration = PersistantGameManager.Instance.smiteDuration;
    }

    public float CalculatePlayerHealing()
    {
        return playerDamage * PersistantGameManager.Instance.lifeStealMulti;
    }


    public double CalculatePlayerDamage()
    {
        if (useFireball)
        {
            if (playerRigidbody.velocity.y > 0f || playerRigidbody.velocity.y < 0f)
            {
                return CalculateHighDamage() * PersistantGameManager.Instance.currentAttackMultiplier * PersistantGameManager.Instance.airAttackMulti;
            }
            return CalculateHighDamage() * PersistantGameManager.Instance.currentAttackMultiplier;
        }
        else
        {
            int randomNumber = random.Next(1, 100);
            if(randomNumber <= PersistantGameManager.Instance.instantKillChance)
            {
                Debug.Log("big damage");
                return playerDamage = (float)Math.Pow(100, 4);
            }

            float timeSinceAttack = Time.time - timeOfAttack;
            if (timeSinceAttack > attackSpeed)
            {
                timeOfAttack = Time.time;

                if (playerRigidbody.velocity.y > 0f || playerRigidbody.velocity.y < 0f)
                {
                    return playerDamage * 1.2f * PersistantGameManager.Instance.currentAttackMultiplier * PersistantGameManager.Instance.airAttackMulti;
                }
                return playerDamage * 1.2f * PersistantGameManager.Instance.currentAttackMultiplier;
            }
            double newPlayerDamage = playerDamage * ((timeSinceAttack / attackSpeed) / 2);
            timeOfAttack = Time.time;
            if (playerRigidbody.velocity.y > 0f || playerRigidbody.velocity.y < 0f)
            {
                return newPlayerDamage * PersistantGameManager.Instance.currentAttackMultiplier * PersistantGameManager.Instance.airAttackMulti;
            }
            return newPlayerDamage * PersistantGameManager.Instance.currentAttackMultiplier; 
        }
    }

    private float CalculateHighDamage()
    {
        //float timeSinceMagic = Time.time - timeOfMagic;
        float magicDamage;
        //if (timeSinceMagic > magicCooldown)

        timeOfMagic = Time.time;
        magicDamage = playerDamage * 2f * PersistantGameManager.Instance.smiteDamageMulti;
        return magicDamage * 2f;



    }
    private void UseItem(string type)
    {
        if (type == "20%H" && Time.timeScale != 0 && PersistantGameManager.Instance.player.currentHealth != PersistantGameManager.Instance.player.totalHealth)
        {
            currentHealth += totalHealth * 0.2f;
            if (currentHealth > totalHealth)
            {
                currentHealth = totalHealth;
            }
            PersistantGameManager.Instance.amountOfConsumables["20%H"] -= 1;
            PersistantGameManager.Instance.healthPotionUseTime = Time.time;

        }

        else if (type == "50%H" && Time.timeScale != 0 && PersistantGameManager.Instance.player.currentHealth != PersistantGameManager.Instance.player.totalHealth)
        {

            currentHealth += totalHealth * 0.5f;
            if (currentHealth > totalHealth)
            {
                currentHealth = totalHealth;
            }
            PersistantGameManager.Instance.amountOfConsumables["50%A"] -= 1;
            PersistantGameManager.Instance.healthPotionUseTime = Time.time;

        }
        else if (type == "100%H" && Time.timeScale != 0 && PersistantGameManager.Instance.player.currentHealth != PersistantGameManager.Instance.player.totalHealth)
        {

            currentHealth = totalHealth;
            PersistantGameManager.Instance.amountOfConsumables["100%H"] -= 1;
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
                PersistantGameManager.Instance.amountOfConsumables["20%A"] -= 1;
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
                PersistantGameManager.Instance.amountOfConsumables["50%A"] -= 1;
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
                PersistantGameManager.Instance.amountOfConsumables["100%A"] -= 1;
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
                PersistantGameManager.Instance.amountOfConsumables["20%L"] -= 1;
                PersistantGameManager.Instance.potionCoolDownTime = 30;
            }
        }
        /*
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
        */

    }
    IEnumerator Shake()
    {
        Vector2 orignalPos = transform.position;
        for (int i = 0; i < 7; i++)
        {
            if (i % 2 == 0)
            {
                transform.position = new Vector2(orignalPos.x + 0.02f, orignalPos.y);
            }
            else
            {
                transform.position = new Vector2(orignalPos.x - 0.02f, orignalPos.y);
            }
            yield return null;
            yield return null;
        }
    }
    IEnumerator HealthRegen()
    {
        while (true)
        {
            if (attackTime + 10f < Time.time)
            {
                if (currentHealth < totalHealth)
                {
                    currentHealth += totalHealth * 0.002f;
                }
                if (currentHealth > totalHealth)
                {
                    currentHealth = totalHealth;
                }
            }
            yield return new WaitForSeconds(0.033f);
        }
    }
}
