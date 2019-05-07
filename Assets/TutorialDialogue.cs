using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TutorialDialogue : MonoBehaviour
{
    GameObject dialogueCanvas;
    Text canvasMainText, canvasNameText, canvasRewardText;
    Button canvasAcceptButton, canvasContinueButton;
    bool dialogueBoxOpen;
    int stageOfConvo, currentSentenceIndex;
    bool isTalking, canContinueDialogue;

    void Start()
    {
        StartCoroutine(StartDialogue());
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

        AddChars("Hello Player, \nMy name is Jason welcome to the incredible world of [insert],\nLet me show you how to play.\n", canvasMainText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator AddChars(string sentence, Text text)
    {
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
            previousChar = ch;
            text.text += ch;
            yield return null;
            if (shouldWait)
            {
                yield return null;
            }

        }
    }
}
