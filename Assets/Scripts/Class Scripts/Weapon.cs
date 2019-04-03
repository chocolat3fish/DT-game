using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A class that stores all the information for a weapon

//Serializable means it can be seen as a variable in the inspector
[Serializable]
public class Weapon
{ 

    public string itemName;
    public float itemDamage;
    public float itemSpeed;
    public float itemRange;
    public float itemLevel;

    //Weapon Constructor
    public Weapon(string itemName, float itemDamage, float itemSpeed, float itemRange, float itemLevel)
    {
        this.itemName = itemName;
        this.itemDamage = itemDamage;
        this.itemSpeed = itemSpeed;
        this.itemRange = itemRange;
        this.itemLevel = itemLevel;
    }

}
