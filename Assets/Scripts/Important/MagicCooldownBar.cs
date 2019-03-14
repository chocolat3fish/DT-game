using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicCooldownBar : MonoBehaviour
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
        //depletes bar on attack, refills relative to current attack speed
        float sizeOfBar = (Time.time - playerControls.timeOfMagic) / playerControls.magicCooldown;
        if (sizeOfBar > 1f)
        {
            sizeOfBar = 1;
        }
        transform.localScale = new Vector3(sizeOfBar, transform.localScale.y, transform.localScale.z);

    }
}

