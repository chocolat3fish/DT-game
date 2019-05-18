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
    public string itemPrefix;
    public float itemDamage;
    public float stockItemSpeed, trueItemSpeed;
    public float itemRange;
    public float itemLevel;

    //Weapon Constructor
    public Weapon(string itemName, string itemPrefix, float itemDamage, float stockItemSpeed, float trueItemSpeed, float itemRange, float itemLevel)
    {
        this.itemName = itemName;
        this.itemPrefix = itemPrefix;
        this.itemDamage = itemDamage;
        this.stockItemSpeed = stockItemSpeed;
        this.trueItemSpeed = trueItemSpeed;
        this.itemRange = itemRange;
        this.itemLevel = itemLevel;
    }

}
