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
        nPCMonitor.ContinueDialouge();
    }
    private void Update()
    {
        if (nPCMonitor.isTalking && nPCMonitor.canContinueDialouge)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                continueDialouge = true;
            }
        }
    }
    private void FixedUpdate()
    {
        if(continueDialouge)
        {
            nPCMonitor.ContinueDialouge();
            continueDialouge = false;

        }
    }
}
