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
    public int weaponValue;
    public float attackRange;


    public string lootDropPreFabName;
    
    

    private float distanceToPlayer;
    public GameObject player;
    private PlayerControls playerControls;
    public Enemy enemyStats;
    private bool attacking = false;

    public LootItem lootItem;

    void Start()
    {

        enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        playerControls = player.GetComponent<PlayerControls>();
        

        totalHealth = enemyStats.enemyHealth;
        enemyName = enemyStats.enemyName;
        enemyDamage = enemyStats.enemyDamage;

        currentHealth = totalHealth;
        //Fixes issue of having to move before being able to attack it again. Likely better solution but will work for now.
        enemyRigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;
        

        enemyRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(Attack());

    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            EnemyDeath();
        }

        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if(distanceToPlayer <= attackRange && !attacking)
        {
            attacking = true;
        }

        else if(distanceToPlayer > attackRange && attacking)
        {
            attacking = false;
        }

    }

    public void EnemyDeath()
    {
        //not done, will apply new values to the loot object
        Weapon newWeapon = LootManager.DropItem(itemChance, weaponValue);
        if(newWeapon != null)
        {
            GameObject lootDropInstance = Instantiate(Resources.Load(lootDropPreFabName), transform.position, Quaternion.identity) as GameObject;
            LootDropMonitor lootDropInstanceMonitor = lootDropInstance.GetComponent<LootDropMonitor>();
            lootDropInstanceMonitor.player = player;
            lootDropInstanceMonitor.newItemStats = newWeapon;
        }
        
        Destroy(gameObject);

    }
    IEnumerator Attack()
    {
        while (true) 
        {
            yield return new WaitForSeconds(.5f);
            if (attacking && Time.timeScale != 0)
            {
                yield return new WaitForSeconds(enemyStats.attackSpeed);
                float enemyAtackDamage = enemyStats.enemyDamage - playerControls.defence;
                if(enemyAtackDamage < 0.1)
                {

                    enemyAtackDamage = 0.1f;
                }
                playerControls.currentHealth -= enemyAtackDamage;
            }
        }



    
    }




}
