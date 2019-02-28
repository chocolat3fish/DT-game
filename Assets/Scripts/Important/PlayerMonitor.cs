using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonitor : MonoBehaviour
{
    private static PlayerControls playerControls;
    public static Weapon currentWeapon;

    void Start()
    {
        playerControls = GetComponent<PlayerControls>();
    }
    
    public static void UpdateWeapon() 
    {
        Debug.Log("updating weapon");
        currentWeapon = PersistantGameManager.Instance.currentWeapon;

    }
}
