using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject loadObjects;
    public GameObject mainObjects;


    public string startScene;

    void Start()
    {
        loadObjects = transform.Find("Load").gameObject;
        mainObjects = transform.Find("Main").gameObject;
    }

    public void PressedNewGame()
    {
        SceneManager.LoadScene(startScene);
    }

    public void PressedLoadGame()
    {
        mainObjects.SetActive(false);
        loadObjects.SetActive(true);
    }

    public void ReturnToMain()
    {
        mainObjects.SetActive(true);
        loadObjects.SetActive(false);
    }

    public void PressedQuitGame()
    {
        Application.Quit();
    }
}
