using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A class that can be used as a variable to store all enemy information

//Serializable means it can be seen as a variable in the inspector
[Serializable]
public class Enemy
{
    public string enemyName;
    public float enemyDamage;
    public double enemyHealth;
    public float attackSpeed;
    public float attackRange;
}
