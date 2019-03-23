using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvasScript : MonoBehaviour
{
    public bool isActive;
    public bool freeze;
    public GameObject mainPanel;
    public GameObject itemsPanel;
    public GameObject consumablesPanel;

    private bool openItems, closeItems, openConsumables, closeConsumables;

    private Text sW1Output, sW2Output, sW3Output;
    private Text wD1Output, wD2Output, wD3Output;
    private Text wS1Output, wS2Output, wS3Output;
    private Text wR1Output, wR2Output, wR3Output;
    private Text sThreeName, sTwoName, sOneName, sSixName, sFiveName, sFourName, sSevenName, sEightName, sNineName;
    private Text sOneOutput, sTwoOutput, sThreeOutput, sFourOutput, sFiveOutput, sSixOutput, sSevenOutput, sEightOutput, sNineOutput;

    private string slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9;


    void Awake()
    {
        mainPanel = gameObject.transform.Find("MainPanel").gameObject;
        itemsPanel = gameObject.transform.Find("ItemsPanel").gameObject;
        consumablesPanel = gameObject.transform.Find("ConsumablesPanel").gameObject;


        sW1Output = itemsPanel.transform.Find("SW1").gameObject.GetComponent<Text>();
        sW2Output = itemsPanel.transform.Find("SW2").gameObject.GetComponent<Text>();
        sW3Output = itemsPanel.transform.Find("SW3").gameObject.GetComponent<Text>();
        wD1Output = itemsPanel.transform.Find("WD1").gameObject.GetComponent<Text>();
        wD2Output = itemsPanel.transform.Find("WD2").gameObject.GetComponent<Text>();
        wD3Output = itemsPanel.transform.Find("WD3").gameObject.GetComponent<Text>();
        wS1Output = itemsPanel.transform.Find("WS1").gameObject.GetComponent<Text>();
        wS2Output = itemsPanel.transform.Find("WS2").gameObject.GetComponent<Text>();
        wS3Output = itemsPanel.transform.Find("WS3").gameObject.GetComponent<Text>();
        wR1Output = itemsPanel.transform.Find("WR1").gameObject.GetComponent<Text>();
        wR2Output = itemsPanel.transform.Find("WR2").gameObject.GetComponent<Text>();
        wR3Output = itemsPanel.transform.Find("WR3").gameObject.GetComponent<Text>();


        sOneName = consumablesPanel.transform.Find("Slot 1").gameObject.GetComponent<Text>();
        sTwoName = consumablesPanel.transform.Find("Slot 2").gameObject.GetComponent<Text>();
        sThreeName = consumablesPanel.transform.Find("Slot 3").gameObject.GetComponent<Text>();

        sFourName = consumablesPanel.transform.Find("Slot 4").gameObject.GetComponent<Text>();
        sFiveName = consumablesPanel.transform.Find("Slot 5").gameObject.GetComponent<Text>();
        sSixName = consumablesPanel.transform.Find("Slot 6").gameObject.GetComponent<Text>();

        sSevenName = consumablesPanel.transform.Find("Slot 7").gameObject.GetComponent<Text>();
        sEightName = consumablesPanel.transform.Find("Slot 8").gameObject.GetComponent<Text>();
        sNineName = consumablesPanel.transform.Find("Slot 9").gameObject.GetComponent<Text>();

        sOneOutput = consumablesPanel.transform.Find("Q1").gameObject.GetComponent<Text>();
        sTwoOutput = consumablesPanel.transform.Find("Q2").gameObject.GetComponent<Text>();
        sThreeOutput = consumablesPanel.transform.Find("Q3").gameObject.GetComponent<Text>();

        sFourOutput = consumablesPanel.transform.Find("Q4").gameObject.GetComponent<Text>();
        sFiveOutput = consumablesPanel.transform.Find("Q5").gameObject.GetComponent<Text>();
        sSixOutput = consumablesPanel.transform.Find("Q6").gameObject.GetComponent<Text>();

        sSevenOutput = consumablesPanel.transform.Find("Q7").gameObject.GetComponent<Text>();
        sEightOutput = consumablesPanel.transform.Find("Q8").gameObject.GetComponent<Text>();
        sNineOutput = consumablesPanel.transform.Find("Q9").gameObject.GetComponent<Text>();


    }

    void Start()
    {
        mainPanel.SetActive(false);
        itemsPanel.SetActive(false);
        consumablesPanel.SetActive(false);
        openItems = false;
        closeItems = false;
        closeConsumables = false;
        openConsumables = false;

    }

    void Update()
    { 
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
            itemsPanel.SetActive(false);
            consumablesPanel.SetActive(false);
            isActive = false;
            freeze = false;
            PersistantGameManager.Instance.menuScreenOpen = false;
            Time.timeScale = 1;
        }


        if (mainPanel.activeSelf && Input.GetKeyDown(KeyCode.L) && itemsPanel.activeSelf == true)
        {

            closeItems = true;
            openItems = false;

        }
        else if (mainPanel.activeSelf && Input.GetKeyDown(KeyCode.L) && itemsPanel.activeSelf == false)
        {
            openItems = true;
            closeItems = false;
        }


        if (mainPanel.gameObject.activeSelf && Input.GetKeyDown(KeyCode.K) && consumablesPanel.activeSelf == true)
        {
            closeConsumables = true;
            openConsumables = false;
        }
        if (mainPanel.gameObject.activeSelf && Input.GetKeyDown(KeyCode.K) && consumablesPanel.activeSelf == false)
        {
            openConsumables = true;
            closeConsumables = false;
        }


    }

    private void LateUpdate()
    {
        if (openItems == true)
        {
            Debug.Log("openI");
            OpenItemsMenu();
            openItems = false;
        }
        else if (closeItems == true)
        {
            Debug.Log("closeI");
            CloseItemsMenu();
            closeItems = false;
        }

        if (openConsumables == true)
        {
            Debug.Log("openC");
            OpenConsumablesMenu();
            openConsumables = false;
        }
        else if (closeConsumables == true)
        {
            Debug.Log("closeC");
            CloseConsumablesMenu();
            closeConsumables = false;
        }


    }

    public void OpenItemsMenu()
    {
        itemsPanel.SetActive(true);

    }

    public void CloseItemsMenu()
    {
        itemsPanel.SetActive(false);

    }

    public void OpenConsumablesMenu()
    {
        consumablesPanel.SetActive(true);

    }

    public void CloseConsumablesMenu()
    {

        consumablesPanel.SetActive(false);

    }


    private void UpdateData()
    {
        slot1 = "";
        slot2 = "";
        slot3 = "";
        slot4 = "";
        slot5 = "";
        slot6 = "";
        slot7 = "";
        slot8 = "";
        slot9 = "";

        sOneOutput.text = "";
        sTwoOutput.text = "";
        sThreeOutput.text = "";
        sFourOutput.text = "";
        sFiveOutput.text = "";
        sSixOutput.text = "";
        sSevenOutput.text = "";
        sEightOutput.text = "";
        sNineOutput.text = "";

        sW1Output.text = PersistantGameManager.Instance.playerWeaponInventory[0].itemName;
        sW2Output.text = PersistantGameManager.Instance.playerWeaponInventory[1].itemName;
        sW3Output.text = PersistantGameManager.Instance.playerWeaponInventory[2].itemName;

        wD1Output.text = PersistantGameManager.Instance.playerWeaponInventory[0].itemDamage.ToString();
        wD2Output.text = PersistantGameManager.Instance.playerWeaponInventory[1].itemDamage.ToString();
        wD3Output.text = PersistantGameManager.Instance.playerWeaponInventory[2].itemDamage.ToString();

        wS1Output.text = PersistantGameManager.Instance.playerWeaponInventory[0].itemSpeed.ToString();
        wS2Output.text = PersistantGameManager.Instance.playerWeaponInventory[1].itemSpeed.ToString();
        wS3Output.text = PersistantGameManager.Instance.playerWeaponInventory[2].itemSpeed.ToString();

        wR1Output.text = PersistantGameManager.Instance.playerWeaponInventory[0].itemRange.ToString();
        wR2Output.text = PersistantGameManager.Instance.playerWeaponInventory[1].itemRange.ToString();
        wR3Output.text = PersistantGameManager.Instance.playerWeaponInventory[2].itemRange.ToString();

       // for (int element = 1; element < PersistantGameManager.Instance.possibleItems.Count; element++)
       foreach(string element in PersistantGameManager.Instance.possibleItems)
        {
            if (slot1 == "")
            {
                if (PersistantGameManager.Instance.amountOfItems[element] > 0)
                {
                    slot1 = element;
                }
            }
            else if(slot2 == "")
            {
                if (PersistantGameManager.Instance.amountOfItems[element] > 0)
                {
                    slot2 = element;
                }
            }
            else if (slot3 == "")
            {
                if (PersistantGameManager.Instance.amountOfItems[element] > 0)
                {
                    slot3 = element;
                }
            }
            else if (slot4 == "")
            {
                if (PersistantGameManager.Instance.amountOfItems[element] > 0)
                {
                    slot4 = element;
                }
            }
            else if (slot5 == "")
            {
                if (PersistantGameManager.Instance.amountOfItems[element] > 0)
                {
                    slot5 = element;
                }
            }
            else if (slot6 == "")
            {
                if(PersistantGameManager.Instance.amountOfItems[element] > 0)
                {
                    slot6 = element;
                }

            }
            else if (slot7 == "")
            {
                if (PersistantGameManager.Instance.amountOfItems[element] > 0)
                {
                    slot7 = element;
                }

            }
            else if (slot8 == "")
            {
                if (PersistantGameManager.Instance.amountOfItems[element] > 0)
                {
                    slot8 = element;
                }

            }
            else if (slot9 == "")
            {
                if (PersistantGameManager.Instance.amountOfItems[element] > 0)
                {
                    slot9 = element;
                }

            }

        }

        sOneName.text = slot1;
        sTwoName.text = slot2;
        sThreeName.text = slot3;

        sFourName.text = slot4;
        sFiveName.text = slot5;
        sSixName.text = slot6;

        sSevenName.text = slot7;
        sEightName.text = slot8;
        sNineName.text = slot9;



        if (slot1 != "")
        {
            sOneOutput.text = PersistantGameManager.Instance.amountOfItems[slot1].ToString();
        }
        else { sOneOutput.text = ""; }

        if (slot2 != "")
        {
            sTwoOutput.text = PersistantGameManager.Instance.amountOfItems[slot2].ToString();
        }
        else { sTwoOutput.text = ""; }

        if (slot3 != "")
        {
           sThreeOutput.text = PersistantGameManager.Instance.amountOfItems[slot3].ToString();
        }
        else { sThreeOutput.text = ""; }

        if (slot4 != "")
        {
            sFourOutput.text = PersistantGameManager.Instance.amountOfItems[slot4].ToString();
        }
        else { sFourOutput.text = ""; }

        if (slot5 != "")
        {
            sFiveOutput.text = PersistantGameManager.Instance.amountOfItems[slot5].ToString();
        }
        else { sFiveOutput.text = ""; }

        if (slot6 != "")
        {
            sSixOutput.text = PersistantGameManager.Instance.amountOfItems[slot6].ToString();
        }
        else { sSixOutput.text = ""; }

        if (slot7 != "")
        {
            sSevenOutput.text = PersistantGameManager.Instance.amountOfItems[slot7].ToString();
        }
        else { sSevenOutput.text = ""; }

        if (slot8 != "")
        {
            sEightOutput.text = PersistantGameManager.Instance.amountOfItems[slot8].ToString();
        }
        else { sEightOutput.text = ""; }

        if (slot9 != "")
        {
            sNineOutput.text = PersistantGameManager.Instance.amountOfItems[slot9].ToString();
        }
        else { sNineOutput.text = ""; }



    }

}
