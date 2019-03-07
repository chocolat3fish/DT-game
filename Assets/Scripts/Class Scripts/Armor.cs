using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class that can act as a variable in other scripts

//Serializable means it can be seen as a variable in the inspector
[Serializable]
public class Armour
{
    //variable within armour class
    public string armourType;
    public float defence;

    //Armour Construtor called with new Armour(type, defence)
    public Armour(string armorType, float defence)
    {
        this.armourType = armorType;
        this.defence = defence;
    }

}

