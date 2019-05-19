using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using System;

public class LoadAndSave : MonoBehaviour
{
    Text S1T, S2T, S3T, L1T, L2T, L3T;
    Timestamps timestamps = new Timestamps();
    private void Awake()
    {
        S1T = transform.Find("S1T").GetComponent<Text>();
        S2T = transform.Find("S2T").GetComponent<Text>();
        S3T = transform.Find("S3T").GetComponent<Text>();
        L1T = transform.Find("L1T").GetComponent<Text>();
        L2T = transform.Find("L2T").GetComponent<Text>();
        L3T = transform.Find("L3T").GetComponent<Text>();
        UpdateTimestamps();

    }
    public void Save(int slot)
    {
        string jsonData = File.ReadAllText(Application.dataPath + "/SavedData/Timestamps.json");
        timestamps = JsonConvert.DeserializeObject<Timestamps>(jsonData);
        print(slot);
        if(slot == 1)
        {
            timestamps.S1T = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().Length - 8);
        }
        if (slot == 2)
        {
            timestamps.S2T = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().Length - 8);
        }
        if (slot == 3)
        {
            timestamps.S3T = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().Length - 8);
        }
        File.WriteAllText(Application.dataPath + "/SavedData/Timestamps.json", JsonConvert.SerializeObject(timestamps, Formatting.Indented));
        PersistantGameManager.Instance.SaveGameManagerData(slot);
        UpdateTimestamps();
    }

    public void Load(int slot)
    {
        PersistantGameManager.Instance.LoadDataFromSave(slot);
    }

    private void UpdateTimestamps()
    {
        string jsonData = File.ReadAllText(Application.dataPath + "/SavedData/Timestamps.json");
        timestamps = JsonConvert.DeserializeObject<Timestamps>(jsonData);
        S1T.text = timestamps.S1T;
        S2T.text = timestamps.S2T;
        S3T.text = timestamps.S3T;
        L1T.text = timestamps.S1T;
        L2T.text = timestamps.S2T;
        L3T.text = timestamps.S3T;
    }
}
