﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownBarMonitor : MonoBehaviour
{
    Vector3 localScale;
    public PlayerControls playerControls;

    void Start()
    {
        //sets scale to default
        localScale = transform.localScale;
        playerControls = FindObjectOfType<PlayerControls>();
    }

    void Update()
    {
        //as health depletes, scales the x value down relative to health
        //uses a Lerp to smooth the depletion
        float sizeOfBar = (Time.time - playerControls.timeOfAttack) / playerControls.attackSpeed;
        if(sizeOfBar > 1f)
        {
            sizeOfBar = 1;
        }
        transform.localScale = new Vector3(sizeOfBar, transform.localScale.y, transform.localScale.z);

    }
}
