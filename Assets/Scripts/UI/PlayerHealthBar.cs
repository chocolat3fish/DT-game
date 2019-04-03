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
        //uses a Lerp to smooth the depletion
        localScale.x = playerControls.currentHealth / playerControls.totalHealth;
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(localScale.x, transform.localScale.y, transform.localScale.z), 0.1f);
        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
        }
    }
}