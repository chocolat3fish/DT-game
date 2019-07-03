using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SkillButtons : MonoBehaviour
{
    public string _name;
    public bool unlocked;
    public string[] toBeUnlocked;
    public int levelToUnlock;
    public int nessesaryXPLevel;
    public int maxLevel;
    public int skillPointCost = 1;
    [HideInInspector]
    public string text;
    [HideInInspector]
    private skillDescription descriptionPanel;
    public string descriptionName;
    [TextArea(5,10)]
    public string skillDescription;
    public Color32 unlockedC = new Color32(0, 245, 7, 255);
    public Color32 notUnlockedC = new Color32(245, 0, 29, 255);
    public bool right, up;
    private void Awake()
    {
        unlockedC = new Color32(13, 147, 0, 255);
        notUnlockedC = new Color32(245, 0, 29, 255);
        descriptionPanel = FindObjectOfType<skillDescription>();
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OpenSkillDescription((PointerEventData)data); });
        trigger.triggers.Add(entry);
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((data) => { CloseSkillDescription((PointerEventData)data); });
        trigger.triggers.Add(entry2);
    }

    public void OpenSkillDescription(PointerEventData data)
    {
        descriptionPanel.gameObject.SetActive(true);
        if (up)
        {
            descriptionPanel.offest.y = 255;
        }
        else
        {
            descriptionPanel.offest.y = -255;
        }
        if (right)
        {
            descriptionPanel.offest.x = 230;
        }
        else
        {
            descriptionPanel.offest.x = -230;
        }
        descriptionPanel.transform.position = Input.mousePosition + descriptionPanel.offest;
        descriptionPanel.transform.Find("Title").GetComponent<Text>().text = AddSpacesToSentence(_name);
        descriptionPanel.transform.Find("Desc").GetComponent<Text>().text = skillDescription;
        bool go = false;
        bool something = false;
        string output = "";
        foreach(string s in toBeUnlocked)
        {
            something = true;
            if(PersistantGameManager.Instance.skillLevels[s] >= levelToUnlock)
            {
                go = true;
                descriptionPanel.transform.Find("Requirements").GetComponent<Text>().color = unlockedC;
            }
            if(output == "")
            {
                output = AddSpacesToSentence(s) + " (lvl " + levelToUnlock + ")";
            }
            else
            {
                output = output + " or\n" + AddSpacesToSentence(s) + " (lvl " + levelToUnlock + ")";
            }

        }
        if (something)
        {
            descriptionPanel.transform.Find("Requirements").GetComponent<Text>().text = output;
        }
        else
        {
            if (nessesaryXPLevel > 0)
            {
                descriptionPanel.transform.Find("Requirements").GetComponent<Text>().text = "Lvl: " + nessesaryXPLevel;
                if(PersistantGameManager.Instance.playerStats.playerLevel >= nessesaryXPLevel)
                {
                    descriptionPanel.transform.Find("Requirements").GetComponent<Text>().color = unlockedC;
                }
                else
                {
                    descriptionPanel.transform.Find("Requirements").GetComponent<Text>().color = notUnlockedC;
                }
            }
            else
            {
                descriptionPanel.transform.Find("Requirements").GetComponent<Text>().text = "None";
                descriptionPanel.transform.Find("Requirements").GetComponent<Text>().color = unlockedC;
            }
            go = true;
        }

        if (!go)
        {
            descriptionPanel.transform.Find("Requirements").GetComponent<Text>().color = notUnlockedC;
        }

       

        descriptionPanel.transform.Find("Lvl").GetComponent<Text>().text = "Level: " + PersistantGameManager.Instance.skillLevels[_name] + "/" + maxLevel + ".";
    }
    public void CloseSkillDescription(PointerEventData data)
    {
        descriptionPanel.gameObject.SetActive(false);
    }

    string AddSpacesToSentence(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                newText.Append(' ');
            newText.Append(text[i]);
        }
        return newText.ToString();
    }




}
