using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//******MUST BE CHILDED TO THE GAMEOBJECT ACTING AS THE PLAYER******\\
//******MUST HAVE TO GAMEOBJECTS WITH 2 BOXCOLLIDER2Ds CHILDED TO THE PLAYER GAMEOBJECT******\\

//Controls the size of the detectors to allow range changing
public class DetectorMonitors : MonoBehaviour
{
    //The collider2Ds themselves
    private BoxCollider2D leftCollider;
    private BoxCollider2D rightCollider;

    private BoxCollider2D playerCollider2D;
    private PlayerControls playerControls;
    
    void Awake()
    {
        //Finds the Components
        leftCollider = gameObject.transform.Find("Left Detector").GetComponent<BoxCollider2D>();
        rightCollider = gameObject.transform.Find("Right Detector").GetComponent<BoxCollider2D>();
        playerControls = gameObject.GetComponent<PlayerControls>();
        playerCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    
    void Update()
    {
        //Changes the size of the colliders based on the range of the players current weapon
        leftCollider.size = new Vector2(playerControls.range, playerControls.gameObject.transform.localScale.y);
        rightCollider.size = new Vector2(playerControls.range, playerControls.gameObject.transform.localScale.y);

        //Keeps the box coliders 0.1 units of the main players box colider regardless of the size
        leftCollider.offset = new Vector2((playerControls.range / -2) - ((playerCollider2D.size.x / 2) + 0.1f), leftCollider.offset.y);
        rightCollider.offset = new Vector2((playerControls.range / 2) + ((playerCollider2D.size.x / 2) + 0.1f), leftCollider.offset.y);
    }
}
