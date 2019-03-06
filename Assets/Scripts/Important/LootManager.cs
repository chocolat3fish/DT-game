using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    

    //uses random from system rather than unityengine, allows for random.Next()

    private static System.Random random = new System.Random();

    public static Weapon DropItem(int chance, int weaponValue)
    {
        int randomChance = random.Next(0, 100);

        if (chance > randomChance)
        {
            return GenerateWeapon(weaponValue);
        }
        else
        {
            return null;
        }
    }
    public static Weapon GenerateWeapon(int weaponValue)
    {
        bool selectingTypeOfWeapon = true;

        bool allowsDaggers = true;
        bool allowsShortSwords = false;
        bool allowsLongSwords = false;
        bool allowsLances = false;
        bool allowsAxes = false;

        int powerBoost = 0;
        int randomNumber;

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
}
