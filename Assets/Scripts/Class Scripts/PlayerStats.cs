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

    
    public static PlayerStats PlayerStatsConstrutor(int health, int xp, int gold)
    {
        PlayerStats playerStatsConstructor = new PlayerStats();
        playerStatsConstructor.health = health;
        playerStatsConstructor.xp = xp;
        playerStatsConstructor.gold = gold;

        return playerStatsConstructor;
    }

}

