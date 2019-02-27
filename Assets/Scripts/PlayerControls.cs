﻿using System.Collections;
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
    public GameObject playerSprite;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public CameraMovement cameraMovement;

    public float moveSpeed;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    public int totalJumps;
    public Rigidbody2D playerRigidbody;

    //Temporary for another script (TrainingEnemy)
    //will make this reliant on weapons later on

    public float playerDamage = 1f;

    //projectiles for either side, variables for original 
    //detectors are prefabs
    public BoxCollider2D DetectorRight;
    public BoxCollider2D DetectorLeft;
    public float attackSpeed;
    public float nextAttack;
    Vector2 detectorPos;


    private Vector2 playerInput;
    private bool canJump;
    private bool shouldJump;
    private int currentJumps;

    private bool facingRight;
    private bool facingLeft;


    void Start()
    {


        playerRigidbody = GetComponent<Rigidbody2D>();
        // stops player from flipping everywhere
        playerRigidbody.freezeRotation = true;
        DetectorLeft.enabled = false;
        DetectorRight.enabled = false;

        currentJumps = 0;
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

        if (Input.GetKeyDown(KeyCode.W) && Time.time > nextAttack)
        {
            //on attack press, shoot function and set cooldown
            nextAttack = Time.time + attackSpeed;
            StartCoroutine(Detect());
        }


    }

    void FixedUpdate()
    {
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
            Debug.Log("jumps: " + currentJumps);
        }
        else if (shouldJump && currentJumps > 0)
        {
            playerRigidbody.AddForce(Vector2.up * doubleJumpSpeed, ForceMode2D.Impulse);
            shouldJump = false;
            canJump = true;
            currentJumps++;
            Debug.Log(currentJumps);
        }

    }

    //detects if player hits ground, which re enables ability to jump
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        canJump = true;
        shouldJump = false;
        currentJumps = 0;
        // tells animator to stop playing Jump animation
        animator.SetBool("IsJumping", false);


    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        animator.SetBool("IsJumping", true);
        Debug.Log("Fall");
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
            DetectorLeft.enabled = true;
            yield return null;
            DetectorLeft.enabled = false;
            Debug.Log("Attack left");
        }
        if (facingRight)
        {
            DetectorRight.enabled = true;
            yield return null;
            DetectorRight.enabled = false;
            Debug.Log("Attack right");

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger");
        if (collision.gameObject.tag == "Enemy")
        {
            TrainingEnemy enemy = collision.gameObject.GetComponent<TrainingEnemy>();
            enemy.currentHealth -= playerDamage;
        }

    }

}