using System;
using UnityEngine;

[Serializable]
public class Quest
{


    public string questName;

    [TextArea(5, 20)]
    public string[] sentencesBeforeQuest1stTime;
    [TextArea(5, 20)]
    public string[] sentencesBeforeQuest2ndTime;
    [TextArea(5, 20)]
    public string questStatment;
    [TextArea(5, 20)]
    public string questStatmentWithItem;
    [TextArea(5, 20)]
    public string[] sentencesAfterQuest;
    [TextArea(5, 20)]
    public string questDescription;
    [TextArea(5, 20)]
    public string[] sentencesAfterQuestEnd;


    [Header("Kill")]
    public bool killEnemies;
    public string enemyToKill;
    public int killRequirement;
    public int enemiesKilled;

    [Header("Fetch")]
    public bool returnItem;
    public string questItemName;

    public string questKey;


    [Header("Rewards")]
    public bool instantComplete;
    public string questReward;
    public string weaponType;
    public string itemName;
    public float XPMultiplier;
    public bool giveWeapon;
    public bool giveItem;
    public int weaponValue;
    public float questExperience;
    public int levelClaimedAt;


    /*
    public Quest(string questName, string[] sentencesBeforeQuest, string questStatment, string[] sentencesAfterQuest, string questItemName, string questDescription)
    {
        this.questName = questName;

        this.sentencesBeforeQuest1stTime = sentencesBeforeQuest;
        this.questStatment = questStatment;
        this.sentencesAfterQuest = sentencesAfterQuest;

        this.questItemName = questItemName;

        this.questDescription = questDescription;
    }*/
}
