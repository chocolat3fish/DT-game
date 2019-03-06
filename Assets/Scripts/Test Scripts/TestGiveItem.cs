﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGiveItem : MonoBehaviour
{


    public static void GiveItem()
    {
        PersistantGameManager.Instance.playerWeaponInventory[0] = new Weapon("Short Sword", 2f, 0.5f, 1);
        PersistantGameManager.Instance.playerWeaponInventory[1] = new Weapon("Knife", 1f, 0.25f, 0.5f);
        PersistantGameManager.Instance.playerWeaponInventory[2] = new Weapon("Spear", 4f, 0.75f, 2);
        PersistantGameManager.Instance.playerWeaponInventory[3] = new Weapon("Long Sword", 3f, 0.6f, 1.5f);
        PersistantGameManager.Instance.playerWeaponInventory[4] = new Weapon("Axe", 10f, 2f, 1.5f);
        PersistantGameManager.Instance.currentArmour = new Armour("Wooden", 0.5f);
        Debug.Log("Add sword");

    }


    void Update()
    {
        
    }
}