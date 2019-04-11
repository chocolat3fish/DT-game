using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CharacterCanvasScript : MonoBehaviour
{
    public bool isActive;
    public GameObject mainPanel;
    public GameObject skillsPanel;

    private bool openSkills, closeSkills;

    void Awake()
    {
        mainPanel = gameObject.transform.Find("Character Panel").gameObject;
        skillsPanel = gameObject.transform.Find("Skills Panel").gameObject;

    }

    void Start()
    {
        mainPanel.SetActive(true);
        skillsPanel.SetActive(false);
        isActive = true;
        PersistantGameManager.Instance.characterScreenOpen = true;

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
        }*/


        if (Input.GetKeyDown(KeyCode.U) && isActive)
        {
            isActive = false;
            PersistantGameManager.Instance.characterScreenOpen = false;
            PersistantGameManager.Instance.skillsScreenOpen = false;
            Time.timeScale = 1;
            SceneManager.UnloadSceneAsync("Character Canvas");
        }
        if (mainPanel.activeSelf && Input.GetKeyDown(KeyCode.K) && skillsPanel.activeSelf == false)
        {
            openSkills = true;
            closeSkills = false;
        }


        if (skillsPanel.activeSelf && Input.GetKeyDown(KeyCode.K) && skillsPanel.activeSelf == true)
        {
            closeSkills = true;
            openSkills = false;
        }


    }

    private void LateUpdate()
    {
        if (openSkills)
        {
            OpenSkillMenu();
            openSkills = false;
        }
        else if (closeSkills)
        {
            CloseSkillMenu();
            closeSkills = false;
        }
    }


    public void OpenSkillMenu()
    {
        skillsPanel.SetActive(true);
        mainPanel.SetActive(false);
        isActive = false;
    }

    public void CloseSkillMenu()
    {
        mainPanel.SetActive(true);
        skillsPanel.SetActive(false);
        isActive = true;
    }
}