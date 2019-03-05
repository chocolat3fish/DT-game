using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Armor 
{
    public string armorType;
    public float defence;

    public Armor(string armorType, float defence)
    {
        this.armorType = armorType;
        this.defence = defence;
    }
   
}
