using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    GameObject miniMap;

    private bool shouldClose, shouldOpen;

    private void Awake()
    {
        miniMap = transform.Find("Minimap Panel").gameObject;
        miniMap.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(miniMap.activeSelf && Time.timeScale != 0)
            {
                shouldClose = true;
                shouldOpen = false;
            }
            else if(!miniMap.activeSelf && Time.timeScale != 0)
            {
                shouldOpen = true;
                shouldClose = false;
            }
        }
        if(Time.timeScale == 0 && miniMap.activeSelf)
        {
            miniMap.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        if(shouldClose)
        {
            miniMap.SetActive(false);
            shouldOpen = false;
            shouldClose = false;
        }
        if(shouldOpen)
        {
            miniMap.SetActive(true);
        }
    }
}
