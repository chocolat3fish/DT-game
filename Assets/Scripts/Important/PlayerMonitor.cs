using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonitor : MonoBehaviour
{
    private static PlayerControls playerControls;
    private static Weapon currentWeapon;

    void Start()
    {
        playerControls = GetComponent<PlayerControls>();
    }

    public static void UpdateWeapon() 
    {
        currentWeapon = PersistantGameManager.Instance.currentWeapon;
        playerControls.playerDamage = currentWeapon.itemDamage;
        playerControls.attackSpeed = currentWeapon.itemSpeed;
    }
}
