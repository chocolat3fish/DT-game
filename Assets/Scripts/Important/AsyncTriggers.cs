using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AsyncTriggers : MonoBehaviour
{
    public GameObject dialoguePanel;
    private void Update()
    {
        //Open Menu Canvas
        if (Input.GetKeyDown(KeyCode.Tab) && !PersistantGameManager.Instance.menuCanvasOpen && Time.timeScale != 0 && !PersistantGameManager.Instance.bugReportSceneIsOpen)
        {
            if(PersistantGameManager.Instance.dialogueSceneIsOpen)
            {
                dialoguePanel = FindObjectOfType<DialogueButtons>().gameObject.transform.parent.gameObject;
                dialoguePanel.SetActive(false);
            }
            SceneManager.LoadSceneAsync("Menu Canvas", LoadSceneMode.Additive);
            PersistantGameManager.Instance.firstTimeOpeningMenuCanvas = true;
            PersistantGameManager.Instance.menuCanvasOpen = true;
        }
        //Close Menu Canvas
        else if (Input.GetKeyDown(KeyCode.Tab) && PersistantGameManager.Instance.menuCanvasOpen)
        {
            SceneManager.UnloadSceneAsync("Menu Canvas");
            Time.timeScale = 1;
            PersistantGameManager.Instance.menuCanvasOpen = false;
            if(PersistantGameManager.Instance.dialogueSceneIsOpen)
            {
                if(dialoguePanel != null)
                {
                    dialoguePanel.SetActive(true);
                }
                dialoguePanel = null;
            }
        }
        //Switch from Character Canvas or Skills Screen to Menu Canvas
        else if (Input.GetKeyDown(KeyCode.Tab) && PersistantGameManager.Instance.characterScreenOpen || PersistantGameManager.Instance.skillsScreenOpen)
        {
            SceneManager.UnloadSceneAsync("Character Canvas");
            Time.timeScale = 1;
            PersistantGameManager.Instance.characterScreenOpen = false;
            PersistantGameManager.Instance.skillsScreenOpen = false;
            SceneManager.LoadSceneAsync("Menu Canvas", LoadSceneMode.Additive);
            PersistantGameManager.Instance.firstTimeOpeningMenuCanvas = true;
            PersistantGameManager.Instance.menuCanvasOpen = true;
        }
        //Open Character Canvas
        else if (Input.GetKeyDown(KeyCode.U) && Time.timeScale != 0 && !PersistantGameManager.Instance.characterScreenOpen  && !PersistantGameManager.Instance.bugReportSceneIsOpen)
        {
            SceneManager.LoadSceneAsync("Character Canvas", LoadSceneMode.Additive);
            PersistantGameManager.Instance.characterScreenOpen = true;
            Time.timeScale = 0;
            if (PersistantGameManager.Instance.dialogueSceneIsOpen)
            {
                dialoguePanel = FindObjectOfType<DialogueButtons>().gameObject.transform.parent.gameObject;
                dialoguePanel.SetActive(false);
            }
        }
    }

    public void OpenCompareCanvas(Weapon compareWeapon)
    {
        if (PersistantGameManager.Instance.dialogueSceneIsOpen)
        {
            dialoguePanel = FindObjectOfType<DialogueButtons>().gameObject.transform.parent.gameObject;
            dialoguePanel.SetActive(false);
        }
        PersistantGameManager.Instance.comparingWeapon = compareWeapon;
        PersistantGameManager.Instance.compareScreenOpen = true;
        SceneManager.LoadSceneAsync("Compare Canvas", LoadSceneMode.Additive);
        Time.timeScale = 0;
    }
    public void OpenBugReportCanvas()
    {
        PersistantGameManager.Instance.bugReportSceneIsOpen = true;
        SceneManager.UnloadSceneAsync("Main Canvas");
        SceneManager.LoadSceneAsync("Bug Report Canvas", LoadSceneMode.Additive);
        Time.timeScale = 0;
    }
    public void CloseBugReportCanvas()
    {
        PersistantGameManager.Instance.bugReportSceneIsOpen = false;
        SceneManager.UnloadSceneAsync("Bug Report Canvas");
        SceneManager.LoadSceneAsync("Main Canvas", LoadSceneMode.Additive);
        if (PersistantGameManager.Instance.dialogueSceneIsOpen)
        {
            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(true);
            }
            dialoguePanel = null;
        }
        Time.timeScale = 1;
    }

}
