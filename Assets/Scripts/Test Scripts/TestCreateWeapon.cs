using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCreateWeapon : MonoBehaviour
{
    Weapon weapon;
    private void Start()
    {
        for(int i=0; i<10; i++)
        {
            LootManager.GenerateWeapon(i+70, 1f, 1f, 1f);
        }
    }
    
}
