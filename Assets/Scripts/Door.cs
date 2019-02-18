using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public BoxCollider2D boxCollider2D;
    private void OnTriggerEnter()
    {
        if (collision.collider == boxCollider2D)
        {
            Debug.Log("yes");

        }
        Debug.Log(collision.collider);
    }
}
