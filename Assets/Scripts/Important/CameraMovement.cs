using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//******SHOULD BE CHILDED TO CAMERA BUT DOESN'T HAVE TO BE******\\

//A script that controls the camera as it follows the player
public class CameraMovement : MonoBehaviour
{
    //the location of the player
    private PlayerControls player;
    //the speed at which it adjusts the camera to the new location
    public float smoothSpeed = 0.05f;
    //the speed at which it adjusts the offset when the offset changes
    public float adjustSmoothSpeed = 0.85f;
    //the offest of the camera
    public Vector2 offset;
    //At the end of each frame it saves the current offset this allows the script to "fade" to the next offest
    public Vector2 savedOffset;
    //The Camera
    private Camera camera;
    void Start()
    {
        camera = FindObjectOfType<Camera>();
        player = FindObjectOfType<PlayerControls>();
        savedOffset = offset;
    }

    void FixedUpdate()
    {
        //fades from the current offest to the new offset which can be change outside the script
        Vector2 adjustedOffset = Vector2.Lerp(offset, savedOffset, adjustSmoothSpeed);
        //sets the position the camera wants travel to based on the players position and the offset
        Vector2 desiredPosition = new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y) + adjustedOffset;
        //Sets the position the camera will travel this frame based off where it wants to go and the current poisiton of the player
        Vector2 smoothPosition = Vector2.Lerp(transform.position, desiredPosition, smoothSpeed);
        //Actully changes the position of the camera and sets the z to the current z of the camera
        camera.transform.position = new Vector3(smoothPosition.x, smoothPosition.y, camera.transform.position.z);
        //saves the current offset
        savedOffset = adjustedOffset;
        
    }
}
