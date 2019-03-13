using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerData
{

    public int currentIndex;
    public Weapon currentWeapon;
    public List<Weapon> playerWeaponInventory = new List<Weapon>();
    public Armour currentArmour;
    public List<Armour> playerArmourInventory = new List<Armour>();

    public PlayerStats playerStats;

    public float totalExperience;

    public string currentScene;


}
