using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGiveItem : MonoBehaviour
{


    void Start()
    {
        PersistantGameManager.Instance.currentWeapon = new Weapon("Sword", 2f, 0.5f);
        PlayerMonitor.UpdateWeapon();
        Debug.Log("Add sword");
    }


    void Update()
    {
        
    }
}