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


    [Header("Quests")]

    public Quest currentDialogueQuest;
    public Dictionary<string, int> characterQuests = new Dictionary<string, int>
    {
        {"Jason", 0 }
    };

    public Dictionary<string, Quest> possibleQuests = new Dictionary<string, Quest>();

    public List<string> activeQuests = new List<string>();

    public Dictionary<string, string> rewards = new Dictionary<string, string>
    {
        {"Ja00", "Reward: A 100% attack potion"},
        {"Ja01", "Reward: A 20% Leech potion"},
        {"Ja02", "Ha you don't get anything" },
        {"Ja03", "Reward: A new Longsword"}
    };

    public Dictionary<string, string> questTargets = new Dictionary<string, string>
    {
        {"Hood of Sartuka", "Ja03"}
    };

    public Dictionary<string, int> itemInventory = new Dictionary<string, int>();
    public List<string> possibleItems = new List<string> { "Dagger of Kaliphase", "Amulet of Honour", "Hood of Sartuka", "Claw of Straphagus" };


    [Header("Skill Multipliers")]
    //Skill related multipliers
    public double attackSpeedMulti = 1;
    public float attackRangeMulti = 1;
    public float attackDamageMulti = 1;
    public float lifeStealMulti = 0;
    public float totalHealthMulti = 0;
    public float damageResistMulti = 1;
    public float turtleResistMulti = 0.5f;
    public float movementResistMulti = 1;

    [Header("Menu Statuses")]
    public bool compareScreenOpen;
    public bool characterScreenOpen;
    public bool skillsScreenOpen;
    public bool menuCanvasOpen;
    public Weapon comparingWeapon;
    public bool firstTimeOpeningMenuCanvas, menuCanvasIsOpen;
    public bool dialogueSceneIsOpen;
    public bool bugReportSceneIsOpen;

    [Header("Objects")]
    public PlayerControls player;
    public Camera _camera;
    public GameObject magicBar;


    [Header("Skill Trackers")]

    public bool hasMagic;
    public string currentActiveAbility = "";
    public bool abilityIsActive;
    public float timeOfAbility;
    public float abilityDuration;
    public float damageResistDuration = 15;
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
    public int currentIndex = 0;
    public int previousIndex = 0;

    [Header("Stats")]
    public float totalExperience;
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
    public float currentAttackMultiplier = 1;
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

    public string equippedItemOne, equippedItemTwo;

    private int currentItemOneIndex, currentItemTwoIndex;
    private bool changeItemOne, changeItemTwo;

    public List<string> possibleConsumables = new List<string> { "20%H", "50%H", "100%H", "20%A", "50%A", "100%A", "20%L" };

    public Dictionary<string, int> amountOfConsumables = new Dictionary<string, int>
    {
        {"20%H", 1},
        {"50%H", 0},
        {"100%H", 0},
        {"20%A", 0},
        {"50%A", 0},
        {"100%A", 0},
        {"20%L", 1},
        {"Empty", 0}
    };



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
        StartCoroutine(loadMainCanvas());
        //Shortened to 3 weapons
        for(int i = 0; i <3; i++)
        {
            playerWeaponInventory.Add(null);
        }

        TestGiveItem.GiveItem();
        player = FindObjectOfType<PlayerControls>();
        currentWeapon = playerWeaponInventory[0];
        itemInventory.Add("Empty", 0);
        foreach(string element in possibleItems)
        {
            itemInventory.Add(element, 0);
        }
        foreach(string element in possibleConsumables)
        {
            if(equippedItemOne == "" && amountOfConsumables[element] > 0)
            {
                equippedItemOne = element;
            }
            else if(equippedItemTwo == "" && amountOfConsumables[element] > 0)
            {
                equippedItemTwo = element;
            }
               
        }
        if(equippedItemOne == "")
        {
            equippedItemOne = "Empty";
        }
        if (equippedItemTwo == "")
        {
            equippedItemTwo = "Empty";
        }
        //LoadDataFromSave();

    }
    void Update()
    {

        if (Time.time > (timeOfAbility + abilityDuration) && damageResistMulti < 1)
        {
            damageResistMulti = 1;
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

        /*
        if (Time.time > timeOfAttackMultiplierChange + 30 && activePotionType == "Attack")
        {
            currentAttackMultiplier = 1;
            potionIsActive = false;

        }
        if (Time.time > timeOfLeechMultiplierChange + 30 && activePotionType == "Leech")
        {
            currentLeechMultiplier = 0;
            potionIsActive = false;

        }


        if (Input.GetKeyDown(KeyCode.Z))
        {
            changeItemOne = true;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            changeItemTwo = true;
        }

            //Shortened to 3 items

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

        else if ((magicBar != null) && (hasMagic == false))
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

    private void FixedUpdate()
    {
        if (changeItemOne == true)
        {
            int startingIndex = currentItemOneIndex;
            currentItemOneIndex += 1;
            bool itemFound = false;

            while (itemFound == false)
            {
                if (currentItemOneIndex >= possibleConsumables.Count)
                {
                    currentItemOneIndex = 0;
                }
                if (currentItemOneIndex == startingIndex)
                {
                    if (amountOfConsumables[possibleConsumables[startingIndex]] > 0)
                    {
                        equippedItemOne = possibleConsumables[startingIndex];
                    }
                    else
                    {
                        equippedItemOne = "Empty";
                    }
                    changeItemOne = false;
                    itemFound = true;
                    break;

                }
                if (amountOfConsumables[possibleConsumables[currentItemOneIndex]] > 0)
                {
                    equippedItemOne = possibleConsumables[currentItemOneIndex];
                    changeItemOne = false;
                    itemFound = true;
                }
                else
                {
                    currentItemOneIndex += 1;
                }

            }

        }

        if (changeItemTwo == true)
        {
            int startingIndex = currentItemTwoIndex;
            currentItemTwoIndex += 1;
            bool itemFound = false;

            while (itemFound == false)
            {
                if (currentItemTwoIndex >= possibleConsumables.Count)
                {
                    currentItemTwoIndex = 0;
                }
                if (currentItemTwoIndex == startingIndex)
                {
                    if(amountOfConsumables[possibleConsumables[startingIndex]] > 0)
                    {
                        equippedItemTwo = possibleConsumables[startingIndex];
                    }
                    else
                    {
                        equippedItemTwo = "Empty";
                    }

                    changeItemTwo = false;
                    itemFound = true;
                    break;
                }
                if (amountOfConsumables[possibleConsumables[currentItemTwoIndex]] > 0)
                {
                    equippedItemTwo = possibleConsumables[currentItemTwoIndex];
                    changeItemTwo = false;
                    itemFound = true;
                }
                else
                {
                    currentItemTwoIndex += 1;
                }

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
        _camera = FindObjectOfType<Camera>();
        DoorMonitor[] doors = FindObjectsOfType<DoorMonitor>();
        foreach (DoorMonitor door in doors)
        {
            if (door.gameObject.name.Replace(" Door","") == currentScene)
            {
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
        SceneManager.LoadScene(SceneName);
    }
    private IEnumerator loadMainCanvas()
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

        #region AttackSpeed

        attackSpeedMulti = 1 - (0.05 * skillLevels["AttackSpeed"]);
        foreach (Weapon weapon in playerWeaponInventory)
        {
            weapon.trueItemSpeed = weapon.stockItemSpeed* (float) attackSpeedMulti;
        }

        #endregion

        #region Smite

        if (skillLevels["Smite"] > 0)
        {
            hasMagic = true;
            hasSmite = true;
        }

        #endregion

        #region LifeSteal

        lifeStealMulti = 0.05f * skillLevels["LifeSteal"];

        #endregion

        #region HealthBonus

        float missingHealth = player.totalHealth - player.currentHealth;
        totalHealthMulti = 0.05f * skillLevels["HealthBonus"];
        player.totalHealth = player.stockHealth + (player.stockHealth * totalHealthMulti);
        player.currentHealth = player.totalHealth - missingHealth;

        #endregion

        #region MoveDefence

        movementResistMulti = 1 - (0.06f * skillLevels["DefenceWithMovement"]);

        #endregion

        #region Turtle

        if (skillLevels["Turtle"] > 0)
        {
            hasMagic = true;
            damageResist = true;
        }

        #endregion

        #region TripleJump

        if (skillLevels["TripleJump"] > 0)
        {
            tripleJump = true;
        }

        #endregion

        #region GripWalls

        if (skillLevels["GripWalls"] > 0)
        {
            gripWalls = true;
        }

        #endregion



    }
}
