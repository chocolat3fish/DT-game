using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* For double jumps:
 * vars for keeping track of total possible jumps and amount done so far
 * cancels jump action if already jumped to capacity
 * increments current jumps after a jump, resets on ground collision
 * also has option for less velocity if jump was not the first.
 */

public class PlayerMovement : MonoBehaviour
{
    public GameObject playerSprite;

    public float moveSpeed;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    public int totalJumps;
    public Rigidbody2D playerRigidbody;

    private Vector2 playerInput;
    private bool canJump;
    private bool shouldJump;
    private int currentJumps;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D> ();
        // stops player from flipping everywhere
        playerRigidbody.freezeRotation = true;

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

        
    }

    void FixedUpdate()
    {
        //changes x axis speed and keeps current y axis velocity
        if(playerInput != Vector2.zero)
        {
           playerRigidbody.velocity = new Vector2(playerInput.x * moveSpeed, playerRigidbody.velocity.y);
        }

        //performs jump if was pressed and haven't jumped too many times, adds upward force.

        if (currentJumps >= totalJumps)
        {
            shouldJump = false;
        }
        else if (shouldJump)
        {
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
        
    }
}
