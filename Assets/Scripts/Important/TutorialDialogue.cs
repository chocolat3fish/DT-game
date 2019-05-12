using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TutorialDialogue : MonoBehaviour
{
    GameObject dialogueCanvas, toTalkPanel;
    public Quest tutorialQuest;
    public Text canvasMainText, canvasNameText, canvasRewardText;
    Button canvasAcceptButton, canvasContinueButton;
    bool dialogueBoxOpen;
    int stageOfConvo, currentSentenceIndex;
    bool isTalking, canContinueDialogue;
    PlayerControls player;
    void Awake()
    {
        StartCoroutine(StartDialogue());
        player = FindObjectOfType<PlayerControls>();

    }
    IEnumerator StartDialogue()
    {
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
                dialogueBoxOpen = true;
                break;
            }
            else
            {
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
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
        PersistantGameManager.Instance.activeQuests.Add(tutorialQuest.questKey);
        PersistantGameManager.Instance.possibleQuests.Add(tutorialQuest.questKey, tutorialQuest);
        yield return new WaitForSecondsRealtime(1f);
        StopAllCoroutines();
        StartCoroutine(AddChars("Hello Player, \nMy name is Jason welcome to the incredible world of [insert].../d", canvasMainText));
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.M) && !(stageOfConvo == 1 && currentSentenceIndex == 0))
        {
            ContinueDialogue();
        }
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
                ContinueDialogue();
            }
        }
    }

    IEnumerator AddChars(string sentence, Text text)
    {
        canContinueDialogue = false;
        text.text = "";
        char previousChar = "a"[0];
        bool shouldWait = false;
        foreach (char ch in sentence)
        {
            if (ch == "/"[0])
            {
                previousChar = ch;
                continue;
            }
            if (ch == "w"[0] && previousChar == "/"[0])
            {
                yield return new WaitForSecondsRealtime(0.5f);
                previousChar = "a"[0];
                shouldWait = true;
                continue;
            }
            if(ch == "d"[0] && previousChar == "/"[0])
            {
                canContinueDialogue = true;
                continue;
            }
            if(ch == "r"[0] && previousChar == "/"[0])
            {
                yield return new WaitForSecondsRealtime(1f);
                text.text = "";
                continue;
            }
            previousChar = ch;
            text.text += ch;
            yield return null;
            yield return null;

            if (shouldWait)
            {
                yield return null;
            }

        }
    }
    public void ContinueDialogue()
    {
        if(canContinueDialogue)
        {
            if(stageOfConvo == 0)
            {
                if(currentSentenceIndex == 0)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars("Let me show you how to play.../d", canvasMainText));
                    currentSentenceIndex = 1;
                }
                else if(currentSentenceIndex == 1)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars("Using the Arrow Keys or ‘a’ and ‘d’ you can move left and right,\n/wTry that now...", canvasMainText));
                    canvasContinueButton.gameObject.SetActive(false);
                    canContinueDialogue = false;
                    StartCoroutine(WaitForPlayerMove());
                    currentSentenceIndex = 2;

                }
                else if(currentSentenceIndex == 2)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars("Now try a jump then a double jump with the space bar...", canvasMainText));
                    canvasContinueButton.gameObject.SetActive(false);
                    canContinueDialogue = false;
                    StartCoroutine(WaitForPlayerJump());
                    currentSentenceIndex = 3;
                }
                else if(currentSentenceIndex == 3)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars("Head to your right and grab that item on the topmost platform,\nUse ‘e’ to interact with items...", canvasMainText));
                    canvasContinueButton.gameObject.SetActive(false);
                    canContinueDialogue = false;
                    StartCoroutine(WaitForPlayerPickup());
                    currentSentenceIndex = 4;
                }
                else if (currentSentenceIndex == 4)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars("Now lets have a look at the items you hold, Click 'tab' to open the\nmenu, And click the items button to view your inventory...", canvasMainText));
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
                    StartCoroutine(AddChars("You can look deeper into a quest by clicking on it, Give it a go...", canvasMainText));
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
                    StartCoroutine(AddChars("Nice you figured out how to talk to me using ‘m’.../d", canvasMainText));
                    canContinueDialogue = false;
                    currentSentenceIndex = 2;
                }
                else if (currentSentenceIndex == 2)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    StartCoroutine(AddChars("This is the way you talk to other [insert]ians like me\nThey will give you quests and rewards this way.../d", canvasMainText));
                    currentSentenceIndex = 3;
                }
                else if (currentSentenceIndex == 3)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    StartCoroutine(AddChars("Hmmm, I just had a look at your weapons you have,\nNothing, More Nothing, And More Nothing.../d", canvasMainText));

                    currentSentenceIndex = 4;
                }
                else if (currentSentenceIndex == 4)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    StartCoroutine(AddChars("We better do something about that.../d", canvasMainText));
                    currentSentenceIndex = 5;
                }
                else if (currentSentenceIndex == 5)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    GameObject questDrop = Instantiate(Resources.Load("Loot Drop"), player.transform.position, Quaternion.identity) as GameObject;
                    LootDropMonitor questDropMonitor = questDrop.GetComponent<LootDropMonitor>();
                    questDropMonitor.type = 0;
                    questDropMonitor.itemStats = new Weapon("Jason's Dagger", 1, 0.5f, 0.5f, 1, 1);
                    StartCoroutine(AddChars("Here's my dagger, I don't use it much anyway, Use 'e' to pick up a weapon, and use 1, 2 or 3 to choose the slot.../d", canvasMainText));
                    currentSentenceIndex = 6;
                }
                else if (currentSentenceIndex == 6)
                {
                    print("Continued");
                    bool hasPickedItUp = false;

                    foreach(Weapon weapon in PersistantGameManager.Instance.playerWeaponInventory)
                    {
                        if(weapon.itemName == "Jason's Dagger")
                        {
                            hasPickedItUp = true;
                            break;
                        }
                    }
                    if(hasPickedItUp)
                    {
                        currentSentenceIndex = 7;
                        ContinueDialogue();
                    }
                    else
                    {
                        StopAllCoroutines();
                        canContinueDialogue = false;
                        canvasContinueButton.gameObject.SetActive(false);
                        StartCoroutine(AddChars("Pick up my dagger, use 'e' and click the Take button or 'L' to take the weapon/d", canvasMainText));
                        StartCoroutine(WaitForPlayerToPickupWeapon());
                        currentSentenceIndex = 7;
                    }
                }
                else if(currentSentenceIndex == 7)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    Instantiate(Resources.Load("Tutorial Enemy"));
                    canvasContinueButton.gameObject.SetActive(false);
                    StartCoroutine(AddChars("Oh no, a evil door broke in, use 'w' to attack,\nBut make sure not to touch it or it will hurt you...", canvasMainText));
                    StartCoroutine(WaitForPlayerToKillEnemy());
                    currentSentenceIndex = 8;
                }
                else if(currentSentenceIndex == 8)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    if(player.currentHealth > player.totalHealth - 0.01f)
                    {
                        StartCoroutine(AddChars("Wow, I'm impressed you did that without taking any damage!.../d", canvasMainText));
                        currentSentenceIndex = 9;
                    }
                    else
                    {
                        canvasContinueButton.gameObject.SetActive(false);
                        StartCoroutine(AddChars("Oosh you took some hard hits, Just wait a few seconds without taking damage or attacking and your health will soon come back...", canvasMainText));
                        StartCoroutine(WaitForPlayerFullHealth());
                        currentSentenceIndex = 10;
                    }

                }
                else if(currentSentenceIndex == 9)
                {
                    canContinueDialogue = false;
                    StopAllCoroutines();
                    StartCoroutine(AddChars("But if you do take damage your health regenerates after a few seconds of no combat.../d", canvasMainText));
                    currentSentenceIndex = 10;
                }
                else if(currentSentenceIndex == 10)
                {
                    StopAllCoroutines();
                    StartCoroutine(AddChars("You have completed the tutorial so far!", canvasMainText));
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
            if(PersistantGameManager.Instance.currentWeapon.itemName == "Jason's Dagger")
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
                print("Alive");
                GameObject nothing = FindObjectOfType<EnemyAttacks>().gameObject;
            }
            catch
            {
                print("Dead");
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
}
