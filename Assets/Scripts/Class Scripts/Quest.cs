using System;
using UnityEngine;

[Serializable]
public class Quest
{
    public string[] sentencesBeforeQuest;
    public string questStatment;
    public string[] sentencesAfterQuest;

    public string questItemName;

    public Quest(string[] sentencesBeforeQuest, string questStatment, string[] sentencesAfterQuest, string questItemName)
    {
        this.sentencesBeforeQuest = sentencesBeforeQuest;
        this.questStatment = questStatment;
        this.sentencesAfterQuest = sentencesAfterQuest;

        this.questItemName = questItemName;
    }
}
