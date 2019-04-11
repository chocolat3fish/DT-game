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
            double newPlayerDamage = player.CalculatePlayerDamage();
            float playerHealthSteal = player.CalculatePlayerHealing();
            enemy.currentHealth -= newPlayerDamage;
            player.currentHealth += playerHealthSteal;
            if (player.currentHealth > player.totalHealth)
            {
                player.currentHealth = player.totalHealth;
            }
        }

    }
}
