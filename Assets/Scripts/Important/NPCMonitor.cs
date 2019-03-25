using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//******MUST BE CHILDED TO A GAMEOBJECT ACTING AS A NPC IT MUST HAVE A TO TALK CANVAS CHILDED AND A CANVAS NAMED "Dialouge Canvas" IN THE SCENE******\\

//A script that monitors an npc and opens certain canvas at the right time as well as controlling dialouge
public class NPCMonitor : MonoBehaviour
{   
    //different panels in game 
    private GameObject overlayPanel;
    private GameObject toTalkPanel;

    private GameObject player;


    public bool isTalking;
    public bool canContinueDialouge;
    public Quest currentQuest;

    public string nameOfNpc;
    private float dialougeBoxQueryTime;
    private bool dialougeBoxQuery;
    private bool canTalk;
    private bool dialougeBoxOpen;
    private int currentSentenceIndex;
    private int stageOfConvo;

    private Text overlayMainText;
    private Button overlayContinueButton;
    private Text overlayNameText;

    private void Awake()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach(Canvas canvas in canvases)
        {
            if(canvas.gameObject.name == "ToTalkPanel")
            {
                toTalkPanel = canvas.gameObject;
            }
            else if (canvas.gameObject.name == "Dialogue Canvas")
            {
                overlayPanel = canvas.gameObject.transform.Find("Panel").gameObject;
            }
        }
        player = FindObjectOfType<PlayerControls>().gameObject;
    }

    private void Start()
    {
        overlayPanel.SetActive(false);
        toTalkPanel.SetActive(false);
        dialougeBoxOpen = false;

        

        overlayMainText = overlayPanel.transform.Find("Text").GetComponent<Text>();
        overlayContinueButton = overlayPanel.transform.Find("Button").GetComponent<Button>();
        overlayNameText = overlayPanel.transform.Find("Name").Find("Text").GetComponent<Text>();
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
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
            if (toTalkPanel.activeSelf)
            {
                toTalkPanel.SetActive(false);
            }
            canTalk = false;
            if (dialougeBoxOpen)
            {
                EndDialouge();
            }
        }
        if (canTalk)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (!dialougeBoxOpen)
                {
                    CreateDialougBox();
                    Debug.Log("dialouge box open");
                }

            }

        }
        {
            if (dialougeBoxQuery && (dialougeBoxQueryTime < (Time.time - 0.2f)))
            {
                dialougeBoxQuery = false;
                canContinueDialouge = true;
                print(Time.time);
            }
        }
    }
    public void ContinueDialouge()
    {
        if (canContinueDialouge)
        {
            if(stageOfConvo == 0)
            {
                currentSentenceIndex++;
                if(currentSentenceIndex >= currentQuest.sentencesBeforeQuest.Length)
                {
                    stageOfConvo = 1;
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars(currentQuest.sentencesBeforeQuest[currentSentenceIndex], overlayMainText));
                    canContinueDialouge = false;
                    dialougeBoxQuery = true;
                    dialougeBoxQueryTime = Time.time;
                    return;
                }
            }
            if(stageOfConvo == 1)
            {

            }

            
            
            
            
            
            
            
            
            
            
            
            
        /*
            currentSentenceIndex++;
            if (currentSentenceIndex == dialogue.sentences.Length)
            {
                EndDialouge();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(AddChars(dialogue.sentences[currentSentenceIndex], overlayMainText));
                canContinueDialouge = false;
                dialougeBoxQuery = true;
                dialougeBoxQueryTime = Time.time;

            }
            */
        }
    }

    private void CreateDialougBox()
    {
        dialougeBoxOpen = true;
        overlayPanel.SetActive(true);
        overlayNameText.text = nameOfNpc;
        stageOfConvo = 0;
        currentSentenceIndex = 0;
        isTalking = true;
        canContinueDialouge = false;
        dialougeBoxQuery = true;
        dialougeBoxQueryTime = Time.time;

        StartCoroutine(AddChars(currentQuest.sentencesBeforeQuest[0], overlayMainText));

    }

    private void EndDialouge()
    {
        StopAllCoroutines();
        dialougeBoxOpen = false;
        overlayPanel.SetActive(false);
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
