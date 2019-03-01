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
    private float distanceToPlayer;
    public GameObject player;
    public PlayerControls playerControls;
    public Enemy enemyStats;
    private bool attacking = false;

    public LootItem lootItem;

    void Start()
    {

        enemyRigidbody = GetComponent<Rigidbody2D>();
        playerControls = player.GetComponent<PlayerControls>();

        totalHealth = enemyStats.enemyHealth;
        enemyName = enemyStats.enemyName;
        enemyDamage = enemyStats.enemyDamage;

        //Fixes issue of having to move before being able to attack it again. Likely better solution but will work for now.
        enemyRigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;
        currentHealth = totalHealth;

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
        if(distanceToPlayer <= 2f && !attacking)
        {
            attacking = true;
        }
        else if(distanceToPlayer > 2f && attacking)
        {
            attacking = false;
        }

    }

    public void EnemyDeath()
    {
        //not done, will apply new values to the loot object
        /*Weapon newWeapon = LootManager.DropItem(itemChance);
        lootItem.newItemName = newWeapon.itemName;
        lootItem.newItemDamage = newWeapon.itemDamage;
        lootItem.newItemSpeed = newWeapon.itemSpeed;*/
        Destroy(gameObject);

    }
    IEnumerator Attack()
    {
        while (true) 
        {   
            yield return new WaitForSeconds(enemyStats.attackSpeed);
            if (attacking)
            {
                playerControls.currentHealth -= enemyStats.enemyDamage;
            }
        }



    
    }




}
