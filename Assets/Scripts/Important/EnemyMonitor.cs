using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMonitor : MonoBehaviour
{

    Rigidbody2D enemyRigidbody;

    private string enemyName;
    private float totalHealth;
    public float currentHealth;
    private float enemyDamage;
    public int itemChance;

    public Enemy enemyStats;

    void Start()
    {

        enemyRigidbody = GetComponent<Rigidbody2D>();

        totalHealth = enemyStats.enemyHealth;
        enemyName = enemyStats.enemyName;
        enemyDamage = enemyStats.enemyDamage;

        //Fixes issue of having to move before being able to attack it again. Likely better solution but will work for now.
        enemyRigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;
        currentHealth = totalHealth;

        enemyRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            EnemyDeath();
        }
    }

    public void EnemyDeath()
    {
        LootManager.DropItem(itemChance);
        Destroy(gameObject);

    }


}
