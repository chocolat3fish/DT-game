using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;
public class PersistantGameManager : MonoBehaviour
{
    public static PersistantGameManager Instance { get; private set; }

    public int currentIndex = 0;
    public int previousIndex = 0;
    public Weapon currentWeapon;
    public List<Weapon> playerWeaponInventory = new List<Weapon>();
    public bool compareScreenOpen = false;
    public Weapon comparingWeapon;
    public Armour currentArmour;
    public Armour comparingArmour;
    public List<Armour> playerArmourInventory = new List<Armour>();
    public PlayerControls player;

    public PlayerStats playerStats;
    public bool checkExp;

    public string currentScene;

    public float totalExperience;



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

        if(currentWeapon.itemName == "")
        {
            ChangeItem(99);
        }

        if(currentScene != SceneManager.GetActiveScene().ToString())
        {
            player = FindObjectOfType<PlayerControls>();
            currentScene = SceneManager.GetActiveScene().name;
        }

        if (checkExp)
        {

            //Temporary values
            if (playerStats.playerExperience >= totalExperience)
            {
                int levels = 0;

                while (playerStats.playerExperience > totalExperience)
                {
                    levels += 1;
                    playerStats.playerExperience -= 300; 
                }


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
   

}
