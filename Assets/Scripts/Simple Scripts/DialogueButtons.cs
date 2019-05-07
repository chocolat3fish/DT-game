using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DialogueButtons : MonoBehaviour
{
    //manager for continue button on overlayCanvas
    public NPCMonitor[] nPCMonitors;
    public NPCMonitor nPCMonitor;
    private bool continueDialouge = false;
    private void Awake()
    {
        nPCMonitors = FindObjectsOfType<NPCMonitor>();
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            string questKey = PersistantGameManager.Instance.currentDialogueQuest.questKey;
            string firstTwoLettersOfKey = questKey[0].ToString() + questKey[1].ToString();
            foreach (NPCMonitor nPC in nPCMonitors)
            {
                string firstTwoLettersOfName = nPC.nameOfNpc[0].ToString() + nPC.nameOfNpc[1].ToString();
                if (firstTwoLettersOfKey == firstTwoLettersOfName)
                {
                    nPCMonitor = nPC;
                    break;
                }
            }
        }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            OnGiveItemClick();
        }
    }

    public void OnContinueClick()
    {
        nPCMonitor.ContinueDialogue();
    }
    public void OnGiveItemClick()
    {
        nPCMonitor.GiveItem();
    }
}
