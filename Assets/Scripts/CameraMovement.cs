using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;

    public float smoothSpeed;
    public Vector3 offset;


    void FixedUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        Vector3 smoothPositionFinal = new Vector3(smoothPosition.x, smoothPosition.y, -10f);

        transform.position = smoothPositionFinal;
    }
}
