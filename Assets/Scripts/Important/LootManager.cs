using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    //uses random from system rather than unityengine, allows for random.Next()

    private static System.Random random = new System.Random();
    public static Weapon DropItem(int chance)
    {
        int randomChance = random.Next(0, 100);

        if (chance > randomChance)
        {
            return GenerateWeapon(chance);
        }
        else
        {
            return null;
        }
    }
    public static Weapon GenerateWeapon(int chance)
    {
        Debug.Log("Dropped Weapon");

        //temporary
        return new Weapon("Temp", 10f, 0.2f);

    }
}
