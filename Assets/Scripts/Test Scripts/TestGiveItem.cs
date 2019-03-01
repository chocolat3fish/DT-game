using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGiveItem : MonoBehaviour
{


    public static void GiveItem()
    {
        PersistantGameManager.Instance.playerIventory[PersistantGameManager.Instance.currentIndex] = new Weapon("Short Sword", 2f, 0.5f);
        PersistantGameManager.Instance.playerIventory[1] = new Weapon("Knife", 1f, 0.25f);
        PersistantGameManager.Instance.playerIventory[2] = new Weapon("Spear", 4f, 0.75f);
        PersistantGameManager.Instance.playerIventory[3] = new Weapon("Long Sword", 3f, 0.6f);
        PersistantGameManager.Instance.playerIventory[4] = new Weapon("Axe", 10f, 2f);
        Debug.Log("Add sword");

    }


    void Update()
    {
        
    }
}