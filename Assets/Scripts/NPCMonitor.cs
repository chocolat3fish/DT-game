using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMonitor : MonoBehaviour
{   
    //different panels in game panels are 
    public GameObject OverlayPanel;
    public GameObject toTalkPanel;
    public GameObject Player;

    public string NPCName;

    private bool canTalk;
    private bool dialougeBoxOpen;

    public Text mainOverlayText;
    public Button overlayContinueButton;
    public Text overlayNameText;

    private void Start()
    {
        OverlayPanel.SetActive(false);
        toTalkPanel.SetActive(false);
        dialougeBoxOpen = false;

        mainOverlayText = OverlayPanel.transform.Find("Text").GetComponent<Text>();
        overlayContinueButton = OverlayPanel.transform.Find("Button").GetComponent<Button>();
        overlayNameText = OverlayPanel.transform.Find("Name").Find("Text").GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        float distance = Vector2.Distance(Player.transform.position, transform.position);
        if (distance < 3f)
        {
            if (!toTalkPanel.activeSelf)
            {
                toTalkPanel.SetActive(true);
            }
            canTalk = true;
        }
        else
        {
            if(toTalkPanel.activeSelf)
            {
                toTalkPanel.SetActive(false);
            }
            canTalk = false;
        }
        if (canTalk)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (!dialougeBoxOpen)
                {
                    CreateDialougBox(NPCName);
                    Debug.Log("dialouge box open");
                }
                 
            }

        }
    }

    private void CreateDialougBox(string NPCName)
    {
        OverlayPanel.SetActive(true);
        overlayNameText.text = NPCName;
    }


    
}
