using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PersistantGameManager : MonoBehaviour
{
    public static PersistantGameManager Instance { get; private set; }


    [Header("Quests")]


    public Dictionary<string, int> itemInventory = new Dictionary<string, int>();

    //[NonSerialized]
    public List<string> possibleItems = new List<string> 
    {"Dagger of Kaliphase",
     "Amulet of Honour",
     "Hood of Sartuka",
     "Claw of Straphagus",
     "Jason's Belt",
     "Steve's Wristband",
     "Corrupt Key",
     "Kindred Relic",
     "Drowned Relic",
     "Magic Potion",
     "Strange Mechanism"};

    public Quest currentDialogueQuest;

    public Dictionary<string, int> characterQuests = new Dictionary<string, int>();

    public Dictionary<string, Quest> possibleQuests = new Dictionary<string, Quest>();

    public List<string> activeQuests = new List<string>();

    public Dictionary<string, int> currentEnemyKills = new Dictionary<string, int>();



    [Header("Skill Multipliers")]
    //Skill related multipliers
    public double attackSpeedMulti = 1;
    public float attackRangeMulti = 1;
    public float currentAttackMultiplier = 1; // player damage multi
    public float smiteDamageMulti = 1; // skill damage multi
    public float smiteDurationMulti = 1; // skill's duration effect damage multi
    public float lifeStealMulti = 0;
    public float totalHealthMulti = 0;
    public float damageResistMulti = 1;
    public float turtleResistMulti = 0.5f; // player resist multi
    public float turtleMultiMulti = 1; // skill resistance multi
    public float turtleDurationMulti = 1; 
    public float movementResistMulti = 1;
    public float moveSpeedMulti = 1;
    public float jumpHeightMulti = 1;
    public float airAttackMulti = 1;
    public float instantKillChance = 0;
    public float betterLootChance = 0;

    [Header("Menu Statuses")]
    public bool compareScreenOpen;
    public bool characterScreenOpen;
    public bool skillsScreenOpen;
    public bool menuCanvasOpen;
    public Weapon comparingWeapon;
    public bool firstTimeOpeningMenuCanvas, menuCanvasIsOpen;
    public bool dialogueSceneIsOpen;
    public bool bugReportSceneIsOpen;
    public bool saveAndLoadSceneIsOpen;

    [Header("Objects")]
    public PlayerControls player;
    public GameObject _camera;
    public GameObject magicBar;
    public LevelUpController levelUpController;


    [Header("Skill Trackers")]

    public bool hasMagic;
    public string currentActiveAbility = "";
    public bool abilityIsActive;
    public float timeOfAbility;
    public float abilityDuration;
    public float damageResistDuration = 15;
    public float smiteDuration = 10;
    public bool checkSkills;
    public bool checkExp;

    //tracks skill progression of each tree
    public Dictionary<string, int> skillLevels = new Dictionary<string, int>
    {
        {"AttackSpeed", 0},
        {"Smite", 0},
        {"SmiteDuration", 0},
        {"SmiteDamage", 0},
        {"LifeSteal", 0},
        {"AirAttack", 0},
        {"DefenceWithMovement", 0},
        {"MoveSpeed", 0},
        {"TripleJump", 0},
        {"JumpHeight", 0},
        {"GripWalls", 0},
        {"HealthBonus", 0},
        {"Turtle", 0},
        {"TurtleDuration", 0},
        {"TurtleDefense", 0},
        {"WeaponDropValue", 0},
        {"InstantKill", 0},
    };

    [Header("Other")]
    public string currentScene;
    public string previousScene;
    public int currentIndex = 0;
    public int previousIndex = 0;
    public bool tutorialComplete;
    public int lastEnemyLevel;

    [Header("Stats")]
    public float totalExperience;
    public bool levellingUp;
    public bool GodMode;
    public Weapon currentWeapon;
    public List<Weapon> playerWeaponInventory = new List<Weapon>();
    public PlayerStats playerStats;

    [Header("Obsolete")]
    public int damageProgress;
    public int tankProgress;
    public int mobilityProgress;
    public int attackSpeedUpgrades;
    public bool potionIsActive;
    public string activePotionType;

    public float currentLeechMultiplier = 0;
    public float timeOfAttackMultiplierChange;
    public float timeOfLeechMultiplierChange;
    public float healthPotionUseTime;
    public float potionCoolDownTime;

    public Armour currentArmour;
    public Armour comparingArmour;

    public bool tripleJump;
    public bool hasSmite;
    public bool gripWalls;
    public bool maxedSpeed;
    public bool damageResist;

    public bool justReloaded;

    public List<string> completedQuests = new List<string>();


    private void Awake()
    {
        if(Instance == null || gameObject.name == "PersistantGameManager - Reload")
        {
            if (gameObject.name == "PersistantGameManager - Reload")
            {
                gameObject.name = "PersistantGameManager";
                GameManagerData data = FindObjectOfType<LoadSceneMonitor>().data;
                Destroy(FindObjectOfType<LoadSceneMonitor>().gameObject);
                #region UpdateData
                itemInventory = data.itemInventory;
                possibleItems = data.possibleItems;
                currentDialogueQuest = data.currentDialogueQuest;
                characterQuests = data.characterQuests;
                possibleQuests = data.possibleQuests;
                activeQuests = data.activeQuests;

                attackSpeedMulti = data.attackSpeedMulti;
                attackRangeMulti = data.attackRangeMulti;
                currentAttackMultiplier = data.currentAttackMultiplier;
                smiteDamageMulti = data.smiteDamageMulti;
                smiteDurationMulti = data.smiteDurationMulti;
                lifeStealMulti = data.lifeStealMulti;
                totalHealthMulti = data.totalHealthMulti;
                damageResistMulti = data.damageResistMulti;
                turtleResistMulti = data.turtleResistMulti;
                turtleMultiMulti = data.turtleMultiMulti;
                turtleDurationMulti = data.turtleDurationMulti;
                movementResistMulti = data.movementResistMulti;
                moveSpeedMulti = data.moveSpeedMulti;
                jumpHeightMulti = data.jumpHeightMulti;
                airAttackMulti = data.airAttackMulti;
                instantKillChance = data.instantKillChance;
                betterLootChance = data.betterLootChance;

                hasMagic = data.hasMagic;
                damageResistDuration = data.damageResistDuration;
                smiteDuration = data.smiteDuration;

                skillLevels = data.skillLevels;

                tutorialComplete = data.tutorialComplete;
                lastEnemyLevel = data.lastEnemyLevel;

                totalExperience = data.totalExperience;

                currentIndex = data.currentIndex;
                currentWeapon = data.currentWeapon;
                playerWeaponInventory = data.playerWeaponInventory;
                playerStats = data.playerStats;

                damageProgress = data.damageProgress;
                tankProgress = data.tankProgress;
                mobilityProgress = data.mobilityProgress;
                attackSpeedUpgrades = data.attackSpeedUpgrades;
                potionIsActive = data.potionIsActive;
                activePotionType = data.activePotionType;

                currentLeechMultiplier = data.currentLeechMultiplier;
                potionCoolDownTime = data.potionCoolDownTime;

                currentArmour = data.currentArmour;
                comparingArmour = data.comparingArmour;

                tripleJump = data.tripleJump;
                hasSmite = data.hasSmite;
                gripWalls = data.gripWalls;
                maxedSpeed = data.maxedSpeed;
                damageResist = data.damageResist;


                previousScene = data.previousScene;

                completedQuests = data.completedQuests;
                currentEnemyKills = data.currentEnemyKills;
                #endregion

                StartCoroutine(TurnOnAndOffJustLoaded());

                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadNewScene(data.currentScene);

            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
          
            Destroy(gameObject);
        }
        currentScene = "Autoload";

    }

    IEnumerator TurnOnAndOffJustLoaded()
    {
        justReloaded = true;
        yield return new WaitForSecondsRealtime(2);
        justReloaded = false;
    }

    void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        currentScene = sceneName;
        LootDropMonitor[] lootDropMonitors = FindObjectsOfType<LootDropMonitor>();
        foreach(LootDropMonitor lDM in lootDropMonitors)
        {
            if(lDM.type == 2)
            {
                if(itemInventory[lDM.item] > 0 && !lDM.IsForMap)
                {
                    print(lDM.item + ": " + Instance.itemInventory[lDM.item].ToString());
                    StartCoroutine(WaitThenDestroy(lDM.gameObject, 0.5f));
                }
            }

        }
        Time.timeScale = 1;
    }

    IEnumerator WaitThenDestroy(GameObject gameObject, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Destroy(gameObject);
        print("die");
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Loading" && !justReloaded)
        {
            timeOfAbility -= 30f;
            //Shortened to 3 weapons
            for (int i = 0; i < 3; i++)
            {
                playerWeaponInventory.Add(null);
            }

            TestGiveItem.GiveItem();
            player = FindObjectOfType<PlayerControls>();
            currentWeapon = playerWeaponInventory[0];
            foreach (string element in possibleItems)
            {
                itemInventory.Add(element, 0);
            }


            //LoadDataFromSave();

            //Generates total xp (Cubic graph) based on level
            totalExperience = (float)((0.04 * Math.Pow(playerStats.playerLevel, 3)) + (0.8 * Math.Pow(playerStats.playerLevel, 2)) + 100);
            totalHealthMulti = 0.05f * skillLevels["HealthBonus"];
        }
        StartCoroutine(AutosaveCoroutine());
    }
    void Update()
    {
        if (Time.time > (timeOfAbility + abilityDuration) && (damageResistMulti < 1 || currentAttackMultiplier > 1))
        {
            damageResistMulti = 1;
            currentAttackMultiplier = 1;
            currentActiveAbility = "";
            abilityIsActive = false;
        }

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

    

        //Hides the magic cooldown if player has not unlocked any magic skills
        if (hasMagic == true)
        {
            magicBar.SetActive(true);
        }

        else if ((magicBar != null) && (hasMagic == false))
        {
            magicBar.SetActive(false);
        }



        if (currentWeapon == null)
        {
            ChangeItem(99);
        }

        if(currentScene != SceneManager.GetActiveScene().name && SceneManager.GetActiveScene().name != "You lost")
        {
            OnSceneChange();
        }

        //if (checkExp){}


            //Temporary values
        if (playerStats.playerExperience >= totalExperience)
        {
            //int levels = 0;
            int skillPoints = 0;

            while (playerStats.playerExperience > totalExperience)
            {
                float oldTotalExperience = totalExperience;
                levellingUp = true;
                //levels += 1;
                skillPoints += 1;
                playerStats.playerLevel += 1;
                player.stockHealth = (float)(54f * Math.Pow(playerStats.playerLevel, 2) + 10f);
                CheckSkills();
                totalExperience = (float)((0.04 * Math.Pow(playerStats.playerLevel, 3)) + (0.8 * Math.Pow(playerStats.playerLevel, 2)) + 100);
                playerStats.playerExperience -= oldTotalExperience;
            }


            playerStats.playerSkillPoints += skillPoints;
            //playerStats.playerLevel += levels;
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
   public void SaveGameManagerData(int slot)
    {
        BinaryFormatter bF = new BinaryFormatter();
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/SavedData/slot" + slot + ".txt"))
        {
            file = File.Open(Application.persistentDataPath + "/SavedData/slot" + slot + ".txt", FileMode.Open);
        }
        else
        {
            file = File.Create(Application.persistentDataPath + "/SavedData/slot" + slot + ".txt");
        }
        GameManagerData data = new GameManagerData();

        data.currentScene = currentScene;
        data.previousScene = previousScene;

        data.itemInventory = itemInventory;
        data.possibleItems = possibleItems;
        data.currentDialogueQuest = currentDialogueQuest;
        data.characterQuests = characterQuests;
        data.possibleQuests = possibleQuests;
        data.activeQuests = activeQuests;

        data.attackSpeedMulti = attackSpeedMulti;
        data.attackRangeMulti = attackRangeMulti;
        data.currentAttackMultiplier = currentAttackMultiplier;
        data.smiteDamageMulti = smiteDamageMulti;
        data.smiteDurationMulti = smiteDurationMulti;
        data.lifeStealMulti = lifeStealMulti;
        data.totalHealthMulti = totalHealthMulti;
        data.damageResistMulti = damageResistMulti;
        data.turtleResistMulti = turtleResistMulti;
        data.turtleMultiMulti = turtleMultiMulti;
        data.turtleDurationMulti = turtleDurationMulti;
        data.movementResistMulti = movementResistMulti;
        data.moveSpeedMulti = moveSpeedMulti;
        data.jumpHeightMulti = jumpHeightMulti;
        data.airAttackMulti = airAttackMulti;
        data.instantKillChance = instantKillChance;
        data.betterLootChance = betterLootChance;

        data.hasMagic = hasMagic;
        data.damageResistDuration = damageResistDuration;
        data.smiteDuration = smiteDuration;

        data.skillLevels = skillLevels;

        data.tutorialComplete = tutorialComplete;
        data.lastEnemyLevel = lastEnemyLevel;

        data.totalExperience = totalExperience;

        data.currentIndex = currentIndex;
        data.currentWeapon = currentWeapon;
        data.playerWeaponInventory = playerWeaponInventory;
        data.playerStats = playerStats;

        data.damageProgress = damageProgress;
        data.tankProgress = tankProgress;
        data.mobilityProgress = mobilityProgress;
        data.attackSpeedUpgrades = attackSpeedUpgrades;
        data.potionIsActive = potionIsActive;
        data.activePotionType = activePotionType;

        data.currentLeechMultiplier = currentLeechMultiplier;
        data.potionCoolDownTime = potionCoolDownTime;

        data.currentArmour = currentArmour;
        data.comparingArmour = comparingArmour;

        data.tripleJump = tripleJump;
        data.hasSmite = hasSmite;
        data.gripWalls = gripWalls;
        data.maxedSpeed = maxedSpeed;
        data.damageResist = damageResist;


        data.completedQuests = completedQuests;
        data.currentEnemyKills = currentEnemyKills;

        bF.Serialize(file, data);
        file.Close();
        Debug.Log("Saved");

    }

    public void LoadDataFromSave(int slot)
    {
        SceneManager.LoadScene("Loading");
        print("done");
        GameObject empty = new GameObject("Load Scene Controller");
        LoadSceneMonitor load = empty.AddComponent<LoadSceneMonitor>();

        BinaryFormatter bF = new BinaryFormatter();
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/SavedData/slot" + slot + ".txt"))
        {
            file = File.Open(Application.persistentDataPath + "/SavedData/slot" +slot + ".txt", FileMode.Open);
        }
        else
        {
            file = File.Create(Application.persistentDataPath + "/SavedData/SavedData/slot" + slot + ".txt");
        }

        GameManagerData data = (GameManagerData)bF.Deserialize(file);

        load.data = data;
        new GameObject("PersistantGameManager - Reload").AddComponent<PersistantGameManager>();
        print("done");
        Destroy(gameObject);
    }


    private void OnSceneChange()
    {
        print("OnSceneChange");
        player = FindObjectOfType<PlayerControls>();
        _camera = FindObjectOfType<CameraMovement>().gameObject;
        DoorMonitor[] doors = FindObjectsOfType<DoorMonitor>();
        if(!justReloaded)
        {
            previousScene = currentScene;
        }
        foreach (DoorMonitor door in doors)
        {
            if (door.gameObject.name.Replace(" Door","") == previousScene)
            {
                StartCoroutine(Autosave());
                player.transform.position = door.transform.position;
                _camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y , -10f);
            }
        }
        currentScene = SceneManager.GetActiveScene().name;
    }
    public void Restart(string SceneName)
    {
        Instance = null;
        Destroy(gameObject);
        currentScene = "Autoload";
        SceneManager.LoadScene(SceneName);
    }
    public IEnumerator loadMainCanvas()
    {
        AsyncOperation loadingMainCanvas = SceneManager.LoadSceneAsync("Main Canvas", LoadSceneMode.Additive);
        while(true)
        {
            if(loadingMainCanvas.isDone)
            {
                magicBar = FindObjectOfType<MagicCooldownBar>().gameObject;
                break;
            }
            else
            {
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
    }

    public void CheckSkills()
    {
        checkSkills = false;

        #region Damage
        attackSpeedMulti = 1 - (0.05 * skillLevels["AttackSpeed"]);
        foreach (Weapon weapon in playerWeaponInventory)
        {
            weapon.trueItemSpeed = weapon.stockItemSpeed * (float) attackSpeedMulti;
        }


        if (skillLevels["Smite"] > 0)
        {
            hasMagic = true;
            hasSmite = true;
        }

        smiteDamageMulti = 1 + (0.05f * skillLevels["SmiteDamage"]);

        smiteDurationMulti = 1 + (0.05f * skillLevels["SmiteDuration"]);


        lifeStealMulti = 0.05f * skillLevels["LifeSteal"];

        #endregion

        #region Tank
        float missingHealth = player.totalHealth - player.currentHealth;
        totalHealthMulti = 0.05f * skillLevels["HealthBonus"];
        player.totalHealth = player.stockHealth + (player.stockHealth * totalHealthMulti);
        player.currentHealth = player.totalHealth - missingHealth;


        movementResistMulti = 1 - (0.06f * skillLevels["DefenceWithMovement"]);


        if (skillLevels["Turtle"] > 0)
        {
            hasMagic = true;
            damageResist = true;
        }

        turtleMultiMulti = 1 - (0.05f * skillLevels["TurtleDefense"]);

        turtleDurationMulti = 1 + (0.05f * skillLevels["TurtleDuration"]);
        damageResistDuration = 15 * turtleDurationMulti;

        #endregion

        #region Mobility

        moveSpeedMulti = 1 + (0.05f * skillLevels["MoveSpeed"]);

        airAttackMulti = 1 + (0.05f * skillLevels["AirAttack"]);

        if (skillLevels["TripleJump"] > 0)
        {
            tripleJump = true;
        }

        if (skillLevels["GripWalls"] > 0)
        {
            gripWalls = true;
        }

        jumpHeightMulti = 1 + (0.05f * skillLevels["JumpHeight"]);

        #endregion

        #region Misc

        instantKillChance = 0 + (1f * skillLevels["InstantKill"]);

        betterLootChance = 0 + (1f * skillLevels["WeaponDropValue"]);

        #endregion


    }
    public IEnumerator AutosaveCoroutine()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(120);
            StartCoroutine(Autosave());
        }
    }
    public IEnumerator Autosave()
    {
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
            file.Close();
            Reset();
        }
        Timestamps timestamps = GetTimestamps();
        timestamps.S4T = "Autosave\n " + DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().Length - 8);
        SaveTimestamps(timestamps);
        SaveGameManagerData(4);
        TextMeshProUGUI text = GameObject.FindGameObjectWithTag("Updates").GetComponent<TextMeshProUGUI>();
        for (int i = 0; i <= 5; i++)
        {
            text.text = "Autosaving" + new string('.', i);
            yield return new WaitForSecondsRealtime(0.2f);
        }
        text.text = "";
    }

    public void SaveTimestamps(Timestamps data)
    {
        BinaryFormatter bF = new BinaryFormatter();
        FileStream file;
        file = File.Open(Application.persistentDataPath + "/SavedData/Timestamps.txt", FileMode.Open);
        bF.Serialize(file, data);
        file.Close();
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

    }
}
