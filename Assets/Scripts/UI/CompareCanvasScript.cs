using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CompareCanvasScript : MonoBehaviour
{
    public bool isActive;
    private LootDropMonitor lootDropMonitor;
    private bool detecting = true;

    public GameObject currentWeaponPanel;
    public GameObject newWeaponPanel;

    //Testing not removing canvases 
    //public List<Canvas> canvases = new List<Canvas>();
    //private List<Canvas> turnedOnCanvases = new List<Canvas>();
    //public Canvas[] removeCanvas;

    private Text cWNOutput;
    private Text cWDOutput;
    private Text cWSOutput;
    private Text cWROutput;
    private Text nWNOutput;
    private Text nWDOutput;
    private Text nWSOutput;
    private Text nWROutput;
    private Text cWLOutput;
    private Text nWLOutput;

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
        cWLOutput = currentWeaponPanel.transform.Find("CWL").gameObject.GetComponent<Text>();

        nWNOutput = newWeaponPanel.transform.Find("NWN").gameObject.GetComponent<Text>();
        nWDOutput = newWeaponPanel.transform.Find("NWD").gameObject.GetComponent<Text>();
        nWSOutput = newWeaponPanel.transform.Find("NWS").gameObject.GetComponent<Text>();
        nWROutput = newWeaponPanel.transform.Find("NWR").gameObject.GetComponent<Text>();
        nWLOutput = newWeaponPanel.transform.Find("NWL").gameObject.GetComponent<Text>();

        CWPButton = currentWeaponPanel.transform.Find("Button").GetComponent<Button>();
        NWPButton = newWeaponPanel.transform.Find("Button").GetComponent<Button>();

        //CWPButton.onClick.AddListener(ContinueGame);
        //NWPButton.onClick.AddListener(ChooseNewWeapon);

        /*
        Canvas[] tempCanvases = FindObjectsOfType<Canvas>();
        foreach(Canvas canvas in tempCanvases)
        {
            if(canvas.gameObject.tag == "Enemy Health Bar") { continue; }
            canvases.Add(canvas);
        }
        */



    }

    private void Start()
    {
        UpdateData();
        currentWeaponPanel.SetActive(true);
        newWeaponPanel.SetActive(true);
        detecting = false;
        Time.timeScale = 0;
        takeEInputForContinue = false;
    }
    private void Update()
    {
        UpdateData();
        takeEInputForContinue = true;

        if (Input.GetKeyDown(KeyCode.E) && !detecting && takeEInputForContinue && PersistantGameManager.Instance.compareScreenOpen)
        {
            takeEInputForContinue = false;
            ContinueGame(); 
        }

        if (Input.GetKeyDown(KeyCode.K) && !detecting && PersistantGameManager.Instance.compareScreenOpen)
        {
            ContinueGame();
        }

        if (Input.GetKeyDown(KeyCode.L) && !detecting && PersistantGameManager.Instance.compareScreenOpen)
        {
            ChooseNewWeapon();
        }

    }
    public void UpdateData()
    {
        cWNOutput.text = PersistantGameManager.Instance.currentWeapon.itemName;
        cWDOutput.text = PersistantGameManager.Instance.currentWeapon.itemDamage.ToString();
        cWSOutput.text = PersistantGameManager.Instance.currentWeapon.itemSpeed.ToString();
        cWROutput.text = PersistantGameManager.Instance.currentWeapon.itemRange.ToString();
        cWLOutput.text = PersistantGameManager.Instance.currentWeapon.itemLevel.ToString();

        nWNOutput.text = PersistantGameManager.Instance.comparingWeapon.itemName;
        nWDOutput.text = PersistantGameManager.Instance.comparingWeapon.itemDamage.ToString();
        nWSOutput.text = PersistantGameManager.Instance.comparingWeapon.itemSpeed.ToString();
        nWROutput.text = PersistantGameManager.Instance.comparingWeapon.itemRange.ToString();
        nWLOutput.text = PersistantGameManager.Instance.comparingWeapon.itemLevel.ToString();
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
        Time.timeScale = 1;
        PersistantGameManager.Instance.compareScreenOpen = false;
        SceneManager.UnloadSceneAsync("Compare Canvas");
    }

}
