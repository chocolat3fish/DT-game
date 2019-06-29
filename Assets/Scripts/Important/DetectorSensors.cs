using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorSensors : MonoBehaviour
{
    public PlayerControls player;

    //Stores what beats what in the element paper-scissors-rock format
    public static List<string> bloodBeats = new List<string> { "Venom", "Shadow" };
    public static List<string> venomBeats = new List<string> { "Shadow", "Void" };
    public static List<string> waterBeats = new List<string> { "Blood", "Venom" };
    public static List<string> shadowBeats = new List<string> { "Water", "Void" };
    public static List<string> voidBeats = new List<string> { "Blood", "Water" };

    public static Dictionary<string, List<string>> weaponElements = new Dictionary<string, List<string>>
    {
        {"Blood", bloodBeats},
        {"Venom", venomBeats},
        {"Water", waterBeats},
        {"Shadow", shadowBeats},
        {"Void", voidBeats}
    };

    private void Awake()
    {
        player = transform.parent.GetComponent<PlayerControls>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //On collision, easiest script/method to find the enemy element and calculate the bonuses and feed it into the damage function.
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyMonitor enemy = collision.gameObject.GetComponent<EnemyMonitor>();
            float elementDamage;

            if (PersistantGameManager.Instance.currentWeapon.itemElement == "" || PersistantGameManager.Instance.currentWeapon.itemElement == enemy.enemyStats.enemyClass)
            {
                elementDamage = 1;
            }
            else if (PersistantGameManager.Instance.currentWeapon.itemElement == "Light")
            {
                elementDamage = 1.2f;
            }
            else if (weaponElements[PersistantGameManager.Instance.currentWeapon.itemElement].Contains(enemy.enemyStats.enemyClass))
            {
                elementDamage = 1.5f;
            }
            else
            {
                elementDamage = 0.5f;
            }

            //Calculates damage then calculates the healing the player does
            double newPlayerDamage = player.CalculatePlayerDamage(elementDamage);
            float playerHealthSteal = player.CalculatePlayerHealing();

            //Deals damage and adds health
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
