using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AsyncTriggers : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !PersistantGameManager.Instance.menuCanvasOpen && Time.timeScale != 0)
        {
            SceneManager.LoadSceneAsync("Menu Canvas", LoadSceneMode.Additive);
            PersistantGameManager.Instance.firstTimeOpeningMenuCanvas = true;
            PersistantGameManager.Instance.menuCanvasOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && PersistantGameManager.Instance.menuCanvasOpen)
        {
            SceneManager.UnloadSceneAsync("Menu Canvas");
            Time.timeScale = 1;
            PersistantGameManager.Instance.menuCanvasOpen = false;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && PersistantGameManager.Instance.characterScreenOpen)
        {
            SceneManager.UnloadSceneAsync("Character Canvas");
            Time.timeScale = 1;
            PersistantGameManager.Instance.characterScreenOpen = false;
        }
        else if (Input.GetKeyDown(KeyCode.U) && Time.timeScale != 0 && !PersistantGameManager.Instance.characterScreenOpen)
        {
            SceneManager.LoadSceneAsync("Character Canvas", LoadSceneMode.Additive);
            PersistantGameManager.Instance.characterScreenOpen = true;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && PersistantGameManager.Instance.skillsScreenOpen)
        {
            SceneManager.UnloadSceneAsync("Character Canvas");
            Time.timeScale = 1;
            PersistantGameManager.Instance.characterScreenOpen = false;
            PersistantGameManager.Instance.skillsScreenOpen = false;
        }
    }

    public void OpenCompareCanvas(Weapon compareWeapon)
    {
        PersistantGameManager.Instance.comparingWeapon = compareWeapon;
        PersistantGameManager.Instance.compareScreenOpen = true;
        SceneManager.LoadSceneAsync("Compare Canvas", LoadSceneMode.Additive);
        Time.timeScale = 0;
    }

}
