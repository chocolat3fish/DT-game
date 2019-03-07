 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//A class that stores all the information for a player can be acessed as a variable in another script

//Serializable means it can be seen as a variable in the inspector
[Serializable]
public class PlayerStats
{

    public int health;
    public int xp;
    public int gold;
    public int playerLevel;
    public float playerExperience;

    //PlayerStats Constructor
    public PlayerStats(int health, int xp, int gold, int playerLevel, float playerExperience)
    {


        this.health = health;
        this.xp = xp;
        this.gold = gold;
        this.playerLevel = playerLevel;
        this.playerExperience = playerExperience;
        
}
   

    
}

