using System;
using UnityEngine;

[Serializable]
public class Quest
{
    public string questName;
    [TextArea(5, 20)]
    public string[] sentencesBeforeQuest;
    [TextArea(5, 20)]
    public string questStatment;
    [TextArea(5, 20)]
    public string[] sentencesAfterQuest;
    [TextArea(5, 20)]
    public string questDescription;

    public string questItemName;

    public string questKey;

    public Quest(string questName, string[] sentencesBeforeQuest, string questStatment, string[] sentencesAfterQuest, string questItemName, string questDescription)
    {
        this.questName = questName;

        this.sentencesBeforeQuest = sentencesBeforeQuest;
        this.questStatment = questStatment;
        this.sentencesAfterQuest = sentencesAfterQuest;

        this.questItemName = questItemName;

        this.questDescription = questDescription;
    }
}
