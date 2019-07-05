using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Controls the dialogue during the tutorial
public class TutorialDialogue : MonoBehaviour
{
    GameObject dialogueCanvas, toTalkPanel;
    public Quest tutorialQuest;
    public Text canvasMainText, canvasNameText, canvasRewardText;
    Button canvasAcceptButton, canvasContinueButton;
    int stageOfConvo, currentSentenceIndex;
    bool isTalking, canContinueDialogue;
    PlayerControls player;

    void Awake()
    {
        
        player = FindObjectOfType<PlayerControls>();

    }
    private void Start()
    {
        StartCoroutine(StartDialogue());
    }
    IEnumerator StartDialogue()
    {
        AsyncOperation loadDialogueCanvas = SceneManager.LoadSceneAsync("Dialogue Canvas", LoadSceneMode.Additive);
        //Loading of the canvas
        while (true)
        {
            if (loadDialogueCanvas.isDone)
            {
                Canvas[] canvases = FindObjectsOfType<Canvas>();
                foreach (Canvas canvas in canvases)
                {
                    if (canvas.gameObject.name == "Dialogue Canvas")
                    {
                        dialogueCanvas = canvas.gameObject.transform.Find("Panel").gameObject;
                        break;
                    }
                    if(canvas.gameObject.name == "ToTalkPanel")
                    {
                        toTalkPanel = canvas.gameObject;              
                    }
                }   
                canvasMainText = dialogueCanvas.transform.Find("Text").GetComponent<Text>();
                canvasContinueButton = dialogueCanvas.transform.Find("Continue").GetComponent<Button>();
                canvasNameText = dialogueCanvas.transform.Find("Name").Find("Text").GetComponent<Text>();
                canvasAcceptButton = dialogueCanvas.transform.Find("GiveItem").GetComponent<Button>();
                canvasRewardText = dialogueCanvas.transform.Find("RewardText").GetComponent<Text>();
                break;
            }
            else
            {
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }

        //Sets defaults
        canvasNameText.text = "Jason";
        stageOfConvo = 0;
        currentSentenceIndex = 0;
        isTalking = true;
        canContinueDialogue = false;
        canvasAcceptButton.gameObject.SetActive(false);
        canvasContinueButton.gameObject.SetActive(true);
        canvasRewardText.text = "";
        PersistantGameManager.Instance.dialogueSceneIsOpen = true;
        toTalkPanel.SetActive(false);
        //Puts the quest in the active quest menu
        if (!PersistantGameManager.Instance.activeQuests.Contains(tutorialQuest.questKey))
        {
            PersistantGameManager.Instance.activeQuests.Add(tutorialQuest.questKey);
            PersistantGameManager.Instance.possibleQuests.Add(tutorialQuest.questKey, tutorialQuest);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        StopAllCoroutines();
        StartCoroutine(AddChars("Hello\nMy name is Jason, you've been asleep for a while./d", canvasMainText));
    }

    // Update is called once per frame
    void Update()
    {
        //Continues dialogue
        if(Input.GetKeyDown(KeyCode.M) && !(stageOfConvo == 1 && currentSentenceIndex == 0))
        {
            ContinueDialogue();
        }
        //Opens the 'to talk panel' when at a certain stage during the tutorial
        if(stageOfConvo == 1 && currentSentenceIndex == 0)
        {
            float distance = Vector2.Distance(player.transform.position, transform.position);
            if(distance < 2)
            {
                toTalkPanel.SetActive(true);
            }
            if(distance < 2 && Input.GetKeyDown(KeyCode.M))
            {
                currentSentenceIndex = 1;
                toTalkPanel.SetActive(false);
                ContinueDialogue();
            }
        }
    }

    //Adds the letters to the text specified
    IEnumerator AddChars(string sentence, Text text)
    {
        canContinueDialogue = false;
        text.text = "";
        char previousChar = 'a';
        bool shouldWait = false;
        foreach (char ch in sentence)
        {

            if(previousChar == '/')
            {
                switch(ch.ToString())
                {
                    case "w":
                        yield return new WaitForSecondsRealtime(0.5f);
                        previousChar = 'a';
                        shouldWait = true;
                        continue;

                    case "d":
                        canContinueDialogue = true;
                        continue;

                    case "r":
                        yield return new WaitForSecondsRealtime(1f);
                        text.text = "";
                        continue;
                }
            }
            if (ch == '/')
            {
                previousChar = ch;
                continue;
            }

            previousChar = ch;
            text.text += ch;
            yield return null;
            
            //slows down text
            if (shouldWait)
            {
                print("waiting");
                yield return null;
            }

        }
    }
    //Runs through each sentence moving onto the next one when the player clicks
    //Some will start a co-routine that waits for the player to complete a certian task before conintuing
    public void ContinueDialogue()
    {
        if(canContinueDialogue)
        {
            if(stageOfConvo == 0)
            {
                if(currentSentenceIndex == 0)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars("Let me get you up to speed, you look new here after all./d", canvasMainText));
                    currentSentenceIndex = 1;
                }
                else if(currentSentenceIndex == 1)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars("Using ‘A’ and ‘D’ you can move left and right,\n/wTry that now...", canvasMainText));
                    canvasContinueButton.gameObject.SetActive(false);
                    canContinueDialogue = false;
                    StartCoroutine(WaitForPlayerMove());
                    currentSentenceIndex = 2;

                }
                else if(currentSentenceIndex == 2)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars("Now try a jump, and then a double jump with the space bar...", canvasMainText));
                    canvasContinueButton.gameObject.SetActive(false);
                    canContinueDialogue = false;
                    StartCoroutine(WaitForPlayerJump());
                    currentSentenceIndex = 3;
                }
                else if(currentSentenceIndex == 3)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars("Head to your right and grab that item on the topmost platform, \nUse ‘E’ to interact with items...", canvasMainText));
                    canvasContinueButton.gameObject.SetActive(false);
                    canContinueDialogue = false;
                    StartCoroutine(WaitForPlayerPickup());
                    currentSentenceIndex = 4;
                }
                else if (currentSentenceIndex == 4)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars("Now lets have a look at the items you hold, Click 'tab' to open the menu, And click the [items] button to view your inventory...", canvasMainText));
                    canvasContinueButton.gameObject.SetActive(false);
                    canContinueDialogue = false;
                    StartCoroutine(WaitForPlayerToOpenMenu());
                    currentSentenceIndex = 5;
                }
                else if(currentSentenceIndex == 5)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars("In the menu you can also see your weapons and active quests.../d", canvasMainText));
                    canContinueDialogue = false;
                    currentSentenceIndex = 6;
                }
                else if (currentSentenceIndex == 6)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars("You can look deeper into a quest by clicking on it in the [quests] menu, Give it a go...", canvasMainText));
                    canvasContinueButton.gameObject.SetActive(false);
                    canContinueDialogue = false;
                    StartCoroutine(WaitForPlayerToOpenQuestDesc());
                    currentSentenceIndex = 7;
                }
                else if (currentSentenceIndex == 7)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars("Good job! When you are ready come chat to me.../d", canvasMainText));
                    currentSentenceIndex = 8;
                }
                else if(currentSentenceIndex == 8)
                {
                    dialogueCanvas.SetActive(false);
                    stageOfConvo = 1;
                    currentSentenceIndex = 0;
                }
            }
            if(stageOfConvo == 1)
            {
                if (currentSentenceIndex == 1)
                {
                    dialogueCanvas.SetActive(true);
                    StopAllCoroutines();
                    StartCoroutine(AddChars("If you find others around, this is how you'll greet them. They will give you quests and rewards should you choose to help them./d", canvasMainText));
                    canContinueDialogue = false;
                    currentSentenceIndex = 2;
                }
                else if (currentSentenceIndex == 2)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    StartCoroutine(AddChars("If you're struggling to find your way around, you can use your minimap by pressing 'P'./d", canvasMainText));
                    currentSentenceIndex = 3;
                }
                else if (currentSentenceIndex == 3)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    StartCoroutine(AddChars("It seems like you don't have any weapons. You're going to need one in here./d", canvasMainText));

                    currentSentenceIndex = 4;
                }
                else if (currentSentenceIndex == 4)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    StartCoroutine(AddChars("You'll find a few different types of weapons around, some more effective than others./d", canvasMainText));
                    currentSentenceIndex = 5;
                }
                else if (currentSentenceIndex == 5)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    //Drops jasons dagger
                    GameObject questDrop = Instantiate(Resources.Load("Loot Drop"), player.transform.position, Quaternion.identity) as GameObject;
                    LootDropMonitor questDropMonitor = questDrop.GetComponent<LootDropMonitor>();
                    questDropMonitor.type = 0;
                    questDropMonitor.itemStats = new Weapon("Dagger", "Jason's ", "", "", 5, 0.5f, 0.5f, 1, 1.5f);
                    StartCoroutine(AddChars("Here's my dagger, I don't use it much anyway, Use 'e' to pick up a weapon. Pressing 1, 2 or 3 will change the slot you are comparing and switching into.../d", canvasMainText));
                    currentSentenceIndex = 6;
                }
                else if (currentSentenceIndex == 6)
                {
                    bool hasPickedItUp = false;

                    foreach (Weapon weapon in PersistantGameManager.Instance.playerWeaponInventory)
                    {
                        if (weapon.itemPrefix == "Jason's ")
                        {
                            hasPickedItUp = true;
                            break;
                        }
                    }
                    if (hasPickedItUp)
                    {
                        currentSentenceIndex = 7;
                        ContinueDialogue();
                    }
                    else
                    {
                        StopAllCoroutines();
                        canContinueDialogue = false;
                        canvasContinueButton.gameObject.SetActive(false);
                        StartCoroutine(AddChars("Pick up my dagger, use 'e' and click the [Take] button or 'L' to take the weapon. Pressing the [Keep] button or 'K' will keep your current weapon and leave the new one./d", canvasMainText));

                        StartCoroutine(WaitForPlayerToPickupWeapon());
                        currentSentenceIndex = 7;
                    }
                }
                else if (currentSentenceIndex == 7)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    Instantiate(Resources.Load("Tutorial Enemy"));
                    canvasContinueButton.gameObject.SetActive(false);
                    StartCoroutine(AddChars("You can see the equipped weapon slot in the bottom left of the screen. Pressing 1, 2, or 3 will change the weapon you are using. " +
                    	"\nAn enemy has appeared on the right. Use 'W' to attack, but try not to touch it...", canvasMainText));
                    StartCoroutine(WaitForPlayerToKillEnemy());
                    currentSentenceIndex = 8;
                }
                else if (currentSentenceIndex == 8)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    if (player.currentHealth > player.totalHealth - 0.01f)
                    {
                        StartCoroutine(AddChars("Wow, I'm impressed you did that without taking any damage!.../d", canvasMainText));
                        currentSentenceIndex = 9;
                    }
                    else
                    {
                        canvasContinueButton.gameObject.SetActive(false);
                        StartCoroutine(AddChars("Oosh, you took some hard hits, Just wait a few seconds without taking damage or attacking and your health will soon come back...", canvasMainText));
                        StartCoroutine(WaitForPlayerFullHealth());
                        currentSentenceIndex = 10;
                    }

                }
                else if (currentSentenceIndex == 9)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    StartCoroutine(AddChars("But if you do take damage your health regenerates after a few seconds of no combat.../d", canvasMainText));
                    currentSentenceIndex = 10;
                }
                else if (currentSentenceIndex == 10)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    StartCoroutine(AddChars("This enemy dropped a weapon, as do others. Taking a weapon on the ground will switch it with your old weapon, so whatever you used to have will be on the ground should you change your mind./d ", canvasMainText));
                    currentSentenceIndex = 11;
                }
                else if (currentSentenceIndex == 11)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    StartCoroutine(AddChars("He also gave you some xp, You need a certain amount of xp to level up\nAnd you seem to have leveled up.../d", canvasMainText));
                    currentSentenceIndex = 13;
                }
                /*Obselete
                else if (currentSentenceIndex == 12)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    StartCoroutine(AddChars("Click 'u' to open the player canvas, Then from there click 'k' to open the skills panel, to unlock a skill you must spend a skill point.../d", canvasMainText));
                    currentSentenceIndex = 13;
                }
                */
                else if (currentSentenceIndex == 13)
                {
                    if (PersistantGameManager.Instance.playerStats.playerSkillPoints == 0)
                    {
                        currentSentenceIndex = 14;
                        ContinueDialogue();
                    }
                    if(PersistantGameManager.Instance.playerStats.playerSkillPoints >  0)
                    {
                        canvasContinueButton.gameObject.SetActive(false);

                        canContinueDialogue = false;
                        StopAllCoroutines();
                        StartCoroutine(AddChars("Press 'U' to open the character menu and find the [Skills] button. in that screen can spend skill points to unlock skills. You earned one from levelling up, try spending it now...", canvasMainText));
                        StartCoroutine(WaitForPlayerToUnlockSkill());
                        currentSentenceIndex = 14;

                    }
                }
                else if (currentSentenceIndex == 14)
                {
                    StopAllCoroutines();
                    canContinueDialogue = false;
                    StartCoroutine(AddChars("Every time you level up you get a skill point, you will use these to unlock skills, remember to use them.../d", canvasMainText));
                    currentSentenceIndex = 15;
                }
                else if(currentSentenceIndex == 15)
                {
                    StopAllCoroutines();
                    canContinueDialogue = false;
                    StartCoroutine(AddChars("Just to make sure you remember.../d", canvasMainText));
                    currentSentenceIndex = 16;
                }
                else if(currentSentenceIndex == 16)
                {
                    StopAllCoroutines();
                    canContinueDialogue = false;
                    StartCoroutine(AddChars("You hold 3 weapons, and can switch them with 1, 2, and 3, the order can be arranged in your weapons menu. If you need to save your game or load an old one, click the button in the menu./d", canvasMainText));
                    currentSentenceIndex = 17;
                }
                else if(currentSentenceIndex == 17)
                {
                    StopAllCoroutines();
                    canContinueDialogue = false;
                    StartCoroutine(AddChars("As you explore and complete quests, the world will change around you. Be sure to check for areas you may have not been able to reach before, and to click on strange looking terrain./d", canvasMainText));
                    currentSentenceIndex = 18;
                }
                else if (currentSentenceIndex == 18)
                {
                    StopAllCoroutines();
                    canContinueDialogue = false;
                    canvasContinueButton.gameObject.SetActive(false);
                    StartCoroutine(AddChars("I've taught you all I can. Use [enter] to go through doors. Goodbye", canvasMainText));

                    PersistantGameManager.Instance.activeQuests.Remove("Tutorial");
                    PersistantGameManager.Instance.possibleQuests.Remove("Tutorial");
                    PersistantGameManager.Instance.tutorialComplete = true;
                }



            }
        }

    }
    public void GiveItem()
    {

    }
    /*
    IEnumerator DialogueBoxControl()
    {
        while (true)
        {
            if ((PersistantGameManager.Instance.menuCanvasOpen || PersistantGameManager.Instance.characterScreenOpen || PersistantGameManager.Instance.skillsScreenOpen) && dialogueCanvas.activeSelf)
            {
                Debug.Log("Close");
                dialogueCanvas.SetActive(false);
            }
            else if (!(PersistantGameManager.Instance.menuCanvasOpen || PersistantGameManager.Instance.characterScreenOpen || PersistantGameManager.Instance.skillsScreenOpen) && !dialogueCanvas.activeSelf)
            {
                Debug.Log("Open");
                dialogueCanvas.SetActive(true);
            }
            yield return null;
        }

    }*/
    IEnumerator WaitForPlayerMove()
    {
        Rigidbody2D pRigidbody = player.GetComponent<Rigidbody2D>();
        bool moveLeft = false, moveRight = false;
        while(true)
        {
            if(pRigidbody.velocity.x > 0.01f)
            {
                moveRight = true;
            }
            else if(pRigidbody.velocity.x < -0.01f)
            {
                moveLeft = true;
            }
            if(moveLeft && moveRight)
            {
                canContinueDialogue = true;
                canvasContinueButton.gameObject.SetActive(true);
                break;
            }
            yield return null;
        }
    }
    IEnumerator WaitForPlayerJump()
    {
        Rigidbody2D pRigidbody = player.GetComponent<Rigidbody2D>();
        bool doubleJump = false;
        while (true)
        {
            if (player.currentJumps >= 2)
            {
                doubleJump = true;
            }
            if (doubleJump)
            {
                canContinueDialogue = true;
                canvasContinueButton.gameObject.SetActive(true);
                break;
            }
            yield return null;
        }
    }
    IEnumerator WaitForPlayerPickup()
    {
        bool gotItem = false;
        while (true)
        {
            if (PersistantGameManager.Instance.itemInventory.ContainsKey("Jason's Belt"))
            {
                if (PersistantGameManager.Instance.itemInventory["Jason's Belt"] > 0)
                {
                    gotItem = true;
                }
            }
            if (gotItem)
            {
                canContinueDialogue = true;
                canvasContinueButton.gameObject.SetActive(true);
                break;
            }
            yield return null;
        }
    }
    IEnumerator WaitForPlayerToOpenMenu()
    {
        bool opened = false;
        bool go = true;
        while (go)
        {
            try
            {
                if(FindObjectOfType<MenuCanvasScript>().itemsPanel.activeSelf)
                {
                    opened = true;
                    go = false;
                }
            }
            catch
            {
                go = true;
            }
            yield return null;
        }
        while(true)
        { 
            if (opened && !PersistantGameManager.Instance.menuCanvasOpen)
            {
                canContinueDialogue = true;
                canvasContinueButton.gameObject.SetActive(true);
                break;

            }
            yield return null;

        }
    }
    IEnumerator WaitForPlayerToOpenQuestDesc()
    {
        bool opened = false;
        bool go = true;
        while (go)
        {
            try
            {
                if (FindObjectOfType<MenuCanvasScript>().questDescPanel.activeSelf)
                {
                    opened = true;
                    go = false;
                }
            }
            catch
            {
                go = true;
            }
            yield return null;
        }
        while (true)
        {
            if (opened && !PersistantGameManager.Instance.menuCanvasOpen)
            {
                canContinueDialogue = true;
                canvasContinueButton.gameObject.SetActive(true);
                break;

            }
            yield return null;

        }
    }
    IEnumerator WaitForPlayerToPickupWeapon()
    {
        while(true)
        {
            if(PersistantGameManager.Instance.currentWeapon.itemPrefix == "Jason's ")
            {
                canContinueDialogue = true;
                canvasContinueButton.gameObject.SetActive(true);
                break;
            }
            yield return null;
        }

    }

    IEnumerator WaitForPlayerToKillEnemy()
    {
        bool killed = false;
        while (true)
        {
            try
            {
            
                GameObject nothing = FindObjectOfType<EnemyAttacks>().gameObject;
            }
            catch
            {
          
                killed = true;
            }
            if(killed)
            {
                canContinueDialogue = true;
                canvasContinueButton.gameObject.SetActive(true);
                break;
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
    IEnumerator WaitForPlayerFullHealth()
    {
        while (true)
        {
            if(player.currentHealth > player.totalHealth - 0.01f)
            {
                canContinueDialogue = true;
                canvasContinueButton.gameObject.SetActive(true);
                break;
            }
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
    IEnumerator WaitForPlayerToUnlockSkill()
    {
        while (true)
        {
            if (PersistantGameManager.Instance.playerStats.playerSkillPoints == 0)
            {
                canContinueDialogue = true;
                canvasContinueButton.gameObject.SetActive(true);
                break;
            }
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
    IEnumerator WaitForPlayerToArrangeWeapons()
    {
        while (true)
        {

            if (PersistantGameManager.Instance.playerWeaponInventory[0].itemName == "Jason's Dagger" && PersistantGameManager.Instance.playerWeaponInventory[1].itemDamage > 0.1f)
            {
                break;
            }
            yield return new WaitForSecondsRealtime(0.2f);
        }
        canContinueDialogue = true;
        canvasContinueButton.gameObject.SetActive(true);

    }

}
