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
    GameObject ConfirmPanel;
    Text Title, Question, LoadOrSaveDetails;
    Button Confirm, Decline;
    Timestamps timestamps = new Timestamps();
    Color32 emptyC = new Color32(192, 0, 0, 255);
    Color32 defaultC = new Color32(50, 50, 50, 255);
    private void Awake()
    {
        S1T = transform.Find("S1T").GetComponent<Text>();
        S2T = transform.Find("S2T").GetComponent<Text>();
        S3T = transform.Find("S3T").GetComponent<Text>();
        L1T = transform.Find("L1T").GetComponent<Text>();
        L2T = transform.Find("L2T").GetComponent<Text>();
        L3T = transform.Find("L3T").GetComponent<Text>();


        ConfirmPanel = transform.Find("Confirm").gameObject;
 
        Title = ConfirmPanel.transform.Find("Title").GetComponent<Text>();
        Question = ConfirmPanel.transform.Find("Question").GetComponent<Text>();
        LoadOrSaveDetails = ConfirmPanel.transform.Find("LoadOrSaveDetails").GetComponent<Text>();

        Confirm = ConfirmPanel.transform.Find("ConfirmB").GetComponent<Button>();
        Decline = ConfirmPanel.transform.Find("DeclineB").GetComponent<Button>();
        UpdateTimestamps();

        ConfirmPanel.SetActive(false);

    }

    public void Save(int slot)
    {
        string jsonData = File.ReadAllText(Application.dataPath + "/SavedData/Timestamps.json");
        timestamps = JsonConvert.DeserializeObject<Timestamps>(jsonData);
        bool shouldSave = false;
        if(slot == 1 && timestamps.S1T == "Empty")
        {
            timestamps.S1T = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().Length - 8);
            shouldSave = true;
        }
        else if (slot == 2 && timestamps.S2T == "Empty")
        {
            timestamps.S2T = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().Length - 8);
            shouldSave = true;
        }
        else if (slot == 3 && timestamps.S3T == "Empty")
        {
            timestamps.S3T = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().Length - 8);
            shouldSave = true;
        }

        if(shouldSave)
        {
            File.WriteAllText(Application.dataPath + "/SavedData/Timestamps.json", JsonConvert.SerializeObject(timestamps, Formatting.Indented));
            PersistantGameManager.Instance.SaveGameManagerData(slot);
            UpdateTimestamps();
        }
        else
        {
            if(slot == 1)
            {
                OpenConfirmPanel(false, slot, timestamps.S1T);
            }
            if (slot == 2)
            {
                OpenConfirmPanel(false, slot, timestamps.S2T);
            }
            if (slot == 3)
            {
                OpenConfirmPanel(false, slot, timestamps.S3T);
            }
        }
    }

    public void OpenConfirmPanel(bool load, int slot, string time)
    {
        ConfirmPanel.SetActive(true);
        if(load)
        {
            Title.text = "Load";
            Question.text = "Are you sure you want to load?\nIt will override your current game!\nMake sure you have saved if you want to";
            LoadOrSaveDetails.text = "Load from: Slot " + slot + "\nSaved at: " +  time;
            Confirm.onClick.RemoveAllListeners();
            Confirm.onClick.AddListener(delegate { PersistantGameManager.Instance.LoadDataFromSave(slot); });
        }
        else
        {
            Title.text = "Save";
            Question.text = "Are you sure you want to save?\nIt will override your current saved game!";
            LoadOrSaveDetails.text = "Overwrite: Slot " + slot + "\nSaved at: " + time;
            Confirm.onClick.RemoveAllListeners();
            Confirm.onClick.AddListener(delegate { SaveOverride(slot); CloseConfirmPanel(); });
        }
    }
    
    public void CloseConfirmPanel()
    {
        ConfirmPanel.SetActive(false);
    }

    public void Load(int slot)
    {
        string jsonData = File.ReadAllText(Application.dataPath + "/SavedData/Timestamps.json");
        timestamps = JsonConvert.DeserializeObject<Timestamps>(jsonData);
        if (slot == 1 && timestamps.S1T == "Empty")
        {
            L1T.color = emptyC;
            StartCoroutine(Shake(L1T));
        }
        else if (slot == 2 && timestamps.S2T == "Empty")
        {
            L2T.color = emptyC;
            StartCoroutine(Shake(L2T));
        }
        else if (slot == 3 && timestamps.S3T == "Empty")
        {
            L3T.color = emptyC;
            StartCoroutine(Shake(L3T));
        }
        else
        {
            if (slot == 1)
            {
                OpenConfirmPanel(true, slot, timestamps.S1T);
            }
            if (slot == 2)
            {
                OpenConfirmPanel(true, slot, timestamps.S2T);
            }
            if (slot == 3)
            {
                OpenConfirmPanel(true, slot, timestamps.S3T);
            }
        }
        
    }
    public void SaveOverride(int slot)
    {
        string jsonData = File.ReadAllText(Application.dataPath + "/SavedData/Timestamps.json");
        timestamps = JsonConvert.DeserializeObject<Timestamps>(jsonData);
        if (slot == 1)
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

    private void UpdateTimestamps()
    {
        StopAllCoroutines();
        string jsonData = File.ReadAllText(Application.dataPath + "/SavedData/Timestamps.json");
        timestamps = JsonConvert.DeserializeObject<Timestamps>(jsonData);
        S1T.text = timestamps.S1T;
        S2T.text = timestamps.S2T;
        S3T.text = timestamps.S3T;
        L1T.text = timestamps.S1T;
        L2T.text = timestamps.S2T;
        L3T.text = timestamps.S3T;
    }

    IEnumerator Shake(Text text)
    {
        Vector3 orignalPos = text.transform.position;
        text.transform.position -= new Vector3(2.5f, 0, 0);
        for(int i = 0; i < 10; i++)
        {
            yield return null;
            text.transform.position += new Vector3(5f, 0, 0);
            yield return null;
            text.transform.position -= new Vector3(5f, 0, 0);
        }
        text.transform.position = orignalPos;
        text.color = defaultC;
    }

    public void Reset()
    {
        string timestamps = File.ReadAllText(Application.dataPath + "/SavedData/EmptyTimestamps.json");
        File.WriteAllText(Application.dataPath + "/SavedData/Timestamps.json", timestamps);
        UpdateTimestamps();
    }
}
