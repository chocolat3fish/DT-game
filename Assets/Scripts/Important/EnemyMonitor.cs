using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//******MUST BE CHILDED TO AN ENEMY ENEMYSTATS MUST BE SET INSPECTOR******\\

//Script for controlling enemy and enemy attack
public class EnemyMonitor : MonoBehaviour
{
    //The Rigidbody of the enemy
    Rigidbody2D enemyRigidbody;

    //The current health of the enemy at the beginning gets set as the total health of the enemy
    public double currentHealth;

    //Damage the enemy does
    private float enemyDamage;

    //How much ex the enemy gives the player on death

    //Distance to player
    public float distanceToPlayer;

    public bool droppedQuestItem;

    //The player
    public GameObject player;
    private PlayerControls playerControls;
    private EnemyAttacks enemyAttacks;

    private NPCMonitor nPCMonitor;

    //All enemy stats (name, damage, health, speed , range)
    public Enemy enemyStats;

    //Stats for lootdrop
    public int itemChance;
    public int weaponValue;
    public int itemValue;

    public bool questTarget;
    public string questReward;

    //Name of the loot drop prefab
    private string lootDropPreFabName = "Loot Drop";


    private EnemyAttacks parentController;
    public bool waitingForCollision;

    private void Awake()
    {
        parentController = transform.parent.GetComponent<EnemyAttacks>();
    }
    void Start()
    {
        //Gets the components
        player = FindObjectOfType<PlayerControls>().gameObject;
        enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        playerControls = player.GetComponent<PlayerControls>();


        float multiplier = 1;

        switch (enemyStats.enemyClass)
        {
            case "Light":
                multiplier *= 0.8f;
                break;

            case "Medium":
                multiplier *= 1f;
                break;

            case "Heavy":
                multiplier *= 1.2f;
                break;
        }
        //sets the current health to the inital health and set health based on level
        enemyStats.enemyHealth = (float)(multiplier * (27 * Math.Pow(enemyStats.enemyLevel, 2) + 10));
        currentHealth = enemyStats.enemyHealth;

        //Sets the default damage
        if (PersistantGameManager.Instance.playerStats.playerLevel < 4)
        {
            enemyStats.enemyDamage = (float)(multiplier * (2.5 * Math.Pow(enemyStats.enemyLevel, 2) + 10) * 0.4f);
        }
        else
        {
            enemyStats.enemyDamage = (float)(multiplier * (2.5 * Math.Pow(enemyStats.enemyLevel, 2) + 10));
        }


        

        //Fixes issue of having to move before being able to attack it again. Likely better solution but will work for now.
        enemyRigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;

        //stops the enemy from rotating
        enemyRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        //Starts the Attack routinue
        //StartCoroutine(Attack());

    }

    void Update()
    {
        //kills the player if its health drops to or below 0
        if (currentHealth <= 0)
        {
            //runs the death method
            EnemyDeath();
        }
        /*
        //gets the distance to player
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        //if the enemy is in range start the attacking part of the the attack coroutine by settign attacking to true
        if(distanceToPlayer <= enemyStats.attackRange && !attacking)
        {
            attacking = true;
        }
        //if the enemy is still attaking and out of range turns off the attacking
        else if(distanceToPlayer > enemyStats.attackRange && attacking)
        {
            attacking = false;
        }
        */

    }

    //The Death method, drops a loot item, gives xp and then destorys the enemy will eventully play death animation
    public void EnemyDeath()
    {

        if (questTarget == true && PersistantGameManager.Instance.activeQuests.Contains(PersistantGameManager.Instance.questTargets[questReward]))
        {
            GameObject questDrop = Instantiate(Resources.Load(lootDropPreFabName), transform.position, Quaternion.identity) as GameObject;
            LootDropMonitor questDropMonitor = questDrop.GetComponent<LootDropMonitor>();



            questDropMonitor.type = 2;
            //questDropMonitor.item = PersistantGameManager.Instance.questTargets[nPCMonitor.currentQuest.questKey];
            questDropMonitor.item = questReward;
            droppedQuestItem = true;

            Debug.Log("Dropped item " + questReward);

        }
        //Gets the new weapon based off the drop chance and the value of weapon, if a weapon is not going to be droped it returns null
        LootItem newItem = LootManager.DropItem(itemChance, weaponValue);

        //runs if there is a weapon stored in new weapon
        if (newItem != null)
        {
            //Instatiates the loot drop prefab at the poition of the enemy takes a local "Gameobject copy" 
            GameObject lootDropInstance = Instantiate(Resources.Load(lootDropPreFabName), transform.position, Quaternion.identity) as GameObject;

            //gets the LootDropMonitor from the newly created Loot Drop
            LootDropMonitor lootDropInstanceMonitor = lootDropInstance.GetComponent<LootDropMonitor>();

            if (newItem.type == 0)
            {
                lootDropInstanceMonitor.type = 0;
                lootDropInstanceMonitor.itemStats = newItem.newWeapon;
            }
            /*
            else if(newItem.type == 1)
            {
                lootDropInstanceMonitor.type = 1;
                lootDropInstanceMonitor.consumable= newItem.consumable;
            }
            */

            if (droppedQuestItem == true)
            {
                Vector2 lootPosition = lootDropInstance.transform.position;
                lootDropInstance.transform.position = new Vector2(lootDropInstance.transform.position.x - 1 / 2, lootPosition.y);
            }


            //Tells the Loot Drop what weapon it should store


        }

        //Gives the player XP
        GiveExp(enemyStats.enemyClass, enemyStats.enemyLevel);

        //Kills the enemy
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Debug.Log("no parent");
            Destroy(transform.gameObject);
        }


    }
    //Obselete
    /*
    //The Attack Co-Routine Temporary will be revised eventully
    IEnumerator Attack()
    {
        //Keeps running infinitly
        while (true) 
        {
            //Stops the program from freezing as it loops for ever is the attacking variable is not true
            yield return new WaitForSeconds(.1f);
            
            //if the attacking variable is true and the compare canvas has not frozen everything
            if (attacking && Time.timeScale != 0)
            {
                //wait however long the enemy attack speed is 
                yield return new WaitForSeconds(enemyStats.attackSpeed);

                //check to make sure the player hasnt left the hit box
                if(attacking && Time.timeScale != 0)
                {
                    //calculates how much damage to apply to the character
                    float enemyAtackDamage = enemyStats.enemyDamage - playerControls.defence;

                    // if the players defence cancels out the enemys attack to much i.e. making it negative sets the damage to 0.1
                    if (enemyAtackDamage < 0.1)
                    {
                        enemyAtackDamage = 0.1f;
                    }

                    //Applys the Damage
                    playerControls.currentHealth -= enemyAtackDamage;
                }
                
            }
        }
    }
    */
    public void GiveExp(string enemyClass, int enemyLevel)
    {
        //Will add Calculation here when ready to balance
        //This is a temporary value
        //xpValue = enemyStats.enemyHealth * enemyStats.enemyDamage;
        //PersistantGameManager.Instance.playerStats.playerExperience += xpValue;
        //PersistantGameManager.Instance.checkExp = true;

        float xpValue;
        float multiplier = 1;

        switch (enemyClass)
        {
            case "Light":
                multiplier *= 0.025f;
                break;

            case "Medium":
                multiplier *= 0.05f;
                break;

            case "Heavy":
                multiplier *= 0.075f;
                break;
        }

        xpValue = (float)((0.04 * Math.Pow(enemyLevel, 3)) + (0.8 * Math.Pow(enemyLevel, 2)) + 500) * multiplier;
        PersistantGameManager.Instance.playerStats.playerExperience += xpValue;
        Debug.Log(xpValue);
        PersistantGameManager.Instance.checkExp = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            if (Time.timeScale != 0f)
            {
                //calculates how much damage to apply to the character
                float enemyAtackDamage = enemyStats.enemyDamage;

                // if the players defence cancels out the enemys attack to much i.e. making it negative sets the damage to 0.1
                if (enemyAtackDamage < 0.1)
                {
                    enemyAtackDamage = 0.1f;
                }

                //Applys the Damage
                playerControls.currentHealth -= enemyAtackDamage * PersistantGameManager.Instance.damageResistMulti;
            }

        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Floor" && waitingForCollision)
        {
            parentController.jumpChargeCollision = true;
            waitingForCollision = false;
            Debug.Log("freeze");
        }
    }
}
