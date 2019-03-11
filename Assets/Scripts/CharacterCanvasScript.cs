using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterCanvasScript : MonoBehaviour
{
    public bool isActive;
    public bool freeze;
    public GameObject mainPanel;
    public GameObject skillsPanel;

    void Awake()
    {
        mainPanel = gameObject.transform.Find("Character Panel").gameObject;
        skillsPanel = gameObject.transform.Find("Skills Panel").gameObject;

    }

    void Start()
    {
        mainPanel.SetActive(false);
        skillsPanel.SetActive(false);

    }

    void Update()
    {

        if (freeze)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.U) && !isActive && skillsPanel.activeSelf == false)
        {
            mainPanel.SetActive(true);
            isActive = true;
            freeze = true;
        }
        else if (Input.GetKeyDown(KeyCode.U) && isActive)
        {
            mainPanel.SetActive(false);
            isActive = false;
            freeze = false;
        }

        else if (Input.GetKeyDown(KeyCode.U) && !isActive && skillsPanel.activeSelf == true)
        {
            skillsPanel.SetActive(false);
            isActive = false;
            freeze = false;
        }

        if (mainPanel.activeSelf && Input.GetKeyDown(KeyCode.K))
        {
            OpenSkillMenu();
        }


        if (skillsPanel.activeSelf && Input.GetKeyDown(KeyCode.L))
        {
            CloseSkillMenu();
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