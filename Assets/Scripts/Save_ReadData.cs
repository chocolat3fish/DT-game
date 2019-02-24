using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
public class Save_ReadData : MonoBehaviour
{
    public int health;
    public int xp;
    public int gold;

    public PlayerStats playerStats;
    JsonData playerJson;
    string jsonDataFromFile;
    private void Start()
    {
        if (File.Exists(Application.dataPath + "/SavedData/Player.json"))
        {
            jsonDataFromFile = File.ReadAllText(Application.dataPath + "/SavedData/Player.json");

            playerStats = JsonMapper.ToObject<PlayerStats>(jsonDataFromFile);
            Debug.Log(playerStats);
            Debug.Log("Complete");
            health = playerStats.health;
            xp = playerStats.xp;
            gold = playerStats.gold;

        }
        else
        {
            playerStats = PlayerStats.PlayerStatsConstrutor(1,2,3);
            playerJson = JsonMapper.ToJson(playerStats);
            File.WriteAllText(Application.dataPath + "/SavedData/Player.json", playerJson.ToString());
            health = playerStats.health;
            xp = playerStats.xp;
            gold = playerStats.gold;
            SaveObjectToFile<PlayerStats>(playerStats, "Player");
        }

    }

    public static T SaveObjectToFile<T>(T input, string fileSaveName)
    {
        JsonData TJson;
        TJson = JsonMapper.ToJson(input);
        File.WriteAllText(Application.dataPath + "/SavedData/" + fileSaveName + ".json", TJson.ToString());
        return input;
    }

    public static T GetObjectFromJson<T>(string fileEnd)
    {
        string jsonDataFromFile = File.ReadAllText(Application.dataPath + "/" + fileEnd + ".json");
        T typeT = JsonMapper.ToObject<T>(jsonDataFromFile);
        return typeT;
    }

    
}



