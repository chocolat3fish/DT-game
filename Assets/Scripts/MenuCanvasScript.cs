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

    private Text sW1Output, sW2Output, sW3Output;
    private Text wD1Output, wD2Output, wD3Output;
    private Text wS1Output, wS2Output, wS3Output;
    private Text wR1Output, wR2Output, wR3Output;


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

    }

    void Start()
    {
        mainPanel.SetActive(false);
        itemsPanel.SetActive(false);
        consumablesPanel.SetActive(false);

    }

    void Update()
    {
        /*
        if (freeze)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        */


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


        if (mainPanel.activeSelf && Input.GetKeyDown(KeyCode.L) && itemsPanel.activeSelf == false)
        {
            OpenItemsMenu();
        }


        if (itemsPanel.activeSelf && Input.GetKeyDown(KeyCode.Semicolon) && itemsPanel.activeSelf == true)
        {
            CloseItemsMenu();
        }





    }

    public void OpenItemsMenu()
    {
        itemsPanel.SetActive(true);
        mainPanel.SetActive(true);
        isActive = false;
    }

    public void CloseItemsMenu()
    {
        mainPanel.SetActive(true);
        itemsPanel.SetActive(false);
        isActive = true;
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

    }

}
