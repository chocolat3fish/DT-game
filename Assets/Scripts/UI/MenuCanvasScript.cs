using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvasScript : MonoBehaviour
{
    public bool isActive;
    public bool freeze;
    public GameObject mainPanel;
    public GameObject weaponsPanel;
    public GameObject consumablesPanel;
    public GameObject itemsPanel;
    public GameObject questsPanel, questDescPanel;
    

    private bool openWeapons, closeWeapons, openConsumables, closeConsumables, openItems, closeItems, openQuests, closeQuests, closeSlot;

    private Text sW1Output, sW2Output, sW3Output;
    private Text wD1Output, wD2Output, wD3Output;
    private Text wS1Output, wS2Output, wS3Output;
    private Text wR1Output, wR2Output, wR3Output;
    private Text sL1Output, sL2Output, sL3Output;

    private Text cSThreeName, cSTwoName, cSOneName, cSSixName, cSFiveName, cSFourName, cSSevenName, cSEightName, cSNineName;
    private Text cSOneOutput, cSTwoOutput, cSThreeOutput, cSFourOutput, cSFiveOutput, cSSixOutput, cSSevenOutput, cSEightOutput, cSNineOutput;
    private Text iSThreeName, iSTwoName, iSOneName, iSSixName, iSFiveName, iSFourName, iSSevenName, iSEightName, iSNineName;
    private Text iSOneOutput, iSTwoOutput, iSThreeOutput, iSFourOutput, iSFiveOutput, iSSixOutput, iSSevenOutput, iSEightOutput, iSNineOutput;
    private Text qSThreeName, qSTwoName, qSOneName, qSSixName, qSFiveName, qSFourName, qSSevenName, qSEightName, qSNineName;
    private Text qSOneOutput, qSTwoOutput, qSThreeOutput, qSFourOutput, qSFiveOutput, qSSixOutput, qSSevenOutput, qSEightOutput, qSNineOutput;
    private Text questName, questDescription, questReward;

    private string cSlot1, cSlot2, cSlot3, cSlot4, cSlot5, cSlot6, cSlot7, cSlot8, cSlot9;
    private string iSlot1, iSlot2, iSlot3, iSlot4, iSlot5, iSlot6, iSlot7, iSlot8, iSlot9;
    private string qSlot1, qSlot2, qSlot3, qSlot4, qSlot5, qSlot6, qSlot7, qSlot8, qSlot9;

    public int index;

    void Awake()
    {
    
        mainPanel = gameObject.transform.Find("MainPanel").gameObject;
        weaponsPanel = gameObject.transform.Find("WeaponsPanel").gameObject;
        consumablesPanel = gameObject.transform.Find("ConsumablesPanel").gameObject;
        itemsPanel = gameObject.transform.Find("ItemsPanel").gameObject;
        questsPanel = gameObject.transform.Find("QuestsPanel").gameObject;
        questDescPanel = gameObject.transform.Find("QuestDescPanel").gameObject;

        sW1Output = weaponsPanel.transform.Find("SW1").gameObject.GetComponent<Text>();
        sW2Output = weaponsPanel.transform.Find("SW2").gameObject.GetComponent<Text>();
        sW3Output = weaponsPanel.transform.Find("SW3").gameObject.GetComponent<Text>();
        wD1Output = weaponsPanel.transform.Find("WD1").gameObject.GetComponent<Text>();
        wD2Output = weaponsPanel.transform.Find("WD2").gameObject.GetComponent<Text>();
        wD3Output = weaponsPanel.transform.Find("WD3").gameObject.GetComponent<Text>();
        wS1Output = weaponsPanel.transform.Find("WS1").gameObject.GetComponent<Text>();
        wS2Output = weaponsPanel.transform.Find("WS2").gameObject.GetComponent<Text>();
        wS3Output = weaponsPanel.transform.Find("WS3").gameObject.GetComponent<Text>();
        wR1Output = weaponsPanel.transform.Find("WR1").gameObject.GetComponent<Text>();
        wR2Output = weaponsPanel.transform.Find("WR2").gameObject.GetComponent<Text>();
        wR3Output = weaponsPanel.transform.Find("WR3").gameObject.GetComponent<Text>();
        sL1Output = weaponsPanel.transform.Find("SL1").gameObject.GetComponent<Text>();
        sL2Output = weaponsPanel.transform.Find("SL2").gameObject.GetComponent<Text>();
        sL3Output = weaponsPanel.transform.Find("SL3").gameObject.GetComponent<Text>();


        cSOneName = consumablesPanel.transform.Find("Slot 1").gameObject.GetComponent<Text>();
        cSTwoName = consumablesPanel.transform.Find("Slot 2").gameObject.GetComponent<Text>();
        cSThreeName = consumablesPanel.transform.Find("Slot 3").gameObject.GetComponent<Text>();

        cSFourName = consumablesPanel.transform.Find("Slot 4").gameObject.GetComponent<Text>();
        cSFiveName = consumablesPanel.transform.Find("Slot 5").gameObject.GetComponent<Text>();
        cSSixName = consumablesPanel.transform.Find("Slot 6").gameObject.GetComponent<Text>();

        cSSevenName = consumablesPanel.transform.Find("Slot 7").gameObject.GetComponent<Text>();
        cSEightName = consumablesPanel.transform.Find("Slot 8").gameObject.GetComponent<Text>();
        cSNineName = consumablesPanel.transform.Find("Slot 9").gameObject.GetComponent<Text>();

        cSOneOutput = consumablesPanel.transform.Find("Q1").gameObject.GetComponent<Text>();
        cSTwoOutput = consumablesPanel.transform.Find("Q2").gameObject.GetComponent<Text>();
        cSThreeOutput = consumablesPanel.transform.Find("Q3").gameObject.GetComponent<Text>();

        cSFourOutput = consumablesPanel.transform.Find("Q4").gameObject.GetComponent<Text>();
        cSFiveOutput = consumablesPanel.transform.Find("Q5").gameObject.GetComponent<Text>();
        cSSixOutput = consumablesPanel.transform.Find("Q6").gameObject.GetComponent<Text>();

        cSSevenOutput = consumablesPanel.transform.Find("Q7").gameObject.GetComponent<Text>();
        cSEightOutput = consumablesPanel.transform.Find("Q8").gameObject.GetComponent<Text>();
        cSNineOutput = consumablesPanel.transform.Find("Q9").gameObject.GetComponent<Text>();

        iSOneName = itemsPanel.transform.Find("Slot 1").gameObject.GetComponent<Text>();
        iSTwoName = itemsPanel.transform.Find("Slot 2").gameObject.GetComponent<Text>();
        iSThreeName = itemsPanel.transform.Find("Slot 3").gameObject.GetComponent<Text>();

        iSFourName = itemsPanel.transform.Find("Slot 4").gameObject.GetComponent<Text>();
        iSFiveName = itemsPanel.transform.Find("Slot 5").gameObject.GetComponent<Text>();
        iSSixName = itemsPanel.transform.Find("Slot 6").gameObject.GetComponent<Text>();

        iSSevenName = itemsPanel.transform.Find("Slot 7").gameObject.GetComponent<Text>();
        iSEightName = itemsPanel.transform.Find("Slot 8").gameObject.GetComponent<Text>();
        iSNineName = itemsPanel.transform.Find("Slot 9").gameObject.GetComponent<Text>();

        iSOneOutput = itemsPanel.transform.Find("Q1").gameObject.GetComponent<Text>();
        iSTwoOutput = itemsPanel.transform.Find("Q2").gameObject.GetComponent<Text>();
        iSThreeOutput = itemsPanel.transform.Find("Q3").gameObject.GetComponent<Text>();

        iSFourOutput = itemsPanel.transform.Find("Q4").gameObject.GetComponent<Text>();
        iSFiveOutput = itemsPanel.transform.Find("Q5").gameObject.GetComponent<Text>();
        iSSixOutput = itemsPanel.transform.Find("Q6").gameObject.GetComponent<Text>();

        iSSevenOutput = itemsPanel.transform.Find("Q7").gameObject.GetComponent<Text>();
        iSEightOutput = itemsPanel.transform.Find("Q8").gameObject.GetComponent<Text>();
        iSNineOutput = itemsPanel.transform.Find("Q9").gameObject.GetComponent<Text>();

        qSOneName = questsPanel.transform.Find("Slot 1").transform.Find("Text").GetComponent<Text>();
        qSTwoName = questsPanel.transform.Find("Slot 2").transform.Find("Text").GetComponent<Text>();
        qSThreeName = questsPanel.transform.Find("Slot 3").transform.Find("Text").GetComponent<Text>();

        qSFourName = questsPanel.transform.Find("Slot 4").transform.Find("Text").GetComponent<Text>();
        qSFiveName = questsPanel.transform.Find("Slot 5").transform.Find("Text").GetComponent<Text>();
        qSSixName = questsPanel.transform.Find("Slot 6").transform.Find("Text").GetComponent<Text>();

        qSSevenName = questsPanel.transform.Find("Slot 7").transform.Find("Text").GetComponent<Text>();
        qSEightName = questsPanel.transform.Find("Slot 8").transform.Find("Text").GetComponent<Text>();
        qSNineName = questsPanel.transform.Find("Slot 9").transform.Find("Text").GetComponent<Text>();

        questName = questDescPanel.transform.Find("QN").GetComponent<Text>();
        questDescription = questDescPanel.transform.Find("QD").GetComponent<Text>();
        questReward = questDescPanel.transform.Find("QR").GetComponent<Text>();


    }

    void Start()
    {
        mainPanel.SetActive(false);
        weaponsPanel.SetActive(false);
        consumablesPanel.SetActive(false);
        itemsPanel.SetActive(false);
        questsPanel.SetActive(false);
        questDescPanel.SetActive(false);
        openWeapons = false;
        closeWeapons = false;
        openItems = false;
        closeItems = false;
        openConsumables = false;
        closeConsumables = false;
        openQuests = false;
        closeQuests = false;


        

    }

    void Update()
    {
        if(PersistantGameManager.Instance.firstTimeOpeningMenuCanvas)
        {
            mainPanel.SetActive(true);
            isActive = true;
            PersistantGameManager.Instance.menuCanvasOpen = true;
            Time.timeScale = 0;

            UpdateData();
            PersistantGameManager.Instance.firstTimeOpeningMenuCanvas = false;
        }
        /*
        if (Input.GetKeyDown(KeyCode.Tab) && isActive == false && PersistantGameManager.Instance.characterScreenOpen == false && PersistantGameManager.Instance.compareScreenOpen == false)
        {
            mainPanel.SetActive(true);
            isActive = true;
            PersistantGameManager.Instance.menuScreenOpen = true;
            Time.timeScale = 0;

            UpdateData();


        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isActive == true)
        {
            mainPanel.SetActive(false);
            weaponsPanel.SetActive(false);
            consumablesPanel.SetActive(false);
            itemsPanel.SetActive(false);
            questsPanel.SetActive(false);
            questDescPanel.SetActive(false);
            isActive = false;
            freeze = false;
            PersistantGameManager.Instance.menuScreenOpen = false;
            Time.timeScale = 1;
        }

        */
        if (mainPanel.activeSelf && Input.GetKeyDown(KeyCode.L) && weaponsPanel.activeSelf == true)
        {

            closeWeapons = true;
            openWeapons = false;

        }
        else if (mainPanel.activeSelf && Input.GetKeyDown(KeyCode.L) && weaponsPanel.activeSelf == false)
        {
            openWeapons = true;
            closeWeapons = false;
        }

        if (mainPanel.activeSelf && Input.GetKeyDown(KeyCode.K) && itemsPanel.activeSelf == true)
        {
            closeItems = true;
            openItems = false;

        }
        else if (mainPanel.activeSelf && Input.GetKeyDown(KeyCode.K) && itemsPanel.activeSelf == false)
        {
            openItems = true;
            closeItems = false;
        }


        if (mainPanel.gameObject.activeSelf && Input.GetKeyDown(KeyCode.J) && consumablesPanel.activeSelf == true)
        {
            closeConsumables = true;
            openConsumables = false;
        }
        else if (mainPanel.gameObject.activeSelf && Input.GetKeyDown(KeyCode.J) && consumablesPanel.activeSelf == false)
        {
            openConsumables = true;
            closeConsumables = false;
        }

        if (mainPanel.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Semicolon) && questsPanel.activeSelf == true)
        {
            closeQuests = true;
            openQuests = false;
        }
        else if (mainPanel.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Semicolon) && questsPanel.activeSelf == false && questDescPanel.activeSelf == false)
        {
            openQuests = true;
            closeQuests = false;
        }
        else if (mainPanel.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Semicolon) && questDescPanel.activeSelf == true)
        {
            closeSlot = true;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            int startIndex = index;
            while (true)
            {
                index -= 1;
                if (consumablesPanel.activeSelf)
                {
                    if (index < 0)
                    {
                        index = startIndex;
                        break;
                    }
                    if (PersistantGameManager.Instance.amountOfConsumables[PersistantGameManager.Instance.possibleConsumables[index]] > 0)
                    {
                        break;
                    }
                }
                else if (itemsPanel.activeSelf)
                {
                    if (index < 0)
                    {
                        index = startIndex;
                        break;
                    }
                    if (PersistantGameManager.Instance.itemInventory[PersistantGameManager.Instance.possibleItems[index]] > 0)
                    {
                        break;
                    }
                }
                else
                {
                    index = 0;
                    break;
                }
            }


            UpdateData();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            int startIndex = index;
            while(true)
            {
                index += 1;
                if(consumablesPanel.activeSelf)
                {
                    if (index > PersistantGameManager.Instance.possibleConsumables.Count - 1)
                    {
                        index = startIndex;
                        break;
                    }
                    if (PersistantGameManager.Instance.amountOfConsumables[PersistantGameManager.Instance.possibleConsumables[index]] > 0)
                    {
                        break;
                    }
                }
                else if( itemsPanel.activeSelf)
                {
                    if (index > PersistantGameManager.Instance.possibleItems.Count - 1)
                    {
                        index = startIndex;
                        break;
                    }
                    if (PersistantGameManager.Instance.itemInventory[PersistantGameManager.Instance.possibleItems[index]] > 0)
                    {
                        break;
                    }
                }
                else
                {
                    index = 0;
                    break;
                }
                
            }


            UpdateData();
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            index = 0;
            UpdateData();
        }

    }

    private void LateUpdate()
    {
        if (openWeapons == true)
        {
            OpenWeaponsMenu();
            openWeapons = false;
        }
        else if (closeWeapons == true)
        {
            CloseWeaponsMenu();
            closeWeapons = false;
        }

        if (openConsumables == true)
        {
            OpenConsumablesMenu();
            openConsumables = false;
        }
        else if (closeConsumables == true)
        {
            CloseConsumablesMenu();
            closeConsumables = false;
        }
        if (openItems == true)
        {
            OpenItemsMenu();
            openItems = false;
        }
        else if (closeItems == true)
        {
            CloseItemsMenu();
            closeItems = false;
        }
        if (openQuests == true)
        {
            OpenQuestsMenu();
            openQuests = false;
        }
        else if (closeQuests == true)
        {
            CloseQuestsMenu();
            closeQuests = false;
        }
        else if (closeSlot == true)
        {
            CloseSlot();
            closeSlot = false;
        }

    }
    public void OpenItemsMenu()
    {
        
        index = 0;
        UpdateData();
        itemsPanel.SetActive(true);
        consumablesPanel.SetActive(false);
        
    }

    public void CloseItemsMenu()
    {
        itemsPanel.SetActive(false);
    }

    public void OpenWeaponsMenu()
    {
        UpdateData();
        weaponsPanel.SetActive(true);
        questsPanel.SetActive(false);
        
    }

    public void CloseWeaponsMenu()
    {
        weaponsPanel.SetActive(false);

    }

    public void OpenConsumablesMenu()
    {
       
        index = 0;
        UpdateData();
        consumablesPanel.SetActive(true);
        itemsPanel.SetActive(false);
    }

    public void CloseConsumablesMenu()
    {

        consumablesPanel.SetActive(false);

    }

    public void OpenQuestsMenu()
    {
        UpdateData();
        questsPanel.SetActive(true);
        weaponsPanel.SetActive(false);
    }

    public void CloseQuestsMenu()
    {
        questDescPanel.SetActive(false);
        questsPanel.SetActive(false);

    }


    private void UpdateData()
    {
        

        sW1Output.text = PersistantGameManager.Instance.playerWeaponInventory[0].itemName;
        sW2Output.text = PersistantGameManager.Instance.playerWeaponInventory[1].itemName;
        sW3Output.text = PersistantGameManager.Instance.playerWeaponInventory[2].itemName;

        if(PersistantGameManager.Instance.currentWeapon.itemName == sW1Output.text)
        {
            sW1Output.fontStyle = FontStyle.Bold;
            sW2Output.fontStyle = FontStyle.Normal;
            sW3Output.fontStyle = FontStyle.Normal;
        }
        else if(PersistantGameManager.Instance.currentWeapon.itemName == sW2Output.text)
        {
            sW1Output.fontStyle = FontStyle.Normal;
            sW2Output.fontStyle = FontStyle.Bold;
            sW3Output.fontStyle = FontStyle.Normal;
        }
        else if (PersistantGameManager.Instance.currentWeapon.itemName == sW3Output.text)
        {
            sW1Output.fontStyle = FontStyle.Normal;
            sW2Output.fontStyle = FontStyle.Normal;
            sW3Output.fontStyle = FontStyle.Bold;
        }

        double itemOneSpeed = Math.Round(PersistantGameManager.Instance.playerWeaponInventory[0].trueItemSpeed, 2);
        double itemTwoSpeed = Math.Round(PersistantGameManager.Instance.playerWeaponInventory[1].trueItemSpeed, 2);
        double itemThreeSpeed = Math.Round(PersistantGameManager.Instance.playerWeaponInventory[2].trueItemSpeed, 2);

        wD1Output.text = PersistantGameManager.Instance.playerWeaponInventory[0].itemDamage.ToString();
        wD2Output.text = PersistantGameManager.Instance.playerWeaponInventory[1].itemDamage.ToString();
        wD3Output.text = PersistantGameManager.Instance.playerWeaponInventory[2].itemDamage.ToString();

        wS1Output.text = itemOneSpeed.ToString();
        wS2Output.text = itemTwoSpeed.ToString();
        wS3Output.text = itemThreeSpeed.ToString();

        wR1Output.text = PersistantGameManager.Instance.playerWeaponInventory[0].itemRange.ToString();
        wR2Output.text = PersistantGameManager.Instance.playerWeaponInventory[1].itemRange.ToString();
        wR3Output.text = PersistantGameManager.Instance.playerWeaponInventory[2].itemRange.ToString();

        sL1Output.text = PersistantGameManager.Instance.playerWeaponInventory[0].itemLevel.ToString();
        sL2Output.text = PersistantGameManager.Instance.playerWeaponInventory[1].itemLevel.ToString();
        sL3Output.text = PersistantGameManager.Instance.playerWeaponInventory[2].itemLevel.ToString();

        #region ConsumablesMenu

        cSlot1 = "";
        cSlot2 = "";
        cSlot3 = "";
        cSlot4 = "";
        cSlot5 = "";
        cSlot6 = "";
        cSlot7 = "";
        cSlot8 = "";
        cSlot9 = "";

        cSOneOutput.text = "";
        cSTwoOutput.text = "";
        cSThreeOutput.text = "";
        cSFourOutput.text = "";
        cSFiveOutput.text = "";
        cSSixOutput.text = "";
        cSSevenOutput.text = "";
        cSEightOutput.text = "";
        cSNineOutput.text = "";

       
        // for (int element = 1; element < PersistantGameManager.Instance.possibleItems.Count; element++)
        foreach (string element in PersistantGameManager.Instance.possibleConsumables)
        {
            if(index > PersistantGameManager.Instance.possibleConsumables.IndexOf(element))
            {
                continue;
            }

            if (cSlot1 == "")
            {
                if (PersistantGameManager.Instance.amountOfConsumables[element] > 0)
                {
                    cSlot1 = element;
                }
            }
            else if(cSlot2 == "")
            {
                if (PersistantGameManager.Instance.amountOfConsumables[element] > 0)
                {
                    cSlot2 = element;
                }
            }
            else if (cSlot3 == "")
            {
                if (PersistantGameManager.Instance.amountOfConsumables[element] > 0)
                {
                    cSlot3 = element;
                }
            }
            else if (cSlot4 == "")
            {
                if (PersistantGameManager.Instance.amountOfConsumables[element] > 0)
                {
                    cSlot4 = element;
                }
            }
            else if (cSlot5 == "")
            {
                if (PersistantGameManager.Instance.amountOfConsumables[element] > 0)
                {
                    cSlot5 = element;
                }
            }
            else if (cSlot6 == "")
            {
                if(PersistantGameManager.Instance.amountOfConsumables[element] > 0)
                {
                    cSlot6 = element;
                }

            }
            else if (cSlot7 == "")
            {
                if (PersistantGameManager.Instance.amountOfConsumables[element] > 0)
                {
                    cSlot7 = element;
                }

            }
            else if (cSlot8 == "")
            {
                if (PersistantGameManager.Instance.amountOfConsumables[element] > 0)
                {
                    cSlot8 = element;
                }

            }
            else if (cSlot9 == "")
            {
                if (PersistantGameManager.Instance.amountOfConsumables[element] > 0)
                {
                    cSlot9 = element;
                }

            }

        }



        cSOneName.text = cSlot1;
        cSTwoName.text = cSlot2;
        cSThreeName.text = cSlot3;

        cSFourName.text = cSlot4;
        cSFiveName.text = cSlot5;
        cSSixName.text = cSlot6;

        cSSevenName.text = cSlot7;
        cSEightName.text = cSlot8;
        cSNineName.text = cSlot9;


        if (cSlot1 != "")
        {
            cSOneOutput.text = PersistantGameManager.Instance.amountOfConsumables[cSlot1].ToString();
        }
        else
        {
            cSOneName.text = "No Items";
            cSOneOutput.text = "";
        }

        if (cSlot2 != "")
        {
            cSTwoOutput.text = PersistantGameManager.Instance.amountOfConsumables[cSlot2].ToString();
        }
        else { cSTwoOutput.text = ""; }

        if (cSlot3 != "")
        {
           cSThreeOutput.text = PersistantGameManager.Instance.amountOfConsumables[cSlot3].ToString();
        }
        else { cSThreeOutput.text = ""; }

        if (cSlot4 != "")
        {
            cSFourOutput.text = PersistantGameManager.Instance.amountOfConsumables[cSlot4].ToString();
        }
        else { cSFourOutput.text = ""; }

        if (cSlot5 != "")
        {
            cSFiveOutput.text = PersistantGameManager.Instance.amountOfConsumables[cSlot5].ToString();
        }
        else { cSFiveOutput.text = ""; }

        if (cSlot6 != "")
        {
            cSSixOutput.text = PersistantGameManager.Instance.amountOfConsumables[cSlot6].ToString();
        }
        else { cSSixOutput.text = ""; }

        if (cSlot7 != "")
        {
            cSSevenOutput.text = PersistantGameManager.Instance.amountOfConsumables[cSlot7].ToString();
        }
        else { cSSevenOutput.text = ""; }

        if (cSlot8 != "")
        {
            cSEightOutput.text = PersistantGameManager.Instance.amountOfConsumables[cSlot8].ToString();
        }
        else { cSEightOutput.text = ""; }

        if (cSlot9 != "")
        {
            cSNineOutput.text = PersistantGameManager.Instance.amountOfConsumables[cSlot9].ToString();
        }
        else { cSNineOutput.text = ""; }

        #endregion


        #region ItemsMenu
        iSlot1 = "";
        iSlot2 = "";
        iSlot3 = "";
        iSlot4 = "";
        iSlot5 = "";
        iSlot6 = "";
        iSlot7 = "";
        iSlot8 = "";
        iSlot9 = "";

        iSOneOutput.text = "";
        iSTwoOutput.text = "";
        iSThreeOutput.text = "";
        iSFourOutput.text = "";
        iSFiveOutput.text = "";
        iSSixOutput.text = "";
        iSSevenOutput.text = "";
        iSEightOutput.text = "";
        iSNineOutput.text = "";



        foreach (string element in PersistantGameManager.Instance.possibleItems)
        {
            if (index > PersistantGameManager.Instance.possibleItems.IndexOf(element))
            {
                continue;
            }

            if (iSlot1 == "")
            {
                if (PersistantGameManager.Instance.itemInventory[element] > 0)
                {
                    iSlot1 = element;
                }
            }
            else if (iSlot2 == "")
            {
                if (PersistantGameManager.Instance.itemInventory[element] > 0)
                {
                    iSlot2 = element;
                }
            }
            else if (iSlot3 == "")
            {
                if (PersistantGameManager.Instance.itemInventory[element] > 0)
                {
                    iSlot3 = element;
                }
            }
            else if (iSlot4 == "")
            {
                if (PersistantGameManager.Instance.itemInventory[element] > 0)
                {
                    iSlot4 = element;
                }
            }
            else if (iSlot5 == "")
            {
                if (PersistantGameManager.Instance.itemInventory[element] > 0)
                {
                    iSlot5 = element;
                }
            }
            else if (iSlot6 == "")
            {
                if (PersistantGameManager.Instance.itemInventory[element] > 0)
                {
                    iSlot6 = element;
                }

            }
            else if (iSlot7 == "")
            {
                if (PersistantGameManager.Instance.itemInventory[element] > 0)
                {
                    iSlot7 = element;
                }

            }
            else if (iSlot8 == "")
            {
                if (PersistantGameManager.Instance.itemInventory[element] > 0)
                {
                    iSlot8 = element;
                }

            }
            else if (iSlot9 == "")
            {
                if (PersistantGameManager.Instance.itemInventory[element] > 0)
                {
                    iSlot9 = element;
                }

            }

        }



        iSOneName.text = iSlot1;
        iSTwoName.text = iSlot2;
        iSThreeName.text = iSlot3;

        iSFourName.text = iSlot4;
        iSFiveName.text = iSlot5;
        iSSixName.text = iSlot6;

        iSSevenName.text = iSlot7;
        iSEightName.text = iSlot8;
        iSNineName.text = iSlot9;


        if (iSlot1 != "")
        {
            iSOneOutput.text = PersistantGameManager.Instance.itemInventory[iSlot1].ToString();
        }
        else
        {
            iSOneName.text = "No Items";
            iSOneOutput.text = "";
        }

        if (iSlot2 != "")
        {
            iSTwoOutput.text = PersistantGameManager.Instance.itemInventory[iSlot2].ToString();
        }
        else { iSTwoOutput.text = ""; }

        if (iSlot3 != "")
        {
            iSThreeOutput.text = PersistantGameManager.Instance.itemInventory[iSlot3].ToString();
        }
        else { iSThreeOutput.text = ""; }

        if (iSlot4 != "")
        {
            iSFourOutput.text = PersistantGameManager.Instance.itemInventory[iSlot4].ToString();
        }
        else { iSFourOutput.text = ""; }

        if (iSlot5 != "")
        {
            iSFiveOutput.text = PersistantGameManager.Instance.itemInventory[iSlot5].ToString();
        }
        else { iSFiveOutput.text = ""; }

        if (iSlot6 != "")
        {
            iSSixOutput.text = PersistantGameManager.Instance.itemInventory[iSlot6].ToString();
        }
        else { iSSixOutput.text = ""; }

        if (iSlot7 != "")
        {
            iSSevenOutput.text = PersistantGameManager.Instance.itemInventory[iSlot7].ToString();
        }
        else { iSSevenOutput.text = ""; }

        if (iSlot8 != "")
        {
            iSEightOutput.text = PersistantGameManager.Instance.itemInventory[iSlot8].ToString();
        }
        else { iSEightOutput.text = ""; }

        if (iSlot9 != "")
        {
            iSNineOutput.text = PersistantGameManager.Instance.itemInventory[iSlot9].ToString();
        }
        else { iSNineOutput.text = ""; }




        #endregion

        #region QuestsMenu
        qSlot1 = "";
        qSlot2 = "";
        qSlot3 = "";
        qSlot4 = "";
        qSlot5 = "";
        qSlot6 = "";
        qSlot7 = "";
        qSlot8 = "";
        qSlot9 = "";

        /*
        qSOneOutput.text = "";
        qSTwoOutput.text = "";
        qSThreeOutput.text = "";
        qSFourOutput.text = "";
        qSFiveOutput.text = "";
        qSSixOutput.text = "";
        qSSevenOutput.text = "";
        qSEightOutput.text = "";
        qSNineOutput.text = "";
        */      



        foreach (string element in PersistantGameManager.Instance.activeQuests)
        {
            if (index > PersistantGameManager.Instance.activeQuests.IndexOf(element))
            {
                continue;
            }

            if (qSlot1 == "")
            {
                if (PersistantGameManager.Instance.possibleQuests[element] != null)
                {
                    qSlot1 = element;
                }
            }
            else if (qSlot2 == "")
            {
                if (PersistantGameManager.Instance.possibleQuests[element] != null)
                {
                    qSlot2 = element;
                }
            }
            else if (qSlot3 == "")
            {
                if (PersistantGameManager.Instance.possibleQuests[element] != null)
                {
                    qSlot3 = element;
                }
            }
            else if (qSlot4 == "")
            {
                if (PersistantGameManager.Instance.possibleQuests[element] != null)
                {
                    qSlot4 = element;
                }
            }
            else if (qSlot5 == "")
            {
                if (PersistantGameManager.Instance.possibleQuests[element] != null)
                {
                    qSlot5 = element;
                }
            }
            else if (qSlot6 == "")
            {
                if (PersistantGameManager.Instance.possibleQuests[element] != null)
                {
                    qSlot6 = element;
                }

            }
            else if (qSlot7 == "")
            {
                if (PersistantGameManager.Instance.possibleQuests[element] != null)
                {
                    qSlot7 = element;
                }

            }
            else if (iSlot8 == "")
            {
                if (PersistantGameManager.Instance.possibleQuests[element] != null)
                {
                    qSlot8 = element;
                }

            }
            else if (qSlot9 == "")
            {
                if (PersistantGameManager.Instance.possibleQuests[element] != null)
                {
                    qSlot9 = element;
                }

            }

        }



        qSOneName.text = qSlot1;
        qSTwoName.text = qSlot2;
        qSThreeName.text = qSlot3;

        qSFourName.text = qSlot4;
        qSFiveName.text = qSlot5;
        qSSixName.text = qSlot6;

        qSSevenName.text = qSlot7;
        qSEightName.text = qSlot8;
        qSNineName.text = qSlot9;


        if (qSlot1 != "")
        {
            qSOneName.transform.parent.gameObject.SetActive(true);
            qSOneName.text = PersistantGameManager.Instance.possibleQuests[qSlot1].questName;
        }
        else
        {

            qSOneName.transform.parent.gameObject.SetActive(false); 
        }

        if (qSlot2 != "")
        {
            qSTwoName.transform.parent.gameObject.SetActive(true);
            qSTwoName.text = PersistantGameManager.Instance.possibleQuests[qSlot2].questName;
        }
        else { qSTwoName.transform.parent.gameObject.SetActive(false); }

        if (qSlot3 != "")
        {
            qSThreeName.transform.parent.gameObject.SetActive(true);
            qSThreeName.text = PersistantGameManager.Instance.possibleQuests[qSlot3].questName;
        }
        else { qSThreeName.transform.parent.gameObject.SetActive(false); }

        if (qSlot4 != "")
        {
            qSFourName.transform.parent.gameObject.SetActive(true);
            qSFourName.text = PersistantGameManager.Instance.possibleQuests[qSlot4].questName;
        }
        else { qSFourName.transform.parent.gameObject.SetActive(false); }

        if (qSlot5 != "")
        {
            qSFiveName.transform.parent.gameObject.SetActive(true);
            qSFiveName.text = PersistantGameManager.Instance.possibleQuests[qSlot5].questName;
        }
        else { qSFiveName.transform.parent.gameObject.SetActive(false); }

        if (qSlot6 != "")
        {
            qSSixName.transform.parent.gameObject.SetActive(true);
            qSSixName.text = PersistantGameManager.Instance.possibleQuests[qSlot6].questName;
        }
        else { qSSixName.transform.parent.gameObject.SetActive(false); }

        if (qSlot7 != "")
        {
            qSSevenName.transform.parent.gameObject.SetActive(true);
            qSSevenName.text = PersistantGameManager.Instance.possibleQuests[qSlot7].questName;
        }
        else { qSSevenName.transform.parent.gameObject.SetActive(false); }

        if (qSlot8 != "")
        {
            qSEightName.transform.parent.gameObject.SetActive(true);
            qSEightName.text = PersistantGameManager.Instance.possibleQuests[qSlot8].questName;
        }
        else { qSEightName.transform.parent.gameObject.SetActive(false); }

        if (qSlot9 != "")
        {
            qSNineName.transform.parent.gameObject.SetActive(true);
            qSNineName.text = PersistantGameManager.Instance.possibleQuests[qSlot9].questName;
        }
        else { qSNineName.transform.parent.gameObject.SetActive(false); }



    }

    #endregion

    public void openSlot(int slot)
    {
        questDescPanel.SetActive(true);
        questsPanel.SetActive(false);

        Quest questToOpen = PersistantGameManager.Instance.possibleQuests[PersistantGameManager.Instance.activeQuests[slot - 1]];

        questName.text = questToOpen.questName;
        questDescription.text = questToOpen.questDescription;
        questReward.text = PersistantGameManager.Instance.rewards[questToOpen.questKey];

    }

    public void CloseSlot()
    {
        questDescPanel.SetActive(false);
        questsPanel.SetActive(true);
    }
}
