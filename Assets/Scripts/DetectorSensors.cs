using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorSensors : MonoBehaviour
{
    public PlayerControls player;
    private void Awake()
    {
        player = transform.parent.GetComponent<PlayerControls>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            EnemyMonitor enemy = collision.gameObject.GetComponent<EnemyMonitor>();
            float newPlayerDamage = player.CalculatePlayerDamage();
            enemy.currentHealth -= newPlayerDamage;
        }

    }
}
