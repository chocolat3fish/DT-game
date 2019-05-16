using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TestGiveItem : MonoBehaviour
{


    public static void GiveItem()
    {
        /*
        PersistantGameManager.Instance.playerWeaponInventory[0] = new Weapon("Short Sword", 2f, 0.5f, 1, 1);
        PersistantGameManager.Instance.playerWeaponInventory[1] = new Weapon("Dagger", 1f, 0.25f, 0.5f, 1);
        PersistantGameManager.Instance.playerWeaponInventory[2] = new Weapon("Lance", 4f, 0.75f, 2, 1);
        */

        //PersistantGameManager.Instance.playerWeaponInventory[0] = LootManager.GenerateWeapon(10);
        //PersistantGameManager.Instance.playerWeaponInventory[1] = LootManager.GenerateWeapon(10);
        //PersistantGameManager.Instance.playerWeaponInventory[2] = LootManager.GenerateWeapon(10);

        //shortened to 3 items
        //PersistantGameManager.Instance.playerWeaponInventory[3] = new Weapon("Long Sword", 3f, 0.6f, 1.5f);
        //PersistantGameManager.Instance.playerWeaponInventory[4] = new Weapon("Axe", 10f, 2f, 1.5f);
        //PersistantGameManager.Instance.currentArmour = new Armour("Wooden", 0.5f);
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            PersistantGameManager.Instance.playerWeaponInventory[0] = new Weapon("Nothing", 0f, 0.00f, 0.00f, 0, 0);
            PersistantGameManager.Instance.playerWeaponInventory[1] = new Weapon("More Nothing", 0f, 0.00f, 0.00f, 0, 0);
            PersistantGameManager.Instance.playerWeaponInventory[2] = new Weapon("And More Nothing", 0f, 0.00f, 0.00f, 0, 0);
        }
        else
        {
            PersistantGameManager.Instance.playerWeaponInventory[0] = LootManager.GenerateWeapon(10);
            PersistantGameManager.Instance.playerWeaponInventory[1] = LootManager.GenerateWeapon(10);
            PersistantGameManager.Instance.playerWeaponInventory[2] = LootManager.GenerateWeapon(10);
        }


    }


    void Update()
    {

    }
}
