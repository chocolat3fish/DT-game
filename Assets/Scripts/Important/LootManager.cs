using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//******MUST BE CHILDED TO THE LOOT DROP PREFAB******\\

//A script that can be referneced by other scrits and controlls loot drops
public class LootManager : MonoBehaviour
{
    

    //uses random from system rather than unityengine, allows for random.Next() 
    //which genreates a random number between ranges
    private static System.Random random = new System.Random();

    //The main referenced method that controls wether a weapon is dropped
    //Based off a chance out of 100 and the value of the weapon
    public static LootItem DropItem(int chance, int itemValue, int weaponValue)
    {
        //Generates a random number between 1 and 100 
        int randomChance = random.Next(0, 100);

        //Checks if the chance is bigger that the generated number
        //This gives a n% where n is chance
        if (chance > randomChance)
        {


            LootItem lootItem = new LootItem();
            lootItem.type = 0;
            lootItem.newWeapon = GenerateWeapon(weaponValue);
            return lootItem;
            


            //calls the method that generates the weapon based off the weapon value

        }
        else
        {
            LootItem lootItem = new LootItem();
            lootItem.type = 1;
            lootItem.consumable = GenerateConsumable(itemValue);
            return lootItem;
        }
    }

    //A method called by the Drop item script to find a weapon based off a weapon value
    public static Weapon GenerateWeapon(int weaponValue)
    {
        //Wether it has is still choosing the type of weapon to use
        bool selectingTypeOfWeapon = true;

        //Different weapons that can be selected
        bool allowsDaggers = true;
        bool allowsShortSwords = false;
        bool allowsLongSwords = false;
        bool allowsLances = false;
        bool allowsAxes = false;

        //how much damage to add to the weapon at the end
        int powerBoost = 0;

        //A random number
        int randomNumber;

        //Information for 
        float minAS = 0, maxAS = 0, minR = 0, maxR = 0;
        int minD = 0, maxD = 0;

        string weaponType = "";

        if (weaponValue > 5)
        {
            allowsShortSwords = true;
        }
        if (weaponValue > 10)
        {
            allowsLongSwords = true;
        }
        if (weaponValue > 15)
        {
            allowsLances = true;
        }
        if (weaponValue > 20)
        {
            allowsAxes = true;
        }
        if (weaponValue > 60)
        {
            powerBoost = weaponValue - 40;
        }

        while (selectingTypeOfWeapon)
        {
            randomNumber = random.Next(0, 4);
            if (randomNumber == 0 && allowsDaggers)
            {
                weaponType = "Dagger";
                minD = 1;
                if (weaponValue >= 25) { maxD = 25; }
                else { maxD = weaponValue; }
                minAS = 0.25f;
                maxAS = 0.75f;
                minR = 0.5f;
                maxR = 1;

                selectingTypeOfWeapon = false;
            }
            else if (randomNumber == 1 && allowsShortSwords)
            {
                weaponType = "Short Sword";
                minD = 1;
                if (weaponValue >= 30) { maxD = 30; }
                else { maxD = weaponValue; }
                minAS = 0.5f;
                maxAS = 1f;
                minR = 1f;
                maxR = 1.25f;

                selectingTypeOfWeapon = false;
            }
            else if (randomNumber == 2 && allowsLongSwords)
            {
                weaponType = "Long Sword";
                minD = 1;
                if (weaponValue >= 40) { maxD = 40; }
                else { maxD = weaponValue; }
                minAS = 1f;
                maxAS = 1.5f;
                minR = 1f;
                maxR = 1.5f;

                selectingTypeOfWeapon = false;
            }
            else if (randomNumber == 3 && allowsLances)
            {
                weaponType = "Lance";
                minD = 1;
                if (weaponValue >= 50) { maxD = 50; }
                else { maxD = weaponValue; }
                minAS = 1.5f;
                maxAS = 2f;
                minR = 1.5f;
                maxR = 2f;

                selectingTypeOfWeapon = false;
            }
            else if (randomNumber == 4 && allowsAxes)
            {
                weaponType = "Axes";
                minD = 1;
                if (weaponValue >= 60) { maxD = 60; }
                else { maxD = weaponValue; }
                minAS = 1.5f;
                maxAS = 2f;
                minR = 0.75f;
                maxR = 1.25f;

                selectingTypeOfWeapon = false;
            }


        }
        //new stats of newWeapon taken from the max and min damage with power boost added for damage
        float newDamage = (random.Next(minD, (maxD + 5)));
        if (newDamage > maxD) { newDamage = maxD; }
        newDamage += powerBoost;
        if (newDamage == 0) { newDamage = 1; }

        float newAttackSpeed = (random.Next((int)((minAS- 0.25f) * 100), (int)(maxAS * 100)))/100.0f;
        newAttackSpeed = (float)(Math.Round(newAttackSpeed,2));
        if (newAttackSpeed < minAS) { newAttackSpeed = maxAS; }

        float newRange = (random.Next((int)(minR * 100), (int)((maxR + 0.25f) * 100)))/100.0f;
        newRange = (float)(Math.Round(newRange ,2));
        if (newRange > maxR) { newRange = maxR; }

        Debug.Log(weaponType);
        Debug.Log(newDamage);
        Debug.Log(newAttackSpeed);
        Debug.Log(newRange);
        return new Weapon(weaponType, newDamage, newAttackSpeed, newRange);
        

    
    }
    public static Consumable GenerateConsumable(int value)
    {
        int randomChance = random.Next(0, 100);
        //if (randomChance < 50)
        //{
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
        //}
        /*
        else
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
        */

    }

}
