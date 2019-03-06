 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerStats
{
    /*
    public int health { get; set; }
    public int xp { get; set; }
    public int gold { get; set; }

    
    public static PlayerStats PlayerStatsConstrutor(int health, int xp, int gold)
    {
        PlayerStats playerStatsConstructor = new PlayerStats
        {
            health = health,
            xp = xp,
            gold = gold
        };

        return playerStatsConstructor;
    }
    */

    public int playerLevel;
    public float playerExperience;
}

