using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    GameObject miniMap;
    GameObject miniMapCamera;
    private bool shouldClose, shouldOpen;

    private void Awake()
    {
        miniMap = transform.Find("Minimap Panel").gameObject;
        Camera[] cameras = FindObjectsOfType<Camera>();
        foreach(Camera camera in cameras)
        {
            if(camera.name == "Minimap Camera")
            { 
                miniMapCamera = camera.gameObject;
            }
        }
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
            Vector2 goToPos = PersistantGameManager.Instance.player.transform.position;
            miniMapCamera.transform.position = new Vector3(goToPos.x, goToPos.y, -10f);
            miniMap.SetActive(true);
            shouldOpen = false;
            shouldClose = false;
        }
    }
}
