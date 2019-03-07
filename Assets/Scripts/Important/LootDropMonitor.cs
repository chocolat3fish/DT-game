using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//******MUST BE APPLYED TO THE LOOT DROP PREFAB******\\

//A script the monitors the loot drop item after it has been instantiated
public class LootDropMonitor : MonoBehaviour
{
    //The Player
    public GameObject player;

    //The script that controls the compare canvas
    public CompareCanvasScript compareCanvas;

    //The new weapon the loot item is holding
    public Weapon itemStats;

    //The Bool that tells the script to check weither the compare screen has closed and should now take the disregared item
    public bool waitingForChange = false;

    //Distance to player
    public float distanceToPlayer;



    private void Awake()
    {
        //Gets Components
        player = FindObjectOfType<PlayerControls>().gameObject;
        compareCanvas = FindObjectOfType<CompareCanvasScript>();
    }

    private void Update()
    {
        //Gets the distance to the player
        distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        //If the player is close enough to the loot drop and "E" is pressed
        if(distanceToPlayer < 2f && Input.GetKeyDown(KeyCode.E))
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
            if (!PersistantGameManager.Instance.compareScreenOpen && compareCanvas.takeEInputForContinue && closestLootDrop == this)
            {
                //Sets the weapon to compare as the weapon this is storing
                PersistantGameManager.Instance.comparingWeapon = itemStats;

                //Opens the compare screen
                PersistantGameManager.Instance.compareScreenOpen = true;

                //Tells the script that is needs to wait for the comapre screen to close
                waitingForChange = true;
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
