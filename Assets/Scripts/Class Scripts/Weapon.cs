using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Weapon
{



    public string itemName;
    public float itemDamage;
    public float itemSpeed;
    public float itemRange;

    public Weapon(string itemName, float itemDamage, float itemSpeed, float itemRange)
    {
        this.itemName = itemName;
        this.itemDamage = itemDamage;
        this.itemSpeed = itemSpeed;
        this.itemRange = itemRange;
    }

}
