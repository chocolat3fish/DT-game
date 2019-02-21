using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMonitor : MonoBehaviour
{   
    //different panels in game 
    public GameObject OverlayPanel;
    public GameObject toTalkPanel;

    public GameObject Player;

    public string NPCName;
    public string[] sentences;
    public bool isTalking;
    public bool canContinueDialouge;

    private float dialougeBoxQueryTime;
    private bool dialougeBoxQuery;
    private bool canTalk;
    private bool dialougeBoxOpen;
    private int currentSentenceIndex;

    private Text overlayMainText;
    private Button overlayContinueButton;
    private Text overlayNameText;

    private void Start()
    {
        OverlayPanel.SetActive(false);
        toTalkPanel.SetActive(false);
        dialougeBoxOpen = false;

        overlayMainText = OverlayPanel.transform.Find("Text").GetComponent<Text>();
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
            if(dialougeBoxOpen)
            {
                EndDialouge();
            }
        }
        if (canTalk)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (!dialougeBoxOpen)
                {
                    CreateDialougBox();
                    Debug.Log("dialouge box open");
                }
                 
            }

        }
    }

    private void CreateDialougBox()
    {
        dialougeBoxOpen = true;
        OverlayPanel.SetActive(true);
        overlayNameText.text = NPCName;
        overlayMainText.text = sentences[0];
        currentSentenceIndex = 0;
        isTalking = true;
        canContinueDialouge = false;
        dialougeBoxQuery = true;
        dialougeBoxQueryTime = Time.time;

    }
    private void Update()
    {
        if(dialougeBoxQuery && (dialougeBoxQueryTime < (Time.time - 1f)))
        {
            dialougeBoxQuery = false;
            canContinueDialouge = true;
            print(Time.time);
        }
    }
    public void ContinueDialouge()
    {   
        if(canContinueDialouge)
        {
            currentSentenceIndex++;
            if (currentSentenceIndex == sentences.Length)
            {
                EndDialouge();
            }
            else
            {

                overlayMainText.text = sentences[currentSentenceIndex];
                canContinueDialouge = false;
                dialougeBoxQuery = true;
                dialougeBoxQueryTime = Time.time;

            }
        }
        
        
    }
    private void EndDialouge()
    {
        dialougeBoxOpen = false;
        OverlayPanel.SetActive(false);
        isTalking = false;
        canContinueDialouge = false;
    }

    
}
