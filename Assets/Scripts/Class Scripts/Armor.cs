using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Armour 
{
    public string armourType;
    public float defence;

    public Armour(string armorType, float defence)
    {
        this.armourType = armorType;
        this.defence = defence;
    }
   
}
