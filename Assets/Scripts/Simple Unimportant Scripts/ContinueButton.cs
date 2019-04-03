using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{   
    //manager for continue button on overlayCanvas
    public NPCMonitor nPCMonitor;
    private bool continueDialouge = false;

    public void OnClick()
    {
        nPCMonitor.ContinueDialogue();
    }
}
