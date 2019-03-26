using System.Collections;
using System.Collections.Generic;
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
    public Quest[] characterQuests;
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
    private Text overlayRewardText;
    private Button overlayAcceptButton;

    public Dictionary<string, string> rewards = new Dictionary<string, string>()
    {
        {"Ja00", "Reward: A 100% attack potion"},
        {"Ja01", "Reward: A 20% Leech potion"}
    };

    private bool reciveInput;
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
        overlayAcceptButton = overlayPanel.transform.Find("GiveItem").GetComponent<Button>();
        overlayRewardText = overlayPanel.transform.Find("RewardText").GetComponent<Text>();
    }

    private void Start()
    {
        overlayPanel.SetActive(false);
        toTalkPanel.SetActive(false);
        dialougeBoxOpen = false;

        

        overlayMainText = overlayPanel.transform.Find("Text").GetComponent<Text>();
        overlayContinueButton = overlayPanel.transform.Find("Continue").GetComponent<Button>();
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
                    CreateDialogueBox();
                    Debug.Log("dialouge box open");
                }
                else if (dialougeBoxOpen && canContinueDialouge)
                {
                    ContinueDialouge();
                }

            }

        }
        {
            if (dialougeBoxQuery && (dialougeBoxQueryTime < (Time.time - 0.5f)))
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
            canContinueDialouge = false;
            dialougeBoxQuery = true;
            dialougeBoxQueryTime = Time.time;
            if (stageOfConvo == 0)
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
                StopAllCoroutines();
                StartCoroutine(AddChars(currentQuest.questStatment, overlayMainText));
                StartCoroutine(AddChars(rewards[currentQuest.rewardKey], overlayRewardText));
                reciveInput = true;
                stageOfConvo = 2;
                currentSentenceIndex = -1;
                if(PersistantGameManager.Instance.itemInventory[currentQuest.questItemName] > 0)
                {
                    OpenNewButtons(1);
                }
                else
                {
                    OpenNewButtons(0);
                }

                return;
            }
            if(stageOfConvo == 2)
            {
                overlayRewardText.text = "";
                currentSentenceIndex++;
                CloseNewButtons();
                if(currentSentenceIndex >= currentQuest.sentencesAfterQuest.Length)
                {
                    EndDialouge();
                    return;
                }
                else 
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars(currentQuest.sentencesAfterQuest[currentSentenceIndex], overlayMainText));
                }



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

    private void OpenNewButtons(int num)
    {
        if(num == 0)
        {
            overlayContinueButton.gameObject.SetActive(true);
        }
        if (num == 1)
        {
            overlayContinueButton.gameObject.SetActive(true);
            overlayAcceptButton.gameObject.SetActive(true);
        }


    }
    private void CloseNewButtons()
    {
        overlayAcceptButton.gameObject.SetActive(false);
        overlayContinueButton.gameObject.SetActive(true);
    }
    private void CreateDialogueBox()
    {
        currentQuest = characterQuests[PersistantGameManager.Instance.characterQuests[nameOfNpc]];
        dialougeBoxOpen = true;
        overlayPanel.SetActive(true);
        overlayNameText.text = nameOfNpc;
        stageOfConvo = 0;
        currentSentenceIndex = 0;
        isTalking = true;
        canContinueDialouge = false;
        dialougeBoxQuery = true;
        dialougeBoxQueryTime = Time.time;
        overlayAcceptButton.gameObject.SetActive(false);
        overlayContinueButton.gameObject.SetActive(true);
        overlayRewardText.text = "";
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
    public void GiveItem()
    {
        if(PersistantGameManager.Instance.itemInventory[currentQuest.questItemName] > 0)
        {
            GiveReward(currentQuest.rewardKey);
        }
        ContinueDialouge();
    }

    public void GiveReward(string key)
    {
        if (key == "Ja00")
        {
            PersistantGameManager.Instance.itemInventory["Claw of Straphagus"] --;
            PersistantGameManager.Instance.amountOfConsumables["100%A"]++;
            PersistantGameManager.Instance.characterQuests["Jason"]++;
        }

        if (key == "Ja01")
        {
            PersistantGameManager.Instance.itemInventory["Amulet of Honour"]--;
            PersistantGameManager.Instance.amountOfConsumables["20%L"]++;
        }
    }
}
