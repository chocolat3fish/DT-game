using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour { 

    Vector3 localScale;
    public PlayerControls playerControls;

    void Start()
    {
        //sets scale to default
        localScale = transform.localScale;
    }

    void Update()
    {
        //as health depletes, scales the x value down relative to health
        localScale.x = playerControls.currentHealth;
        transform.localScale = localScale;
    }
}