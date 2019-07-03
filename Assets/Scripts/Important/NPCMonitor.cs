using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using TMPro;

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

        //If there are no more sentences for the NPC to say then the NPC will say the waiting plan       
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
            canContinueDialouge = false;
            dialogueBoxQuery = true;
            dialogueBoxQueryTime = Time.time;
            //Stage of Convo 0 is running through the sentences before quest 1st and 2nd
            if (stageOfConvo == 0)
            {
                currentSentenceIndex++;
                if (hasTalkedBefore)
                {
                    //If there are no more lines to say the NPC will move to the next section
                    if (currentSentenceIndex >= currentQuest.sentencesBeforeQuest2ndTime.Length)
                    {
                        stageOfConvo = 1;
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
                //If the quest is a fetch quest and is complete it tells the player to give it to them 
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
                //If the quest is a kill enemies or level up quest
                else if(hasTalkedBefore && (currentQuest.killEnemies || currentQuest.levelUp))
                {
                    //For kill enemies quest
                    if(currentQuest.killEnemies)
                    {
                        //If the player has killed enough enemies yet tells them to retreive their reward
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
                        //If the player hasn't killed enough enemies yet tells them who and how many to kill
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
                    //If it is a level up quest and it is complete asks the player to collect their reward
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
                    //If neither are complete then state what they need to do
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
                //Runs the first time we reach this stage for a quest
                else
                {
                    //Assigns the reward for the quest
                    if (currentQuest.giveWeapon)
                    {
                        currentQuest.questReward = currentQuest.weaponType;
                    }
                    
                    if (currentQuest.killEnemies) 
                    {
                        //Makes sure the enemy is in the dictionary containing the amount of enemies killed so it does not cause error later
                        if (!PersistantGameManager.Instance.currentEnemyKills.ContainsKey(currentQuest.enemyToKill))
                        {
                            PersistantGameManager.Instance.currentEnemyKills.Add(currentQuest.enemyToKill, 0);
                        }
                        //Sets the intial amount of enemies to kill to the amount so it can work out how many enemies have been killed scince starting this quest
                        currentQuest.initialEnemiesKilled = PersistantGameManager.Instance.currentEnemyKills[currentQuest.enemyToKill];
                    }
                    //Calculates how much experenice to give the player at the end of the quest
                    currentQuest.questExperience = (float)(1.1f * (0.04 * Math.Pow(currentQuest.levelClaimedAt, 3) + (0.8 * Math.Pow(currentQuest.levelClaimedAt, 2) + 100))) * currentQuest.XPMultiplier;
                    hasTalkedBefore = true;
                    StopAllCoroutines();
                    //Gives the quest statment
                    StartCoroutine(AddChars(currentQuest.questStatment, overlayMainText));
                    //Sets the output in the reward slot to the reward
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

                    //If quest has not been submited as an active quest then it submits it
                    if (!PersistantGameManager.Instance.activeQuests.Contains(currentQuest.questKey))
                    {
                        PersistantGameManager.Instance.activeQuests.Add(currentQuest.questKey);
                        PersistantGameManager.Instance.possibleQuests.Add(currentQuest.questKey, currentQuest);
                        TextMeshProUGUI text = GameObject.FindGameObjectWithTag("Updates").GetComponent<TextMeshProUGUI>();
                        if (!currentQuest.instantComplete)
                        {
                            text.text = "Started " + currentQuest.questName;
                        }

                    }
                    
                    //Opens the correct buttons
                    //0 is normal, 1 is to complete a quest and 3 is for instant complete quests
                    if (currentQuest.instantComplete == true)
                    {
                        OpenNewButtons(3);
                    }
                    else if (currentQuest.killEnemies == true && PersistantGameManager.Instance.currentEnemyKills[currentQuest.enemyToKill] >= (currentQuest.killRequirement + currentQuest.initialEnemiesKilled))
                    {
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
            //Gives the closing statements
            if(stageOfConvo == 2)
            {
                //If the quest is complete then give the final closing statement
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
                    //Otherwise give the normal closer
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
                //If there are no more quests
                else if(currentQuest == null)
                {
                    if(currentSentenceIndex == 1)
                    {
                        EndDialogue();
                    }
                    return;
                }
                //Gives normal closer
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

    //Sets buttons to defualt
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
    //Gives the reward for completing a quest
    public void GiveItem()
    {
        //Gives a weapon
        if (currentQuest.giveWeapon == true)
        {
            GameObject questDrop = Instantiate(Resources.Load("Loot Drop"), player.transform.position, Quaternion.identity) as GameObject;
            LootDropMonitor questDropMonitor = questDrop.GetComponent<LootDropMonitor>();

            questDropMonitor.type = 0;
            questDropMonitor.itemStats = LootManager.GenerateSpecificWeapon(currentQuest.weaponType, currentQuest.weaponValue);

        }
        //Give an Item
        if (currentQuest.giveItem == true)
        {
            GameObject questDrop = Instantiate(Resources.Load("Loot Drop"), player.transform.position, Quaternion.identity) as GameObject;
            LootDropMonitor questDropMonitor = questDrop.GetComponent<LootDropMonitor>();

            questDropMonitor.type = 2;
            questDropMonitor.item = currentQuest.itemName;
        }
        //Gives Xp for quest
        PersistantGameManager.Instance.playerStats.playerExperience += currentQuest.questExperience;
        //Removes item from inventory
        if (!currentQuest.instantComplete && currentQuest.returnItem)
        {
            PersistantGameManager.Instance.itemInventory[currentQuest.questItemName]--;
        }
        //Tells PGM to move onto the next quest
        PersistantGameManager.Instance.characterQuests[nameOfNpc]++;

        //Removes quest from PGM
        PersistantGameManager.Instance.activeQuests.Remove(currentQuest.questKey);
        PersistantGameManager.Instance.possibleQuests.Remove(currentQuest.questKey);
        PersistantGameManager.Instance.completedQuests.Add(currentQuest.questKey);

        //Tells player they have completed the quest
        TextMeshProUGUI text = GameObject.FindGameObjectWithTag("Updates").GetComponent<TextMeshProUGUI>();
        if (!currentQuest.instantComplete)
        {
            text.text = "Completed " + currentQuest.questName;
        }

        hasTalkedBefore = false;
        currentQuest = null;
        EndDialogue();

        //If instant complete then loop to next dialogue
        if (mGiveItem)
        {
            StartCoroutine(CreateDialogueBox());
            mGiveItem = false;
        }



    }

}
