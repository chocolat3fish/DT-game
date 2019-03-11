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
    public int playerLevel;
    public float playerExperience;
    public int playerSkillPoints;

    //PlayerStats Constructor
    public PlayerStats(int health, int playerLevel, float playerExperience, int playerSkillPoints)
    {
        this.health = health;
        this.playerLevel = playerLevel;
        this.playerExperience = playerExperience;
        this.playerSkillPoints = playerSkillPoints;
        
}
   

    
}

