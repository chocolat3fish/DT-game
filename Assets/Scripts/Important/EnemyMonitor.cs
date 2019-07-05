using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public string questKey;

    [Header("Animation")]
    public string attack, idle, walk, charge, stopCharge;
    private bool wasCharging;
    private bool attacking;
    private bool stopping;
    private bool idling;
    private bool charging;
    private bool walking;

    //Name of the loot drop prefab
    private string lootDropPreFabName = "Loot Drop";

    private static System.Random random = new System.Random();


    private EnemyAttacks parentController;
    public bool waitingForCollision;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        parentController = transform.parent.GetComponent<EnemyAttacks>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        //Gets the components
        player = FindObjectOfType<PlayerControls>().gameObject;
        enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        playerControls = player.GetComponent<PlayerControls>();

        if (PersistantGameManager.Instance.playerStats.playerLevel < enemyStats.enemyMinLevel)
        {
            enemyStats.enemyLevel = enemyStats.enemyMinLevel;
        }
        else if (PersistantGameManager.Instance.playerStats.playerLevel > enemyStats.enemyMaxLevel)
        {
            enemyStats.enemyLevel = enemyStats.enemyMaxLevel;
        }
        else //if player level in between min and max
        {
            int dif = random.Next(0, 1);
            enemyStats.enemyLevel = PersistantGameManager.Instance.playerStats.playerLevel + dif;
        }


        float multiplier = 1;

        switch (enemyStats.enemyTier)
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
        enemyStats.enemyHealth = (float)(multiplier * (27 * Math.Pow(enemyStats.enemyLevel, 2) + 10)) * 2;
        currentHealth = enemyStats.enemyHealth;

        //Sets the default damage

        if (PersistantGameManager.Instance.playerStats.playerLevel < 0)
        {
            enemyStats.enemyDamage = (float)(multiplier * (2.5 * Math.Pow(enemyStats.enemyLevel, 2) + 10) * 0.4f);
            //sets the current health to the inital health and set health based on level, but less if low level
            enemyStats.enemyHealth = (float)(multiplier * (27 * Math.Pow(enemyStats.enemyLevel, 2) + 10));
            currentHealth = enemyStats.enemyHealth;
        }
        else
        {
            enemyStats.enemyDamage = (float)(multiplier * (6 * Math.Pow(enemyStats.enemyLevel, 2) + 10));
            //sets the current health to the inital health and set health based on level
            enemyStats.enemyHealth = (float)(multiplier * (27 * Math.Pow(enemyStats.enemyLevel, 2) + 10)) * 2;
            currentHealth = enemyStats.enemyHealth;
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

        if (parentController.patrol == true && parentController.currentPointIndex == 0)
        {
            spriteRenderer.flipX = true;
        }
        if (parentController.patrol == true && parentController.currentPointIndex == 1)
        {
            spriteRenderer.flipX = false;
        }

        //Animation statements determining what animations to play depending on enemy class/action

        if (parentController.patrol && !attacking)
        {
            idling = false;
            walking = true;
            attacking = false;
            charging = false;
            stopping = false;
        }

        if (parentController.chargingLeft || parentController.chargingRight)
        {
            if (parentController.chargingLeft)
            {
                spriteRenderer.flipX = true;
            }
            if (parentController.chargingRight)
            {
                spriteRenderer.flipX = false;
            }
            idling = false;
            walking = false;
            attacking = false;
            charging = true;
            stopping = false;
            wasCharging = true;
        }
        if (!parentController.patrol && (!parentController.chargingLeft || !parentController.chargingRight))
        {
            idling = true;
            walking = false;
            attacking = false;
            charging = false;
            stopping = false;
        }

        if (!parentController.patrol && (parentController.chargingLeft || parentController.chargingRight))
        {
            idling = false;
            walking = false;
            attacking = false;
            charging = true;
            stopping = false;
        }

        if ((parentController.charge && transform.position.x <= parentController.patrolPoints[0].position.x 
            || parentController.charge && transform.position.x >= parentController.patrolPoints[1].position.x)
            && wasCharging)
        {
            idling = false;
            walking = false;
            attacking = false;
            charging = false;
            stopping = true;
            wasCharging = false;
        }

        if (idling)
        {
            animator.Play(idle);
        }
        if (attacking)
        {
            animator.Play(attack);
        }
        if (stopping)
        {
            animator.Play(stopCharge);
        }
        if (charging)
        {
            animator.Play(charge);
        }
        if (walking)
        {
            animator.Play(walk);
        }




        //kills the player if its health drops to or below 0
        if (currentHealth <= 0)
        {
            //runs the death method
            EnemyDeath();
        }
       

    }

    //The Death method, drops a loot item, gives xp and then destorys the enemy will eventully play death animation
    public void EnemyDeath()
    {
        if(PersistantGameManager.Instance.currentEnemyKills.ContainsKey(enemyStats.enemyName))
        {
            PersistantGameManager.Instance.currentEnemyKills[enemyStats.enemyName]++;
        }
        else
        {
            PersistantGameManager.Instance.currentEnemyKills.Add(enemyStats.enemyName, 1);
        }

        if (questTarget == true && PersistantGameManager.Instance.activeQuests.Contains(questKey))
        {
            GameObject questDrop = Instantiate(Resources.Load(lootDropPreFabName), transform.position + new Vector3(0.6f,0,0), Quaternion.identity) as GameObject;
            LootDropMonitor questDropMonitor = questDrop.GetComponent<LootDropMonitor>();



            questDropMonitor.type = 2;
            //questDropMonitor.item = PersistantGameManager.Instance.questTargets[nPCMonitor.currentQuest.questKey];
            questDropMonitor.item = questReward;
            droppedQuestItem = true;



        }

        // Sets most recent killed enemy's level to the enemy level for loot to level based on enemy stats rather than the player.
        PersistantGameManager.Instance.lastEnemyLevel = enemyStats.enemyLevel;

        
        //Gets the new weapon based off the drop chance and the value of weapon, if a weapon is not going to be droped it returns null
        LootItem newItem = LootManager.DropItem(itemChance, weaponValue);

        int chance = random.Next(0, 3);
        if (chance > 0 && enemyStats.enemyName != "Evil Door")
        {
            newItem = null;
        }

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
            

            if (droppedQuestItem == true)
            {
                Vector2 lootPosition = lootDropInstance.transform.position;
                lootDropInstance.transform.position = new Vector2(lootDropInstance.transform.position.x - 1 / 2, lootPosition.y);
            }


            //Tells the Loot Drop what weapon it should store


        }

        //Gives the player XP
        GiveExp(enemyStats.enemyTier, enemyStats.enemyLevel);


        //Kills the enemy
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {

            Destroy(transform.gameObject);
        }


    }
   
    public void GiveExp(string enemyClass, int enemyLevel)
    {
        //Will add Calculation here when ready to balance
        //This is a temporary value
        //xpValue = enemyStats.enemyHealth * enemyStats.enemyDamage;
        //PersistantGameManager.Instance.playerStats.playerExperience += xpValue;
        //PersistantGameManager.Instance.checkExp = true;

        float xpValue;
        float multiplier = 1f;

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
            default:
                multiplier *= 0.05f;
                break;
        }

        xpValue = (float)((0.3 * Math.Pow(enemyLevel, 3)) + (0.8 * Math.Pow(enemyLevel, 2)) + 500) * multiplier;
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            xpValue = 110;
        }
        PersistantGameManager.Instance.playerStats.playerExperience += xpValue;
        print(multiplier);

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
                animator.Play(attack.ToString());

                //Applys the Damage
                playerControls.currentHealth -= enemyAtackDamage * PersistantGameManager.Instance.damageResistMulti;
                playerControls.attackTime = Time.time;
            }


        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Floor" && waitingForCollision)
        {
            parentController.jumpChargeCollision = true;
            waitingForCollision = false;
        }
    }
}
