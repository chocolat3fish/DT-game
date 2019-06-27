using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdatesMonitor : MonoBehaviour
{
    public TextMeshProUGUI updateText;
    string oldText;

    void Start()
    {
        updateText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (oldText != updateText.text)
        {
            StopAllCoroutines();
            StartCoroutine(DisplayUpdate());
            oldText = updateText.text;
        }
    }

    public IEnumerator DisplayUpdate()
    {
        yield return new WaitForSecondsRealtime(5);
        updateText.text = "";
        oldText = "";
    }
}
