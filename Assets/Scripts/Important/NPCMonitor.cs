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


    public bool isTalking;
    public bool canContinueDialouge;
    public Dialouge dialouge;

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
        overlayNameText.text = dialouge.NPCName;
        currentSentenceIndex = 0;
        isTalking = true;
        canContinueDialouge = false;
        dialougeBoxQuery = true;
        dialougeBoxQueryTime = Time.time;

        overlayNameText.text = dialouge.NPCName;
        StartCoroutine(AddChars(dialouge.sentences[0], overlayMainText));

    }
    private void Update()
    {
        if(dialougeBoxQuery && (dialougeBoxQueryTime < (Time.time - 0.5f)))
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
            if (currentSentenceIndex == dialouge.sentences.Length)
            {
                EndDialouge();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(AddChars(dialouge.sentences[currentSentenceIndex], overlayMainText));
                canContinueDialouge = false;
                dialougeBoxQuery = true;
                dialougeBoxQueryTime = Time.time;

            }
        }
        
        
    }
    private void EndDialouge()
    {
        StopAllCoroutines();
        dialougeBoxOpen = false;
        OverlayPanel.SetActive(false);
        isTalking = false;
        canContinueDialouge = false;
    }

    IEnumerator AddChars(string sentence, Text text)
    {
        text.text = "";
        foreach (char ch in sentence)
        {
            text.text += ch;
            yield return null;
            
        }
    }
}
