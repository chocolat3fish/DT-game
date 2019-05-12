using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//******MUST BE APPLYED TO THE LOOT DROP PREFAB******\\

//A script the monitors the loot drop item after it has been instantiated
public class LootDropMonitor : MonoBehaviour
{
    public bool IsForMap;
    //The Player
    public GameObject player;
    public AsyncTriggers asyncTriggers;
    public SpriteRenderer spriteRenderer;
    //The script that controls the compare canvas
    public Color consumableColor;
    public Color weaponColor;
    public Color itemColor;
    public Color defaultColor;
    [Header("Choose type, 0 is weapon, 1 is consumable, 2 is item")]
    public int type;
    //The new weapon the loot item is holding
    public Weapon itemStats;
    public Consumable consumable;
    public string item;

    //The Bool that tells the script to check weither the compare screen has closed and should now take the disregared item
    public bool waitingForChange = false;

    //Distance to player
    public float distanceToPlayer;



    private void Awake()
    {
        //Gets Components
        asyncTriggers = FindObjectOfType<AsyncTriggers>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(!IsForMap)
        {
            player = FindObjectOfType<PlayerControls>().gameObject;
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), player.GetComponent<BoxCollider2D>());
            EnemyMonitor[] enemies = FindObjectsOfType<EnemyMonitor>();
            foreach (EnemyMonitor m in enemies)
            {
                Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), m.GetComponent<BoxCollider2D>());
            }
        }



    }
    private void Start()
    {
        if(IsForMap)
        {
            type = transform.parent.GetComponent<LootDropMonitor>().type;
        }
        if (type == 0)
        {
            spriteRenderer.color = weaponColor;
        }
        else if(type == 1)
        {
            spriteRenderer.color = consumableColor;
        }
        else if (type == 2)
        {
            spriteRenderer.color = itemColor;
        }
        else
        {
            spriteRenderer.color = defaultColor;
        }
        if(IsForMap)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        //Gets the distance to the player
        distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        //If the player is close enough to the loot drop and "E" is pressed
        if(distanceToPlayer < 2f && Input.GetKeyDown(KeyCode.E) && Time.timeScale != 0)
        {
            //makes an array of all current loot drops
            LootDropMonitor[] lootDrops = FindObjectsOfType<LootDropMonitor>();

            //Sets the closest loot drop info as this
            LootDropMonitor closestLootDrop = this;
            float closestDistance = distanceToPlayer;

            //repeats for each loot drop
            foreach (LootDropMonitor lootDrop in lootDrops)
            {
                //if a loot drop is closer than any previous once sets that loot drop as the closest loot drop 
                if (lootDrop.distanceToPlayer < closestDistance)
                {
                    closestDistance = lootDrop.distanceToPlayer;
                    closestLootDrop = lootDrop;
                }
            }

            //If the compare Screen is not currently open, the compare canvas wont close on a "E" press and this is the closest loot drop
            if (!PersistantGameManager.Instance.compareScreenOpen && closestLootDrop == this && type == 0)
            {
                asyncTriggers.OpenCompareCanvas(itemStats);

                //Tells the script that is needs to wait for the comapre screen to close
                waitingForChange = true;
            }
            else if (!PersistantGameManager.Instance.compareScreenOpen && closestLootDrop == this && type == 1)
            {
                //Sets the weapon to compare as the weapon this is storing
                PersistantGameManager.Instance.amountOfConsumables[consumable.type] += 1;
                Destroy(gameObject);
            }
            else if(!PersistantGameManager.Instance.compareScreenOpen && closestLootDrop == this && type == 2)
            {
                PersistantGameManager.Instance.itemInventory[item] += 1;
                Destroy(gameObject);
            }

        }

        //Calls when the compare screen closes
        if(waitingForChange && !PersistantGameManager.Instance.compareScreenOpen)
        {
            //stores the disregarded weapon 
            itemStats = PersistantGameManager.Instance.comparingWeapon;

            //tells the script it doesnt need to wait for the compare screen to close
            waitingForChange = false;
        }
    }


    

   
}
