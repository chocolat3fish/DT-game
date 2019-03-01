using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeathBar : MonoBehaviour
{
    Vector3 localScale;
    public GameObject player;
    private PlayerControls playerControls;
    void Start()
    {
        //sets scale to default
        localScale = transform.localScale;
        playerControls = player.GetComponent<PlayerControls>();

    }

    void Update()
    {
        //as health depletes, scales the x value down relative to health
        localScale.x = playerControls.currentHealth/10;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
