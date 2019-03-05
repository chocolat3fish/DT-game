using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CompareCanvasScript : MonoBehaviour
{
    public bool isActive;
    private LootDropMonitor lootDropMonitor;
    private bool detecting = true;

    public GameObject currentWeaponPanel;
    public GameObject newWeaponPanel;

    public List<Canvas> canvases = new List<Canvas>();
    private List<Canvas> turnedOnCanvases = new List<Canvas>();
    public Canvas[] removeCanvas;

    private Text cWNOutput;
    private Text cWDOutput;
    private Text cWSOutput;
    private Text cWROutput;
    private Text nWNOutput;
    private Text nWDOutput;
    private Text nWSOutput;
    private Text nWROutput;

    public Button CWPButton;
    public Button NWPButton;

    public bool takeEInputForContinue;
    private void Awake()
    {
        
        currentWeaponPanel = GameObject.Find("Current Weapon Panel");
        newWeaponPanel = GameObject.Find("New Weapon Panel");

        cWNOutput = currentWeaponPanel.transform.Find("CWN").gameObject.GetComponent<Text>();
        cWDOutput = currentWeaponPanel.transform.Find("CWD").gameObject.GetComponent<Text>();
        cWSOutput = currentWeaponPanel.transform.Find("CWS").gameObject.GetComponent<Text>();
        cWROutput = currentWeaponPanel.transform.Find("CWR").gameObject.GetComponent<Text>();

        nWNOutput = newWeaponPanel.transform.Find("NWN").gameObject.GetComponent<Text>();
        nWDOutput = newWeaponPanel.transform.Find("NWD").gameObject.GetComponent<Text>();
        nWSOutput = newWeaponPanel.transform.Find("NWS").gameObject.GetComponent<Text>();
        nWROutput = newWeaponPanel.transform.Find("NWR").gameObject.GetComponent<Text>();

        CWPButton = currentWeaponPanel.transform.Find("Button").GetComponent<Button>();
        NWPButton = newWeaponPanel.transform.Find("Button").GetComponent<Button>();

        CWPButton.onClick.AddListener(ContinueGame);
        NWPButton.onClick.AddListener(ChooseNewWeapon);


        Canvas[] tempCanvases = FindObjectsOfType<Canvas>();
        foreach(Canvas canvas in tempCanvases)
        {
            if(canvas.gameObject.tag == "Enemy Health Bar") { continue; }
            canvases.Add(canvas);
        }




    }

    private void Start()
    {
        currentWeaponPanel.SetActive(false);
        newWeaponPanel.SetActive(false);
    }
    private void Update()
    {
        takeEInputForContinue = true;
        if (detecting)
        {


            if (PersistantGameManager.Instance.compareScreenOpen)
            {
                
                turnedOnCanvases = new List<Canvas>();
                foreach (Canvas canvas in canvases)
                {
                    if (canvas.gameObject.activeSelf && canvas.gameObject != gameObject)
                    {
                        turnedOnCanvases.Add(canvas);
                        canvas.gameObject.SetActive(false);
                    }
                }
                UpdateData();
                Debug.Log("1");
                currentWeaponPanel.SetActive(true);
                newWeaponPanel.SetActive(true);
                Debug.Log("2");
                detecting = false;
                Time.timeScale = 0;
                Debug.Log("3");
                takeEInputForContinue = false;
            }
        }
        if(Input.GetKeyDown(KeyCode.E) && !detecting && takeEInputForContinue && PersistantGameManager.Instance.compareScreenOpen)
        {
            takeEInputForContinue = false;
        ContinueGame(); }





    }
    private void UpdateData()
    {
        cWNOutput.text = PersistantGameManager.Instance.currentWeapon.itemName;
        cWDOutput.text = PersistantGameManager.Instance.currentWeapon.itemDamage.ToString();
        cWSOutput.text = PersistantGameManager.Instance.currentWeapon.itemSpeed.ToString();
        cWROutput.text = PersistantGameManager.Instance.currentWeapon.itemRange.ToString();

        nWNOutput.text = PersistantGameManager.Instance.comparingWeapon.itemName;
        nWDOutput.text = PersistantGameManager.Instance.comparingWeapon.itemDamage.ToString();
        nWSOutput.text = PersistantGameManager.Instance.comparingWeapon.itemSpeed.ToString();
        nWROutput.text = PersistantGameManager.Instance.comparingWeapon.itemRange.ToString();
    }

    public void ChooseNewWeapon()
    {
        Weapon tempWeapon = PersistantGameManager.Instance.currentWeapon;
        PersistantGameManager.Instance.currentWeapon = PersistantGameManager.Instance.comparingWeapon;
        PersistantGameManager.Instance.comparingWeapon = tempWeapon;
        PersistantGameManager.Instance.playerWeaponInventory[PersistantGameManager.Instance.currentIndex] = PersistantGameManager.Instance.currentWeapon;

        ContinueGame();
    }

    public void ContinueGame()
    {
        currentWeaponPanel.SetActive(false);
        newWeaponPanel.SetActive(false);

        foreach (Canvas canvas in turnedOnCanvases )
        {
            if(canvas.gameObject != gameObject)
            {
                canvas.gameObject.SetActive(true);
            }
        }
        currentWeaponPanel.SetActive(false);
        newWeaponPanel.SetActive(false);
        detecting = true;
        Time.timeScale = 1;
        PersistantGameManager.Instance.compareScreenOpen = false;
    }
    

}
