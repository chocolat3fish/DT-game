using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameManagerData
{
    public string currentScene;
    public string previousScene;

    public Dictionary<string, int> itemInventory;
    public List<string> possibleItems;
    public Quest currentDialogueQuest;
    public Dictionary<string, int> characterQuests;
    public Dictionary<string, Quest> possibleQuests;
    public List<string> activeQuests;
    public Dictionary<string, string> rewards;
    public Dictionary<string, string> questTargets;

    public double attackSpeedMulti;
    public float attackRangeMulti;
    public float currentAttackMultiplier;
    public float smiteDamageMulti;
    public float smiteDurationMulti;
    public float lifeStealMulti;
    public float totalHealthMulti;
    public float damageResistMulti;
    public float turtleResistMulti;
    public float turtleMultiMulti;
    public float turtleDurationMulti;
    public float movementResistMulti;
    public float moveSpeedMulti;
    public float jumpHeightMulti;
    public float airAttackMulti;
    public float instantKillChance;
    public float betterLootChance;

    public bool hasMagic;
    public float damageResistDuration;
    public float smiteDuration;


    //tracks skill progression of each tree
    public Dictionary<string, int> skillLevels;

    public bool tutorialComplete;
    public int lastEnemyLevel;

    public float totalExperience;

    public int currentIndex;
    public Weapon currentWeapon;
    public List<Weapon> playerWeaponInventory;
    public PlayerStats playerStats;

    public int damageProgress;
    public int tankProgress;
    public int mobilityProgress;
    public int attackSpeedUpgrades;
    public bool potionIsActive;
    public string activePotionType;

    public float currentLeechMultiplier = 0;
    public float potionCoolDownTime;

    public Armour currentArmour;
    public Armour comparingArmour;

    public bool tripleJump;
    public bool hasSmite;
    public bool gripWalls;
    public bool maxedSpeed;
    public bool damageResist;

    public string equippedItemOne, equippedItemTwo;

    private int currentItemOneIndex, currentItemTwoIndex;
    private bool changeItemOne, changeItemTwo;


    public Dictionary<string, int> amountOfConsumables;


}
