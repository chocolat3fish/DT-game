using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeathBar : MonoBehaviour
{
    Vector3 localScale;
    private float currentHealth = 5f * 1000f;
    void Start()
    {
        //sets scale to default
        localScale = transform.localScale;

    }

    /*void Update()
    {
        //as health depletes, scales the x value down relative to health
        localScale.x = currentHealth;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }*/
}
