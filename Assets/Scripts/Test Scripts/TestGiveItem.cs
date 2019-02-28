using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGiveItem : MonoBehaviour
{


    public static void GiveItem()
    {
        PersistantGameManager.Instance.playerIventory[PersistantGameManager.Instance.currentIndex] = new Weapon("Sword", 2f, 0.5f);
        PersistantGameManager.Instance.playerIventory[1] = new Weapon("Knife", 1f, 0.25f);
        Debug.Log("Add sword");
    }


    void Update()
    {
        
    }
}