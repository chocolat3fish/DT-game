using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnCollisionStay(Collision other)
    {

        Debug.Log(other);
    }
}
