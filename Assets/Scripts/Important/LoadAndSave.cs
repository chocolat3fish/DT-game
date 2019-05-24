using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadAndSave : MonoBehaviour
{
    public Text L1T, L2T, L3T;
    GameObject ConfirmPanel;
    Text Title, Question, LoadOrSaveDetails;
    Button Confirm, Decline, Back;
    Timestamps timestamps = new Timestamps();
    InputField inputField;
    Color32 emptyC = new Color32(192, 0, 0, 255);
    Color32 defaultC = new Color32(50, 50, 50, 255);
    Color32 iFDefaultC;
    private void Awake()
    {
        L1T = transform.Find("L1T").GetComponent<Text>();
        L2T = transform.Find("L2T").GetComponent<Text>();
        L3T = transform.Find("L3T").GetComponent<Text>();

        ConfirmPanel = transform.Find("Confirm").gameObject;
 
        Title = ConfirmPanel.transform.Find("Title").GetComponent<Text>();
        Question = ConfirmPanel.transform.Find("Question").GetComponent<Text>();
        LoadOrSaveDetails = ConfirmPanel.transform.Find("LoadOrSaveDetails").GetComponent<Text>();

        Confirm = ConfirmPanel.transform.Find("ConfirmB").GetComponent<Button>();
        Decline = ConfirmPanel.transform.Find("DeclineB").GetComponent<Button>();

        Back = transform.Find("Back").GetComponent<Button>();
        Back.onClick.AddListener(delegate { FindObjectOfType<AsyncTriggers>().CloseSaveAndLoadCanvas(); });

        inputField = ConfirmPanel.transform.Find("InputField").GetComponent<InputField>();
        iFDefaultC = inputField.transform.Find("Placeholder").GetComponent<Text>().color;
        ConfirmPanel.SetActive(false);

        if (!File.Exists(Application.persistentDataPath + "/SavedData/Timestamps.txt"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SavedData");

            FileStream file;
            file = File.Create(Application.persistentDataPath + "/SavedData/Timestamps.txt");
            file.Close();

            file = File.Create(Application.persistentDataPath + "/SavedData/Slot1.txt");
            file.Close();
            file = File.Create(Application.persistentDataPath + "/SavedData/Slot2.txt");
            file.Close();
            file = File.Create(Application.persistentDataPath + "/SavedData/Slot3.txt");
            file.Close();

            Reset();
        }

        UpdateTimestamps();
    }
    public Timestamps GetTimestamps()
    {
        BinaryFormatter bF = new BinaryFormatter();
        FileStream file;
        file = File.Open(Application.persistentDataPath + "/SavedData/Timestamps.txt", FileMode.Open);
        Timestamps returnData = (Timestamps)bF.Deserialize(file);
        file.Close();
        return returnData;
    }
    public void SaveTimestamps(Timestamps data)
    {
        BinaryFormatter bF = new BinaryFormatter();
        FileStream file;
        file = File.Open(Application.persistentDataPath + "/SavedData/Timestamps.txt", FileMode.Open);
        bF.Serialize(file, data);
        file.Close();
    }
    public void Save(int slot)
    {
        timestamps = GetTimestamps();

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
            if (slot == 1)
            {
                OpenConfirmPanel(false, false, slot, timestamps.S1T);
            }
            if (slot == 2)
            {
                OpenConfirmPanel(false, false, slot, timestamps.S2T);
            }
            if (slot == 3)
            {
                OpenConfirmPanel(false, false, slot, timestamps.S3T);
            }

            UpdateTimestamps();
        }
        else
        {
            if(slot == 1)
            {
                OpenConfirmPanel(false, true, slot, timestamps.S1T);
            }
            if (slot == 2)
            {
                OpenConfirmPanel(false, true, slot, timestamps.S2T);
            }
            if (slot == 3)
            {
                OpenConfirmPanel(false, true, slot, timestamps.S3T);
            }
        }
    }

    public void OpenConfirmPanel(bool load, bool overwrite, int slot, string time)
    {
        ConfirmPanel.SetActive(true);
        if(load)
        {
            Title.text = "Load";
            Question.text = "Are you sure you want to load?\nIt will override your current game!\nMake sure you have saved if you want to";
            LoadOrSaveDetails.text = "Load from: Slot " + slot + "\nSaved at: " +  time;
            Confirm.onClick.RemoveAllListeners();
            inputField.gameObject.SetActive(false);
            Confirm.onClick.AddListener(delegate { PersistantGameManager.Instance.LoadDataFromSave(slot); });
        }
        else
        {
            if (overwrite)
            {
                Title.text = "Save";
                Question.text = "Are you sure you want to save?\nIt will override your current saved game!";
                LoadOrSaveDetails.text = "Override: Slot " + slot + "\nSave: " + time + "\nPlease enter name of game:";
                inputField.gameObject.SetActive(true);
                Text placeholder = inputField.transform.Find("Placeholder").GetComponent<Text>();
                placeholder.text = "Please enter name of game...";
                placeholder.color = iFDefaultC;
                inputField.text = "";
                Confirm.onClick.RemoveAllListeners();
                Confirm.onClick.AddListener(delegate { SaveOverride(slot); });
            }
            else
            {
                Title.text = "Save";
                Question.text = "You are saving over an empty slot.\nSaving allows you to load a game and keep your progress.";
                LoadOrSaveDetails.text = "\n\n\nPlease enter name of game:";
                inputField.gameObject.SetActive(true);
                Text placeholder = inputField.transform.Find("Placeholder").GetComponent<Text>();
                placeholder.text = "Please enter name of game...";
                placeholder.color = iFDefaultC;
                inputField.text = "";
                Confirm.onClick.RemoveAllListeners();
                Confirm.onClick.AddListener(delegate { SaveOverride(slot); });
            }
        }
    }
    
    public void CloseConfirmPanel()
    {
        ConfirmPanel.SetActive(false);
    }

    public void Load(int slot)
    {
        timestamps = GetTimestamps();
        if (slot == 1 && timestamps.S1T == "Empty")
        {
            L1T.color = emptyC;
            StartCoroutine(Shake(L1T.gameObject, defaultC));
        }
        else if (slot == 2 && timestamps.S2T == "Empty")
        {
            L2T.color = emptyC;
            StartCoroutine(Shake(L2T.gameObject, defaultC));
        }
        else if (slot == 3 && timestamps.S3T == "Empty")
        {
            L3T.color = emptyC;
            StartCoroutine(Shake(L3T.gameObject, defaultC));
        }
        else
        {
            if (slot == 1)
            {
                OpenConfirmPanel(true, false, slot, timestamps.S1T);
            }
            if (slot == 2)
            {
                OpenConfirmPanel(true, false, slot, timestamps.S2T);
            }
            if (slot == 3)
            {
                OpenConfirmPanel(true, false, slot, timestamps.S3T);
            }
        }
        
    }

    public void SaveOverride(int slot)
    {
        if(inputField.text == "")
        {
            Text placeholder = inputField.transform.Find("Placeholder").GetComponent<Text>();
            placeholder.text = "Please fill in";
            placeholder.color = emptyC;
            StartCoroutine(Shake(placeholder.gameObject, iFDefaultC));
        }
        else
        {
            timestamps = GetTimestamps();
            if (slot == 1)
            {
                timestamps.S1T = inputField.text + "\n" + DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().Length - 8);
            }
            if (slot == 2)
            {
                timestamps.S2T = inputField.text + "\n" + DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().Length - 8);
            }
            if (slot == 3)
            {
                timestamps.S3T = inputField.text + "\n" + DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().Length - 8);
            }

            SaveTimestamps(timestamps);
            PersistantGameManager.Instance.SaveGameManagerData(slot);
            CloseConfirmPanel();
            UpdateTimestamps();
        }

    }

    private void UpdateTimestamps()
    {
        StopAllCoroutines();
        timestamps = GetTimestamps();
        Debug.Log(timestamps.S1T);
        L1T.text = timestamps.S1T;
        L2T.text = timestamps.S2T;
        L3T.text = timestamps.S3T;
    }

    IEnumerator Shake(GameObject text, Color32 defaultC)
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
        text.GetComponent<Text>().color = defaultC;
    }

    public void Reset()
    {
        BinaryFormatter bF = new BinaryFormatter();
        FileStream file;
        file = File.Open(Application.persistentDataPath + "/SavedData/Timestamps.txt", FileMode.Open);
        Timestamps emptyTimestamps = new Timestamps()
        {
            S1T = "Empty",
            S2T = "Empty",
            S3T = "Empty"
        };


        bF.Serialize(file, emptyTimestamps);
        file.Close();

        UpdateTimestamps();
    }
}
