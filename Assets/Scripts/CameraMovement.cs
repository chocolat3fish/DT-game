using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public OnSpawn onSpawn;
    public float smoothSpeed;
    public float adjustSmoothSpeed;
    public Vector3 offset;
    private Vector3 savedOffset;
    private Vector3 adjustedOffset;

    private void Start()
    {
        while (!onSpawn.StartCameraMovement)
        {

        }
        savedOffset = offset;
    }

    void FixedUpdate()
    {
        adjustedOffset = Vector3.Lerp(offset, savedOffset, adjustSmoothSpeed);
        Vector3 desiredPosition = player.position + adjustedOffset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        Vector3 smoothPositionFinal = new Vector3(smoothPosition.x, smoothPosition.y, -10f);

        transform.position = smoothPositionFinal;
        savedOffset = adjustedOffset;
        
        //smooths the movement of the camera as it follows the camera
        
    }
}
