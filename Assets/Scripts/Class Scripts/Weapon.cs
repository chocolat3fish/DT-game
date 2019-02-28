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
    public int inventoryIndex;

    public Weapon(string itemName, float itemDamage, float itemSpeed)
    {
        this.itemName = itemName;
        this.itemDamage = itemDamage;
        this.itemSpeed = itemSpeed;
    }

}
