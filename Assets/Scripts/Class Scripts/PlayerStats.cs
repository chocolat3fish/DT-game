 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerStats
{
    
    public int health { get; set; }
    public int xp { get; set; }
    public int gold { get; set; }
    public int playerLevel;
    public float playerExperience;

    public PlayerStats(int health, int xp, int gold, int playerLevel, float playerExperience)
    {


        this.health = health;
        this.xp = xp;
        this.gold = gold;
        this.playerLevel = playerLevel;
        this.playerExperience = playerExperience;
        
}
   

    
}

