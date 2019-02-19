using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{   

    //speed of projectile, time until destruction
    float attackSpeed = 30f;
    float detectorTime = 0.05f;
    Rigidbody2D detectorRb;


    void Start()
    {
        detectorRb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {

        //adds velocity and destroys after set time
        detectorRb.velocity = new Vector2(attackSpeed, 0f);

        Destroy(gameObject, detectorTime);
    }


}
