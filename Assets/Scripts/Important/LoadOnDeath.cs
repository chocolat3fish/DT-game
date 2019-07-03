using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//This script controlls load from the death scene
public class LoadOnDeath : MonoBehaviour
{
    public Text L1T, L2T, L3T, L4T;
    Button Confirm, Decline, Back;
    Timestamps timestamps = new Timestamps();
    Color32 emptyC = new Color32(192, 0, 0, 255);
    Color32 defaultC = new Color32(50, 50, 50, 255);

    private void Awake()
    {
        L1T = transform.Find("L1T").GetComponent<Text>();
        L2T = transform.Find("L2T").GetComponent<Text>();
        L3T = transform.Find("L3T").GetComponent<Text>();
        L4T = transform.Find("L4T").GetComponent<Text>();

        //If the directory containing save information doesn't exist then it creates it
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
            file = File.Create(Application.persistentDataPath + "/SavedData/Slot4.txt");

            Reset();
        }

        UpdateTimestamps();

        //Adds the on click listener to the buttons
        transform.Find("Slot1").GetComponent<Button>().onClick.AddListener(delegate { Load(1); });
        transform.Find("Slot2").GetComponent<Button>().onClick.AddListener(delegate { Load(2); });
        transform.Find("Slot3").GetComponent<Button>().onClick.AddListener(delegate { Load(3); });
        transform.Find("Slot4").GetComponent<Button>().onClick.AddListener(delegate { Load(4); });
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
    //Perform the load
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
        else if (slot == 4 && timestamps.S4T == "Empty") 
        {
            L4T.color = emptyC;
            StartCoroutine(Shake(L4T.gameObject, defaultC));
        }

        else
        {
            PersistantGameManager.Instance.LoadDataFromSave(slot);
        }
        
    }


    private void UpdateTimestamps()
    {
        StopAllCoroutines();
        timestamps = GetTimestamps();
        L1T.text = timestamps.S1T;
        L2T.text = timestamps.S2T;
        L3T.text = timestamps.S3T;
        L4T.text = timestamps.S4T;
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
            S3T = "Empty",
            S4T = "Empty"
        };


        bF.Serialize(file, emptyTimestamps);
        file.Close();

        UpdateTimestamps();
    }
}
