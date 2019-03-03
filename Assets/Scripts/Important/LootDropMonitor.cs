using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropMonitor : MonoBehaviour
{
    public GameObject player;
    private BoxCollider2D playerBoxCollider2D;

    public GameObject compareCanvas;
    public Weapon newItemStats;

    public bool waitingForChange = false;

    public float distanceToPlayer;


    private void Update()
    {
        distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
        if(distanceToPlayer < 2f && Input.GetKeyDown(KeyCode.E))
        {
            if(!PersistantGameManager.Instance.compareScreenOpen)
            {
                
                PersistantGameManager.Instance.comparingWeapon = newItemStats;
                PersistantGameManager.Instance.compareScreenOpen = true;
                waitingForChange = true;
                Debug.Log("Done");
            }
        }
        if(waitingForChange && !PersistantGameManager.Instance.compareScreenOpen)
        {
            newItemStats = PersistantGameManager.Instance.comparingWeapon;
            waitingForChange = false;
        }
    }


    

   
}
