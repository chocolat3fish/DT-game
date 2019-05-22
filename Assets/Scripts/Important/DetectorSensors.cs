using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorSensors : MonoBehaviour
{
    public PlayerControls player;

    public static List<string> energyBeats = new List<string> { "Fire", "Void", "Light" };
    public static List<string> fireBeats = new List<string> { "Acid", "Shadow", "Void" };
    public static List<string> acidBeats = new List<string> { "Light", "Water", "Energy" };
    public static List<string> waterBeats = new List<string> { "Fire", "Energy", "Shadow" };
    public static List<string> shadowBeats = new List<string> { "Acid", "Energy", "Void" };
    public static List<string> lightBeats = new List<string> { "Water", "Shadow", "Fire" };
    public static List<string> voidBeats = new List<string> { "Acid", "Water", "Light" };
    public static Dictionary<string, List<string>> weaponElements = new Dictionary<string, List<string>>
    {
        {"Energy", energyBeats},
        {"Fire", fireBeats},
        {"Acid", acidBeats},
        {"Water", waterBeats},
        {"Shadow", shadowBeats},
        {"Light", lightBeats},
        {"Void", voidBeats}
    };

    private void Awake()
    {
        player = transform.parent.GetComponent<PlayerControls>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            EnemyMonitor enemy = collision.gameObject.GetComponent<EnemyMonitor>();
            float elementDamage;

            if (PersistantGameManager.Instance.currentWeapon.itemElement == "")
            {
                elementDamage = 1;
            }
            else if (weaponElements[PersistantGameManager.Instance.currentWeapon.itemElement].Contains(enemy.enemyStats.enemyClass))
            {
                elementDamage = 1.5f;
            }
            else
            {
                elementDamage = 0.5f;
            }

            double newPlayerDamage = player.CalculatePlayerDamage(elementDamage);
            float playerHealthSteal = player.CalculatePlayerHealing();
            enemy.currentHealth -= newPlayerDamage;
            player.currentHealth += playerHealthSteal;
            player.attackTime = Time.time;
            if (player.currentHealth > player.totalHealth)
            {
                player.currentHealth = player.totalHealth;
            }
        }

    }
}
