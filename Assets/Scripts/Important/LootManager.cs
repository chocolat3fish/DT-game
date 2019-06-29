using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//******MUST BE CHILDED TO THE LOOT DROP PREFAB******\\

//A script that can be referneced by other scrits and controlls loot drops
public class LootManager : MonoBehaviour
{

    public static float rangeBonus, speedBonus, damageBonus;
    //uses random from system rather than unityengine, allows for random.Next() 
    //which genreates a random number between ranges
    private static System.Random random = new System.Random();


    //Dictionaries with different classes of prefixes
    public static Dictionary<string, float> damagePrefixes = new Dictionary<string, float>

    {
        {"Keen", 1.1f},
        {"Sharp", 1.3f},
        {"Fierce", 1.5f},
        {"Cruel", 1.7f},
        {"Ruthless", 1.9f}
    };

    public static Dictionary<string, float> speedPrefixes = new Dictionary<string, float>
    {
        {"Quick", 1.1f},
        {"Swift", 1.2f},
        {"Nimble", 1.3f},
        {"Deft", 1.4f},
        {"Fleet", 1.5f}
    };

    public static Dictionary<string, float> rangePrefixes = new Dictionary<string, float>
    {
        {"Longer", 1.1f},
        {"Tall", 1.3f},
        {"Great", 1.5f},
        {"Full", 1.7f},
        {"Fat", 1.9f}
    };

    public static Dictionary<string, string> weaponElements = new Dictionary<string, string>
    {
        {"Blood", " of Sanguis"},
        {"Venom", " of Virulence"},
        {"Water", " of Drowning"},
        {"Shadow", " of Nightmares"},
        {"Light", " of Cleansing"},
        {"Void", " of The Abyss"}
    };

    //The main referenced method that controls wether a weapon is dropped
    //Based off a chance out of 100 and the value of the weapon
    public static LootItem DropItem(int chance, int weaponValue)
    {
        /*
        //Generates a random number between 1 and 100 
        int randomChance = random.Next(0, 100);

        //Checks if the chance is bigger that the generated number
        //This gives a n% where n is chance
        if (chance > randomChance)
        {

        */
        LootItem lootItem = new LootItem();
        lootItem.type = 0;
        lootItem.newWeapon = GenerateWeapon(weaponValue);
        return lootItem;
        /*


            //calls the method that generates the weapon based off the weapon value

        }
        else
        {
            LootItem lootItem = new LootItem();
            lootItem.type = 1;
            lootItem.consumable = GenerateConsumable(itemValue);
            return lootItem;
        }
        */
    }

    //A method called by the Drop item script to find a weapon based off a weapon value
    public static Weapon GenerateWeapon(int weaponValue)
    {
        //A random number
        int randomNumber;

        string weaponType = "";

        //picks weapon type at random.
        randomNumber = random.Next(0, 5);
        switch (randomNumber)
        {
            case 0:
                weaponType = "Dagger";
                rangeBonus = 1f;
                speedBonus = 0.4f;
                damageBonus = 0.6f;
                break;

            case 1:
                weaponType = "Short Sword";
                rangeBonus = 1.5f;
                speedBonus = 0.6f;
                damageBonus = 0.8f;
                break;

            case 2:
                weaponType = "Long Sword";
                rangeBonus = 2f;
                speedBonus = 1f;
                damageBonus = 1f;
                break;

            case 3:
                weaponType = "Lance";
                rangeBonus = 3f;
                speedBonus = 1f;
                damageBonus = 0.8f;
                break;

            case 4:
                weaponType = "Axe";
                rangeBonus = 4f;
                speedBonus = 1.5f;
                damageBonus = 1.5f;
                break;
        }


        //sets weapon level to the level of the enemy you killed
        int enemyLevel = PersistantGameManager.Instance.lastEnemyLevel;

        int newLevel = random.Next(enemyLevel - 1, enemyLevel + 1);
        if (newLevel <= 0) { newLevel = 1; }

        //for the better loot skill
        int randomChance = random.Next(1, 100);
        if (randomChance <= PersistantGameManager.Instance.betterLootChance)
        {
            weaponValue += 20;
        }

        //Decides dictionary to choose prefixes from
        string prefixType = "";
        int randomImprove = random.Next(1, 4);
        switch (randomImprove)
        {
            case 1:
                prefixType = "Damage";
                break;

            case 2:
                prefixType = "Speed";
                break;

            case 3:
                prefixType = "Range";
                break;
        }

        //chooses a prefix
        List<string> dictKeys = new List<string>(); 
        switch (prefixType)
        {
            case "Damage":
                dictKeys = new List<string>(damagePrefixes.Keys);
                break;

            case "Speed":
                dictKeys = new List<string>(speedPrefixes.Keys);
                break;

            case "Range":
                dictKeys = new List<string>(rangePrefixes.Keys);
                break;
        }

        string listPrefix = dictKeys[random.Next(dictKeys.Count)];
        string newPrefix = listPrefix;

        //chooses a suffix / element, but has a small chance
        string newElement;
        string newSuffix;
        int elementChance = random.Next(1, 100);
        if (elementChance <= 10 + PersistantGameManager.Instance.betterLootChance)
        {
            List<string> dictElements = new List<string>(weaponElements.Keys);
            string listElement = dictElements[random.Next(dictElements.Count)];
            newElement = listElement;

            newSuffix = weaponElements[newElement];

        }
        else
        {
            newElement = "";

            newSuffix = "";
        }

        //calculates damage
        float newDamage = random.Next((int)((16 * Math.Pow(newLevel, 2) + 10) * 0.9f), (int)((16 * Math.Pow(newLevel, 2) + 10) * 1.1f));
        newDamage *= damageBonus + (damageBonus * (weaponValue / 100));
        if (prefixType == "Damage")
        {
            newDamage *= damagePrefixes[newPrefix];
        }
        if (newDamage <= 0) { newDamage = 0.1f; }

        //calculates speed
        double tempSpeed;
        while (true)
        {
            tempSpeed = random.NextDouble();
            if (tempSpeed >= 0.5)
            {
                break;
            }
        }
        if (prefixType == "Speed")
        {
            speedBonus *= (2 - speedPrefixes[newPrefix]);
        }
        float newAttackSpeed = (float)tempSpeed;
        newAttackSpeed *= speedBonus * (float)PersistantGameManager.Instance.attackSpeedMulti;

        newAttackSpeed = (float)Math.Round(newAttackSpeed, 2);
        if (newAttackSpeed <= 0) { newAttackSpeed = 0.1f; }

        //calculates range
        float newRange = rangeBonus;

        if (prefixType == "Range")
        {
            newRange *= rangePrefixes[newPrefix];
        }

        if (newRange <= 0) { newRange = 0.1f; }
        return new Weapon(weaponType, newPrefix, newElement, newSuffix, newDamage, newAttackSpeed / (float)PersistantGameManager.Instance.attackSpeedMulti, newAttackSpeed, newRange, newLevel);
    
    }

    public static Weapon GenerateSpecificWeapon(string weaponType, int weaponValue)
    { 

        //Takes specific weapon type for quests, rather than a random weapon choice.

        switch (weaponType)
        {
            case "Dagger":

                rangeBonus = 1f;
                speedBonus = 0.4f;
                damageBonus = 0.6f;
                break;

            case "Short Sword":
                rangeBonus = 1.5f;
                speedBonus = 0.6f;
                damageBonus = 0.8f;
                break;

            case "Long Sword":
                rangeBonus = 2f;
                speedBonus = 1f;
                damageBonus = 1f;
                break;

            case "Lance":
                rangeBonus = 3f;
                speedBonus = 1f;
                damageBonus = 0.8f;
                break;

            case "Axe":
                rangeBonus = 4f;
                speedBonus = 1.5f;
                damageBonus = 1.5f;
                break;
        }

        //quests use player level instead of enemy level
        int playerLevel = PersistantGameManager.Instance.playerStats.playerLevel;

        int newLevel = random.Next(playerLevel - 1, playerLevel + 1);
        if (newLevel <= 0) { newLevel = 1; }


        /*
        float newDamage = random.Next(newLevel - newLevel / 5, newLevel + newLevel / 5);
        newDamage *= damageBonus + (damageBonus * (weaponValue / 100));
        */

        //for better loot skill, might remove
        int randomChance = random.Next(1, 100);
        if(randomChance <= PersistantGameManager.Instance.betterLootChance)
        {
            weaponValue += 20;
        }

        //Decides dictionary to choose prefixes from
        string prefixType = "";
        int randomImprove = random.Next(1, 3);
        switch (randomImprove)
        {
            case 1:
                prefixType = "Damage";
                break;

            case 2:
                prefixType = "Speed";
                break;

            case 3:
                prefixType = "Range";
                break;
        }

        //chooses a prefix
        List<string> dictKeys = new List<string>();
        switch (prefixType)
        {
            case "Damage":
                dictKeys = new List<string>(damagePrefixes.Keys);
                break;

            case "Speed":
                dictKeys = new List<string>(speedPrefixes.Keys);
                break;

            case "Range":
                dictKeys = new List<string>(rangePrefixes.Keys);
                break;
        }

        string listPrefix = dictKeys[random.Next(dictKeys.Count)];
        string newPrefix = listPrefix;

        //chooses a suffix / element, but has a small chance
        string newElement;
        string newSuffix;
        int elementChance = random.Next(1, 100);
        if (elementChance <= 10 + PersistantGameManager.Instance.betterLootChance)
        {
            List<string> dictElements = new List<string>(weaponElements.Keys);
            string listElement = dictElements[random.Next(dictElements.Count)];
            newElement = listElement;

            newSuffix = weaponElements[newElement];
           
        }
        else
        {
            newElement = "";

            newSuffix = "";
        }

        //calculates damage

        float newDamage = random.Next((int)((16 * Math.Pow(newLevel, 2) + 10) * 0.9f), (int)((16 * Math.Pow(newLevel, 2) + 10) * 1.1f));
        newDamage *= damageBonus + (damageBonus * (weaponValue / 100));
        if (prefixType == "Damage")
        {
            newDamage *= damagePrefixes[newPrefix];
        }

        if (newDamage <= 0) { newDamage = 0.1f; }
              
        //calculates speed
        double tempSpeed;
        while (true)
        {   
            tempSpeed = random.NextDouble();
            if (tempSpeed >= 0.5)
            {

                break;
            }
        }

        if (prefixType == "Speed")
        {
            speedBonus *= (2 - speedPrefixes[newPrefix]);
        }

        float newAttackSpeed = (float)Math.Round((decimal)tempSpeed, 2, MidpointRounding.AwayFromZero);
        //float newAttackSpeed = random.Next((newLevel - newLevel / 5) / 10, (newLevel + newLevel / 5) / 10);
        newAttackSpeed *= speedBonus * (float)PersistantGameManager.Instance.attackSpeedMulti;
        newAttackSpeed = (float)Math.Round(newAttackSpeed, 2);
        if (newAttackSpeed <= 0) { newAttackSpeed = 0.1f; }


        //calculates range
        float newRange = rangeBonus;
        if (prefixType == "Range")
        {
            newRange *= rangePrefixes[newPrefix];
        }

        if (newRange <= 0) { newRange = 0.1f; }



       
        
        return new Weapon(weaponType, newPrefix, newElement, newSuffix, newDamage, newAttackSpeed / (float)PersistantGameManager.Instance.attackSpeedMulti, newAttackSpeed, newRange, newLevel);
    }


    //delete
    public static Consumable GenerateConsumable(int value)
    {

        int randomChance = random.Next(0, 100);
        if (randomChance < 40)
        {
            if (value < 20)
            {
                Consumable consumable = new Consumable();
                consumable.type = "20%H";
                return consumable;
            }
            else if (value < 50)
            {
                Consumable consumable = new Consumable();
                consumable.type = "50%H";
                return consumable;
            }
            else
            {
                Consumable consumable = new Consumable();
                consumable.type = "100%H";
                return consumable;
            }
        }

        else if(randomChance < 80)
        {
            if (value < 20)
            {
                Consumable consumable = new Consumable();
                consumable.type = "20%A";
                return consumable;
            }
            else if (value < 50)
            {
                Consumable consumable = new Consumable();
                consumable.type = "50%A";
                return consumable;
            }
            else
            {
                Consumable consumable = new Consumable();
                consumable.type = "100%A";
                return consumable;
            }
        }
        else
        {
            Consumable consumable = new Consumable();
            consumable.type = "20%L";
            return consumable;
        }


    }

}
