using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject playerSprite;

    public float moveSpeed;
    public float jumpSpeed;
    public Rigidbody2D playerRigidbody;

    private Vector2 playerInput;
    private bool canJump;
    private bool shouldJump;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D> ();
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

        //performs jump if was pressed, adds upward force.
        if (shouldJump)
        {
            playerRigidbody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            shouldJump = false;
        }
      
    }

    //detects if player hits ground, which re enables ability to jump
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        canJump = true;
        shouldJump = false;
        
    }
}
