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
    public Text fullHOutput, halfHOutput, fifthHOutput, fullAOutput, halfAOutput, fifthAOutput;


    void Awake()
    {
        mainPanel = gameObject.transform.Find("MainPanel").gameObject;
        itemsPanel = gameObject.transform.Find("ItemsPanel").gameObject;
        consumablesPanel = gameObject.transform.Find("ConsumablesPanel").gameObject;

        //SW = show weapon
        //WD = weapon damage
        //WS = weapon speed
        //WR = weapon range

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

        //fifthH = 20% (one fifth) health
        //halfH = 50% health
        //fullH = 100% health

        fifthHOutput = consumablesPanel.transform.Find("20HQ").gameObject.GetComponent<Text>();
        halfHOutput = consumablesPanel.transform.Find("50HQ").gameObject.GetComponent<Text>();
        fullHOutput = consumablesPanel.transform.Find("100HQ").gameObject.GetComponent<Text>();

        //fifthA = 20% (one fifth) Attack
        //halfA = 50% Attack
        //fullA = 100% Attack
        fifthAOutput = consumablesPanel.transform.Find("20AQ").gameObject.GetComponent<Text>();
        halfAOutput = consumablesPanel.transform.Find("50AQ").gameObject.GetComponent<Text>();
        fullAOutput = consumablesPanel.transform.Find("100AQ").gameObject.GetComponent<Text>();


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

        fifthHOutput.text = PersistantGameManager.Instance.amountOfItems["20%H"].ToString();
        halfHOutput.text = PersistantGameManager.Instance.amountOfItems["50%H"].ToString();
        fullHOutput.text = PersistantGameManager.Instance.amountOfItems["100%H"].ToString();

        fifthAOutput.text = PersistantGameManager.Instance.amountOfItems["20%A"].ToString();
        halfAOutput.text = PersistantGameManager.Instance.amountOfItems["50%A"].ToString();
        fullAOutput.text = PersistantGameManager.Instance.amountOfItems["100%A"].ToString();

    }

}
