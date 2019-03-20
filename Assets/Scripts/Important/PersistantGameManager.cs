using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using Newtonsoft.Json;
public class PersistantGameManager : MonoBehaviour
{
    public static PersistantGameManager Instance { get; private set; }

    public int currentIndex = 0;
    public int previousIndex = 0;
    public Weapon currentWeapon;
    public List<Weapon> playerWeaponInventory = new List<Weapon>();
    public Dictionary<string, int>amountOfItems = new Dictionary<string, int>()
    {
        {"20%H", 0},
        {"50%H", 0},
        {"100%H", 0},
        //{"20%A", 0},
        //{"50%A", 0},
        //{"100%A", 0}
    };
    public bool compareScreenOpen;
    public bool characterScreenOpen;
    public bool menuScreenOpen;
    public Weapon comparingWeapon;
    public Armour currentArmour;
    public Armour comparingArmour;
    public PlayerControls player;
    public Camera camera;

    public GameObject magicBar;

    public bool hasMagic;
    public bool tripleJump;
    public bool fireball;
    public bool gripWalls;

    public PlayerStats playerStats;
    public bool checkExp;

    public string currentScene;

    public float totalExperience;

    public string equippedItemOne, equippedItemTwo;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        currentScene = SceneManager.GetActiveScene().name;

    }
    private void Start()
    {

        //Shortened to 3 weapons
        for(int i = 0; i <3; i++)
        {
            playerWeaponInventory.Add(null);
        }

        TestGiveItem.GiveItem();
        player = FindObjectOfType<PlayerControls>();
        currentWeapon = playerWeaponInventory[0];
        //LoadDataFromSave();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentIndex = 0;
            ChangeItem(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentIndex = 1;
            ChangeItem(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentIndex = 2;
            ChangeItem(currentIndex);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            equippedItemOne = "20%H";
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            equippedItemOne = "50%H";
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            equippedItemOne = "100%H";
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            equippedItemTwo = "20%H";
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            equippedItemTwo = "50%H";
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            equippedItemTwo = "100%H";
        }

        //Shortened to 3 items
        /*
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentIndex = 3;
            ChangeItem(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentIndex = 4;
            ChangeItem(currentIndex);
        }
        */

        //Hides the magic cooldown if player has not unlocked any magic skills
        if (hasMagic == true)
        {
            magicBar.SetActive(true);
        }

        else if (hasMagic == false)
        {
            magicBar.SetActive(false);
        }



        if (currentWeapon == null)
        {
            ChangeItem(99);
        }

        if(currentScene != SceneManager.GetActiveScene().ToString())
        {
            OnSceneChange();
        }

        if (checkExp)
        {

            //Temporary values
            if (playerStats.playerExperience >= totalExperience)
            {
                int levels = 0;
                int skillPoints = 0;

                while (playerStats.playerExperience > totalExperience)
                {
                    levels += 1;
                    skillPoints += 1;
                    playerStats.playerExperience -= 300;
                }

                playerStats.playerSkillPoints += skillPoints;
                playerStats.playerLevel += levels;
            }
        }

    }
    public void ChangeItem(int index)
    {
        if(index == 99)
        {
            currentWeapon = playerWeaponInventory[0];
            previousIndex = 0;
        }
        //shortened to 3 items
        else if(index != previousIndex && index < 3 && index > -1)
        {

            if(playerWeaponInventory[index] != null )
            {
                currentWeapon = playerWeaponInventory[index];
                previousIndex = currentIndex;
            }

        }
    }
   public void SaveGameManagerData()
    {
        GameManagerData data = new GameManagerData();
        data.currentIndex = currentIndex;
        data.currentWeapon = currentWeapon;
        data.playerWeaponInventory = playerWeaponInventory;
        data.currentArmour = currentArmour;
        data.playerStats = playerStats;
        data.totalExperience = totalExperience;
        data.currentScene = currentScene;
        File.WriteAllText(Application.dataPath + "/SavedData/GameManagerData.json", JsonConvert.SerializeObject(data, Formatting.Indented));
        Debug.Log("Saved");

    }
    public void LoadDataFromSave()
    {
        string jsonData = File.ReadAllText(Application.dataPath + "/SavedData/GameManagerData.json");
        GameManagerData data = JsonConvert.DeserializeObject<GameManagerData>(jsonData);
        currentIndex = data.currentIndex;
        playerWeaponInventory = data.playerWeaponInventory;
        currentArmour = data.currentArmour;
        playerStats = data.playerStats;
        totalExperience = data.totalExperience;
        SceneManager.LoadScene(data.currentScene);
        currentScene = data.currentScene;
        checkExp = true;
        Debug.Log("Load");
    }


    private void OnSceneChange()
    {
        player = FindObjectOfType<PlayerControls>();
        camera = FindObjectOfType<Camera>();
        DoorMonitor[] doors = FindObjectsOfType<DoorMonitor>();
        foreach (DoorMonitor door in doors)
        {
            if (door.gameObject.name.Replace(" Door","") == currentScene)
            {
                player.transform.position = door.transform.position;
                camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y , -10f);

            }
        }

        currentScene = SceneManager.GetActiveScene().name;
    }
}
