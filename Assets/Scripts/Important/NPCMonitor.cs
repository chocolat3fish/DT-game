using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

//******MUST BE CHILDED TO A GAMEOBJECT ACTING AS A NPC IT MUST HAVE A TO TALK CANVAS******\\

//A script that monitors an npc and opens certain canvas at the right time as well as controlling dialouge
public class NPCMonitor : MonoBehaviour
{  
    //If the player has talked to npc about a certian quest before
    private bool hasTalkedBefore;

    //Stage 0 = Introductory sentences, Stage 1 = Quest statement, Stage 2 = Quest done
    private int stageOfConvo;

    //Important quest things
    public string nameOfNpc;
    public string waitingText;
    public Quest[] characterQuests;
    [HideInInspector]
    public Quest currentQuest;
    [HideInInspector]
    public bool isTalking, canContinueDialouge;

    //Misc
    private GameObject player;
    private float dialogueBoxQueryTime;
    private bool dialogueBoxQuery;
    private bool canTalk;
    private bool dialougeBoxOpen;
    private int currentSentenceIndex;
    private bool mGiveItem;

    //UI elements
    private GameObject overlayPanel;
    public GameObject toTalkPanel;

    private Text overlayMainText;
    private Button overlayContinueButton;
    private Text overlayNameText;
    private Text overlayRewardText;
    private Button overlayAcceptButton;
    private Button overlayInstantCompleteButton;

    private void Awake()
    {
        //Asigns Panels and Player
        toTalkPanel = transform.Find("ToTalkPanel").gameObject;
        player = FindObjectOfType<PlayerControls>().gameObject;
        //Sets everything to its starting state
        toTalkPanel.SetActive(false);
        dialougeBoxOpen = false;
    }

    private void Update()
    {
        #region Distance from Player Controls
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
                EndDialogue();
            }
        }
        #endregion

        #region Start Dialogue with 'M'
        if (canTalk)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if(mGiveItem)
                {
                    GiveItem();
                }
                else if (!dialougeBoxOpen)
                {
                    StartCoroutine(CreateDialogueBox());
                }
                else if (dialougeBoxOpen && canContinueDialouge)
                {
                    ContinueDialogue();
                }
            }
        }
        #endregion

        #region Reset DialogueBoxQuery times 
        if (dialogueBoxQuery && (dialogueBoxQueryTime < (Time.time - 0.5f)))
        {
            dialogueBoxQuery = false;
            canContinueDialouge = true;
        }
        #endregion

    }

    private IEnumerator CreateDialogueBox()
    {
        //If the NPC hasn't communicated with PGM before it adds it to the dictionary controlling the quest the enemy is up to
        if (!PersistantGameManager.Instance.characterQuests.ContainsKey(nameOfNpc))
            PersistantGameManager.Instance.characterQuests.Add(nameOfNpc, 0);

        //Set the current quest
        //If there are more quests left to do set it to the correct one
        if (characterQuests.Length - 1 >= PersistantGameManager.Instance.characterQuests[nameOfNpc])
        {
            //If the quest it already active it retreives it from PGM otherwise just set the quest to the next one
            if (PersistantGameManager.Instance.activeQuests.Contains(characterQuests[PersistantGameManager.Instance.characterQuests[nameOfNpc]].questKey))
            {
                currentQuest = PersistantGameManager.Instance.possibleQuests[characterQuests[PersistantGameManager.Instance.characterQuests[nameOfNpc]].questKey];
                hasTalkedBefore = true;
            }
            else
            {
                currentQuest = characterQuests[PersistantGameManager.Instance.characterQuests[nameOfNpc]];
            }
            //Tells the dialogue button, etc what npc is talking
            PersistantGameManager.Instance.currentDialogueQuest = currentQuest;
        }
        //Otherwise it sets the the current quest to nothing
        else
        {
            currentQuest = null;
            //Tells the dialogue button, etc which npc is talking
            PersistantGameManager.Instance.currentDialogueQuest.questKey = nameOfNpc;
        }

        #region Load and assign the dialogue canvas and the text items within it
        AsyncOperation loadDialogueCanvas = SceneManager.LoadSceneAsync("Dialogue Canvas", LoadSceneMode.Additive);
        while (true)
        {
            if (loadDialogueCanvas.isDone)
            {
                Canvas[] canvases = FindObjectsOfType<Canvas>();
                foreach (Canvas canvas in canvases)
                {
                    if (canvas.gameObject.name == "Dialogue Canvas")
                    {
                        overlayPanel = canvas.gameObject.transform.Find("Panel").gameObject;
                        break;
                    }
                }
                overlayMainText = overlayPanel.transform.Find("Text").GetComponent<Text>();
                overlayContinueButton = overlayPanel.transform.Find("Continue").GetComponent<Button>();
                overlayNameText = overlayPanel.transform.Find("Name").Find("Text").GetComponent<Text>();
                overlayAcceptButton = overlayPanel.transform.Find("GiveItem").GetComponent<Button>();
                overlayInstantCompleteButton = overlayPanel.transform.Find("InstantComplete").GetComponent<Button>();
                overlayRewardText = overlayPanel.transform.Find("RewardText").GetComponent<Text>();
                dialougeBoxOpen = true;
                break;
            }
            else
            {
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
        #endregion

        //Sets variables to nessecary values for starting dialogue
        stageOfConvo = 0;
        currentSentenceIndex = 0;
        isTalking = true;
        canContinueDialouge = false;
        dialogueBoxQuery = true;
        dialogueBoxQueryTime = Time.time;
        PersistantGameManager.Instance.dialogueSceneIsOpen = true;

        //Sets dialogue canvas to the nessecary state
        overlayNameText.text = nameOfNpc;
        overlayRewardText.text = "";
        overlayAcceptButton.gameObject.SetActive(false);
        overlayContinueButton.gameObject.SetActive(true);
        overlayInstantCompleteButton.gameObject.SetActive(false);
        
        //Make sure that PGM contains the enemy in the dictionary that keeps track of how many enemies are killed 
        if (currentQuest != null)
        {
            if (currentQuest.killEnemies)
            {
                if (!PersistantGameManager.Instance.currentEnemyKills.ContainsKey(currentQuest.enemyToKill))
                {
                    PersistantGameManager.Instance.currentEnemyKills.Add(currentQuest.enemyToKill, 0);
                }
            }
        }

        //If there is no more quest it shows the waiting text
        if (currentQuest == null)
        {
            StartCoroutine(AddChars(waitingText, overlayMainText));
            stageOfConvo = 3;
            currentSentenceIndex = 1;
            canContinueDialouge = false;
        }

        //If you have already talked to the npc about a quest runs sentencesBeforeQuest2ndTime
        else if (hasTalkedBefore)
        {
            StartCoroutine(AddChars(currentQuest.sentencesBeforeQuest2ndTime[0], overlayMainText));
        }

        //Run the normal respone 
        else
        {
            try
            {
                StartCoroutine(AddChars(currentQuest.sentencesBeforeQuest1stTime[0], overlayMainText));
                currentQuest.levelClaimedAt = PersistantGameManager.Instance.playerStats.playerLevel;
            }
            catch (NullReferenceException)
            {
                StartCoroutine(AddChars(waitingText, overlayMainText));
                stageOfConvo = 3;
                currentSentenceIndex = 1;
                canContinueDialouge = false;
            }

        }
    }

    //Display the Next Sentence + logic for quests
    public void ContinueDialogue()
    {
        //If the quest has been removed end the dialogue
        if(currentQuest == null && stageOfConvo != 3)
        {
            EndDialogue();
            currentSentenceIndex = 0;
            return;
        }
        else if(stageOfConvo == 3)
        {
            if(currentSentenceIndex == 1)
            {
                EndDialogue();
                return;
            }
            currentSentenceIndex = 1;
            return;
        }
        //Runs if player is allowed to continue
        else if (canContinueDialouge)
        {
            print("Continue Dialogue: " + stageOfConvo);
            canContinueDialouge = false;
            dialogueBoxQuery = true;
            dialogueBoxQueryTime = Time.time;
            //Stage of Convo 0 is running through the sentences before quest 1st and 2nd
            if (stageOfConvo == 0)
            {
                currentSentenceIndex++;
                if (hasTalkedBefore)
                {
                    if (currentSentenceIndex >= currentQuest.sentencesBeforeQuest2ndTime.Length)
                    {

                        stageOfConvo = 1;
                        print("Done");

                    }
                    else
                    {
                        StopAllCoroutines();
                        StartCoroutine(AddChars(currentQuest.sentencesBeforeQuest2ndTime[currentSentenceIndex], overlayMainText));
                        canContinueDialouge = false;
                        dialogueBoxQuery = true;
                        dialogueBoxQueryTime = Time.time;
                        return;
                    }
                }
                else
                {
                    if (currentSentenceIndex >= currentQuest.sentencesBeforeQuest1stTime.Length)
                    {
                        stageOfConvo = 1;
                    }
                    else
                    {
                        StopAllCoroutines();
                        StartCoroutine(AddChars(currentQuest.sentencesBeforeQuest1stTime[currentSentenceIndex], overlayMainText));
                        canContinueDialouge = false;
                        dialogueBoxQuery = true;
                        dialogueBoxQueryTime = Time.time;
                        return;
                    }
                }
            }
            //Gives the main quest statment
            if(stageOfConvo == 1)
            {
                /*
                if (hasTalkedBefore && currentQuest.instantComplete)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars(currentQuest.questStatment, overlayMainText));
                    if (currentQuest.giveItem == true)
                    {
                        currentQuest.questReward = currentQuest.itemName;
                        StartCoroutine(AddChars(currentQuest.questReward, overlayRewardText));
                    }


                    stageOfConvo = 2;
                    currentSentenceIndex = -1;
                    OpenNewButtons(1);
                    return;

                }
                else */
                if (hasTalkedBefore && currentQuest.returnItem && PersistantGameManager.Instance.itemInventory[currentQuest.questItemName] > 0)                
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars(currentQuest.questStatmentWithItem, overlayMainText));
                    if (currentQuest.questReward == "")
                    {
                        StartCoroutine(AddChars(currentQuest.questExperience + " XP", overlayRewardText));
                        stageOfConvo = 2;
                        currentSentenceIndex = -1;
                    }
                    else
                    {
                        StartCoroutine(AddChars(currentQuest.questReward + ", " + currentQuest.questExperience + " XP", overlayRewardText));
                    }
                    stageOfConvo = 2;
                    currentSentenceIndex = -1;
                    OpenNewButtons(1);
                    return;

                }
                else if(hasTalkedBefore && (currentQuest.killEnemies || currentQuest.levelUp))
                {
                    if(currentQuest.killEnemies)
                    {
                        if (((currentQuest.killRequirement - (PersistantGameManager.Instance.currentEnemyKills[currentQuest.enemyToKill] - currentQuest.initialEnemiesKilled) <= 0)))
                        {
                            StopAllCoroutines();
                            StartCoroutine(AddChars(currentQuest.questStatmentWithItem, overlayMainText));
                            if (currentQuest.questReward == "")
                            {
                                StartCoroutine(AddChars(currentQuest.questExperience + " XP", overlayRewardText));
                                stageOfConvo = 2;
                                currentSentenceIndex = -1;
                            }
                            else
                            {
                                StartCoroutine(AddChars(currentQuest.questReward + ", " + currentQuest.questExperience + " XP", overlayRewardText));
                            }
                            stageOfConvo = 2;
                            currentSentenceIndex = -1;
                            OpenNewButtons(1);
                            return;
                        }
                        else
                        {
                            StopAllCoroutines();
                            StartCoroutine(AddChars(currentQuest.questStatment, overlayMainText));
                            if (currentQuest.questReward == "")
                            {
                                StartCoroutine(AddChars(currentQuest.questExperience + " XP", overlayRewardText));
                                stageOfConvo = 2;
                                currentSentenceIndex = -1;
                            }
                            else
                            {
                                StartCoroutine(AddChars(currentQuest.questReward + ", " + currentQuest.questExperience + " XP", overlayRewardText));
                                stageOfConvo = 2;
                                currentSentenceIndex = -1;
                            }
                            return;
                        }
                    }
                    else if (currentQuest.levelUp == true && PersistantGameManager.Instance.playerStats.playerLevel >= currentQuest.levelToReach)
                    {
                        StopAllCoroutines();
                        StartCoroutine(AddChars(currentQuest.questStatmentWithItem, overlayMainText));
                        if (currentQuest.questReward == "")
                        {
                            StartCoroutine(AddChars(currentQuest.questExperience + " XP", overlayRewardText));
                            stageOfConvo = 2;
                            currentSentenceIndex = -1;
                        }
                        else
                        {
                            StartCoroutine(AddChars(currentQuest.questReward + ", " + currentQuest.questExperience + " XP", overlayRewardText));
                        }
                        stageOfConvo = 2;
                        currentSentenceIndex = -1;
                        OpenNewButtons(1);
                        return;

                    }
                    else
                    {
                        StopAllCoroutines();
                        StartCoroutine(AddChars(currentQuest.questStatment, overlayMainText));
                        if (currentQuest.questReward == "")
                        {
                            StartCoroutine(AddChars(currentQuest.questExperience + " XP", overlayRewardText));
                            stageOfConvo = 2;
                            currentSentenceIndex = -1;
                        }
                        else
                        {
                            StartCoroutine(AddChars(currentQuest.questReward + ", " + currentQuest.questExperience + " XP", overlayRewardText));
                            stageOfConvo = 2;
                            currentSentenceIndex = -1;
                        }
                        return;
                    }


                }
                else
                {
                    if (currentQuest.giveWeapon == true)
                    {
                        currentQuest.questReward = currentQuest.weaponType;
                    }

                    if (currentQuest.killEnemies == true) 
                    {
                        if(!PersistantGameManager.Instance.currentEnemyKills.ContainsKey(currentQuest.enemyToKill))
                        {
                            PersistantGameManager.Instance.currentEnemyKills.Add(currentQuest.enemyToKill, 0);
                        }
                        currentQuest.initialEnemiesKilled = PersistantGameManager.Instance.currentEnemyKills[currentQuest.enemyToKill];
                        //Debug.Log(currentQuest.initialEnemiesKilled);
                    }
                    //this is bad code
                    currentQuest.questExperience = (float)(1.1f * (0.04 * Math.Pow(currentQuest.levelClaimedAt, 3) + (0.8 * Math.Pow(currentQuest.levelClaimedAt, 2) + 100))) * currentQuest.XPMultiplier;
                    StopAllCoroutines();
                    hasTalkedBefore = true;
                    StartCoroutine(AddChars(currentQuest.questStatment, overlayMainText));
                    if (currentQuest.questReward == "")
                    {
                        StartCoroutine(AddChars(currentQuest.questExperience + " XP", overlayRewardText));
                    }
                    else
                    {
                        StartCoroutine(AddChars(currentQuest.questReward + ", " + currentQuest.questExperience + " XP", overlayRewardText));
                    }
                    stageOfConvo = 2;
                    currentSentenceIndex = -1;


                    if (PersistantGameManager.Instance.activeQuests.Contains(currentQuest.questKey) == false)
                    {
                        PersistantGameManager.Instance.activeQuests.Add(currentQuest.questKey);
                        PersistantGameManager.Instance.possibleQuests.Add(currentQuest.questKey, currentQuest);
                    }
                    /*
                    Debug.Log("Killed Prior: " + currentQuest.initialEnemiesKilled);
                    Debug.Log("Needed: " + (currentQuest.killRequirement - (PersistantGameManager.Instance.currentEnemyKills[currentQuest.enemyToKill] - currentQuest.initialEnemiesKilled)) + " Killed: " + PersistantGameManager.Instance.currentEnemyKills[currentQuest.enemyToKill]);
                    */                   
                    if (currentQuest.instantComplete == true)
                    {
                        OpenNewButtons(3);
                    }
                    else if (currentQuest.killEnemies == true && PersistantGameManager.Instance.currentEnemyKills[currentQuest.enemyToKill] >= (currentQuest.killRequirement + currentQuest.initialEnemiesKilled))
                    {
                        //Debug.Log("Passed");
                        OpenNewButtons(1);
                    }
                    else if (currentQuest.returnItem == true && PersistantGameManager.Instance.itemInventory[currentQuest.questItemName] > 0)
                    {
                        OpenNewButtons(1);
                    }
                    else
                    {

                        OpenNewButtons(0);
                    }
                    return;
                }
            }

            if(stageOfConvo == 2)
            {
                print(currentQuest == null);
                if (characterQuests.Length - 1 >= PersistantGameManager.Instance.characterQuests[nameOfNpc])
                {
                    if (characterQuests[PersistantGameManager.Instance.characterQuests[nameOfNpc]].questKey != currentQuest.questKey)
                    {
                        overlayRewardText.text = "";
                        currentSentenceIndex++;
                        CloseNewButtons();
                        if (currentSentenceIndex >= currentQuest.sentencesAfterQuestEnd.Length)
                        {
                            EndDialogue();
                            return;
                        }
                        else
                        {
                            StopAllCoroutines();
                            StartCoroutine(AddChars(currentQuest.sentencesAfterQuestEnd[currentSentenceIndex], overlayMainText));
                        }
                    }
                    else
                    {
                        overlayRewardText.text = "";
                        currentSentenceIndex++;
                        CloseNewButtons();
                        if (currentSentenceIndex >= currentQuest.sentencesAfterQuest.Length)
                        {
                            EndDialogue();
                            return;
                        }
                        else
                        {
                            StopAllCoroutines();
                            StartCoroutine(AddChars(currentQuest.sentencesAfterQuest[currentSentenceIndex], overlayMainText));
                        }
                    }
                }
                else if(currentQuest == null)
                {
                    if(currentSentenceIndex == 1)
                    {
                        EndDialogue();
                    }
                    return;
                }
                else
                {
                    overlayRewardText.text = "";
                    currentSentenceIndex++;
                    CloseNewButtons();
                    if (currentSentenceIndex >= currentQuest.sentencesAfterQuestEnd.Length)
                    {
                        EndDialogue();
                        return;
                    }
                    else
                    {
                        StopAllCoroutines();
                        StartCoroutine(AddChars(currentQuest.sentencesAfterQuestEnd[currentSentenceIndex], overlayMainText));
                    }

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
        switch (num)
        {
            //Opens buttons if the player can just continue the quest
            case 0:
                overlayContinueButton.gameObject.SetActive(true);
                overlayAcceptButton.gameObject.SetActive(false);
                break;

            //Opens Butons if the player can complete the quest
            case 1:
                overlayContinueButton.gameObject.SetActive(true);
                overlayAcceptButton.gameObject.SetActive(true);
                break;

            //Opens buttons for Instant Complete Quests
            case 3:
                mGiveItem = true;
                overlayContinueButton.gameObject.SetActive(false);
                overlayContinueButton.gameObject.SetActive(false);
                overlayInstantCompleteButton.gameObject.SetActive(true);
                break;
        }
    }

    private void CloseNewButtons()
    {
        mGiveItem = false;
        overlayAcceptButton.gameObject.SetActive(false);
        overlayContinueButton.gameObject.SetActive(true);
        overlayInstantCompleteButton.gameObject.SetActive(false);
    }



    private void EndDialogue()
    {
        StopAllCoroutines();
        SceneManager.UnloadSceneAsync("Dialogue Canvas");
        dialougeBoxOpen = false;
        isTalking = false;
        PersistantGameManager.Instance.dialogueSceneIsOpen = false;
    }

    IEnumerator AddChars(string sentence, Text text)
    {
        text.text = "";
        char previousChar = "a"[0];
        bool shouldWait = false;
        foreach (char ch in sentence)
        {
            if(ch == "/"[0])
            {
                previousChar = ch;
                continue;
            }
            if(ch == "w"[0] && previousChar == "/"[0])
            {
                yield return new WaitForSecondsRealtime(0.5f);
                previousChar = "a"[0];
                shouldWait = true;
                continue;
            }
            previousChar = ch;
            text.text += ch;
            yield return null;
            if (shouldWait)
            {
                yield return null;
            }
            
        }
    }
    public void GiveItem()
    {
        /*
        if(PersistantGameManager.Instance.itemInventory[currentQuest.questItemName] > 0)
        {
            GiveReward(currentQuest.questKey);
        }
        */
        if (currentQuest.giveWeapon == true)
        {
            GameObject questDrop = Instantiate(Resources.Load("Loot Drop"), player.transform.position, Quaternion.identity) as GameObject;
            LootDropMonitor questDropMonitor = questDrop.GetComponent<LootDropMonitor>();

            questDropMonitor.type = 0;
            //questDropMonitor.item = PersistantGameManager.Instance.questTargets[nPCMonitor.currentQuest.questKey];
            questDropMonitor.itemStats = LootManager.GenerateSpecificWeapon(currentQuest.weaponType, currentQuest.weaponValue);

        }
        if (currentQuest.giveItem == true)
        {
            GameObject questDrop = Instantiate(Resources.Load("Loot Drop"), player.transform.position, Quaternion.identity) as GameObject;
            LootDropMonitor questDropMonitor = questDrop.GetComponent<LootDropMonitor>();

            questDropMonitor.type = 2;
            //questDropMonitor.item = PersistantGameManager.Instance.questTargets[nPCMonitor.currentQuest.questKey];
            questDropMonitor.item = currentQuest.itemName;
        }

        PersistantGameManager.Instance.playerStats.playerExperience += PersistantGameManager.Instance.totalExperience / 4;
        if (!currentQuest.instantComplete && currentQuest.returnItem)
        {
            PersistantGameManager.Instance.itemInventory[currentQuest.questItemName]--;
        }

        PersistantGameManager.Instance.characterQuests[nameOfNpc]++;

        PersistantGameManager.Instance.activeQuests.Remove(currentQuest.questKey);
        PersistantGameManager.Instance.possibleQuests.Remove(currentQuest.questKey);
        PersistantGameManager.Instance.completedQuests.Add(currentQuest.questKey);
        hasTalkedBefore = false;
        currentQuest = null;
        EndDialogue();
        if (mGiveItem)
        {
            StartCoroutine(CreateDialogueBox());
            mGiveItem = false;
        }



    }
    /*
    public void GiveReward(string key)
    {
        if (key == "Ja00")
        {
            ReceiveWeapon("Short Sword", 10);
            PersistantGameManager.Instance.playerStats.playerExperience += PersistantGameManager.Instance.totalExperience / 4;
            PersistantGameManager.Instance.itemInventory["Claw of Straphagus"] --;

           
        }

        if (key == "Ja01")
        {
            ReceiveWeapon("Lance", 10);
            PersistantGameManager.Instance.playerStats.playerExperience += PersistantGameManager.Instance.totalExperience / 4;
            PersistantGameManager.Instance.itemInventory["Amulet of Honour"]--;

        }

        if (key == "Ja03")
        {
            ReceiveWeapon("Long Sword", 10);
            PersistantGameManager.Instance.playerStats.playerExperience += PersistantGameManager.Instance.totalExperience / 4;
        }


        PersistantGameManager.Instance.characterQuests[nameOfNpc]++;
        
        PersistantGameManager.Instance.activeQuests.Remove(key);
        PersistantGameManager.Instance.possibleQuests.Remove(key);
        PersistantGameManager.Instance.completedQuests.Add(currentQuest.questKey);
        hasTalkedBefore = false;
    }
    */

    public void ReceiveWeapon(string weaponType, int weaponValue)
    {
        GameObject questDrop = Instantiate(Resources.Load("Loot Drop"), player.transform.position, Quaternion.identity) as GameObject;
        LootDropMonitor questDropMonitor = questDrop.GetComponent<LootDropMonitor>();

        questDropMonitor.type = 0;
        //questDropMonitor.item = PersistantGameManager.Instance.questTargets[nPCMonitor.currentQuest.questKey];
        questDropMonitor.itemStats = LootManager.GenerateSpecificWeapon(weaponType, weaponValue);
    }

}
