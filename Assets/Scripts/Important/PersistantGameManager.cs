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
    public List<Weapon> playerIventory = new List<Weapon>();
    public PlayerStats playerStats;
    public bool compareScreenOpen = false;
    public Weapon comparingWeapon;
    public PlayerControls player;

    public string currentScene;

    private IEnumerator CasualPlayerWeaponChecker()
    {
        while (true)
        {
            if(player.playerDamage != currentWeapon.itemDamage)
            {
                PlayerMonitor.UpdateWeapon();
            }
            yield return new WaitForSeconds(1);
        }
    }

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
        for(int i = 0; i <5; i++)
        {
            playerIventory.Add(null);
        }
        TestGiveItem.GiveItem();
        player = FindObjectOfType<PlayerControls>();
        StartCoroutine(CasualPlayerWeaponChecker());
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentIndex = 0;
            changeItem(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentIndex = 1;
            changeItem(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentIndex = 2;
            changeItem(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentIndex = 3;
            changeItem(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentIndex = 4;
            changeItem(currentIndex);
        }
        if(currentWeapon.itemName == "")
        {
            changeItem(99);
        }

        if(currentScene != SceneManager.GetActiveScene().ToString())
        {
            player = FindObjectOfType<PlayerControls>();
            StartCoroutine(CasualPlayerWeaponChecker());
            currentScene = SceneManager.GetActiveScene().name;
        }

    }
    public void changeItem(int index)
    {   
        if(index == 99)
        {
            currentWeapon = playerIventory[0];
            previousIndex = 0;
            PlayerMonitor.UpdateWeapon();
        }
        else if(index != previousIndex && index < 5 && index > -1)
        {
            
            if(playerIventory[index] != null )
            {
                currentWeapon = playerIventory[index];
                previousIndex = currentIndex;
                PlayerMonitor.UpdateWeapon();
            }
            
        }
    }
   

}
