using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AsyncTriggers : MonoBehaviour
{
    public GameObject dialoguePanel;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !PersistantGameManager.Instance.menuCanvasOpen && Time.timeScale != 0)
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
        else if (Input.GetKeyDown(KeyCode.Tab) && PersistantGameManager.Instance.characterScreenOpen)
        {
            SceneManager.UnloadSceneAsync("Character Canvas");
            Time.timeScale = 1;
            PersistantGameManager.Instance.characterScreenOpen = false;
            SceneManager.LoadSceneAsync("Menu Canvas", LoadSceneMode.Additive);
            PersistantGameManager.Instance.firstTimeOpeningMenuCanvas = true;
            PersistantGameManager.Instance.menuCanvasOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.U) && Time.timeScale != 0 && !PersistantGameManager.Instance.characterScreenOpen)
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
        else if (Input.GetKeyDown(KeyCode.Tab) && PersistantGameManager.Instance.skillsScreenOpen)
        {
            SceneManager.UnloadSceneAsync("Character Canvas");
            Time.timeScale = 1;
            PersistantGameManager.Instance.characterScreenOpen = false;
            PersistantGameManager.Instance.skillsScreenOpen = false;
            if (PersistantGameManager.Instance.dialogueSceneIsOpen)
            {
                dialoguePanel = FindObjectOfType<DialogueButtons>().gameObject.transform.parent.gameObject;
                dialoguePanel.SetActive(false);
            }
            SceneManager.LoadSceneAsync("Menu Canvas", LoadSceneMode.Additive);
            PersistantGameManager.Instance.firstTimeOpeningMenuCanvas = true;
            PersistantGameManager.Instance.menuCanvasOpen = true;
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
        if (PersistantGameManager.Instance.dialogueSceneIsOpen)
        {
            dialoguePanel = FindObjectOfType<DialogueButtons>().gameObject.transform.parent.gameObject;
            dialoguePanel.SetActive(false);
        }
        SceneManager.LoadSceneAsync("Bug Report Canvas", LoadSceneMode.Additive);
        Time.timeScale = 0;
    }
    public void CloseBugReportCanvas()
    {
        SceneManager.UnloadSceneAsync("Bug Report Canvas");
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
