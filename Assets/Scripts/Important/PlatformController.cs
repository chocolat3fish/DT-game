using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class PlatformController : MonoBehaviour
{
    [Header("3 is complete quest, 4 is kill a certian ammount of enemies")]
    [Header("1 is Level, 2 is Level of a Skill,")]

    public int type;
    [Header("Stats for Level")]
    public int neededLevel;
    [Header("Stats for Level of a Skill")]
    public string skill;
    public int skillLevel;
    [Header("Stats for complete quest")]
    public string questKey;
    public string NPCName;
    public string questName;
    [Header("Stats for kill certian ammount of enemies")]
    public string typeOfEnemy;
    public int ammountNeeded;
    CompositeCollider2D compCollider2D;
    Color32 normalC;
    Color32 greyedOutC = new Color32(81, 81, 81, 81);
    public GameObject panel;
    public Text message;
    string myMessage;
    bool panelOpen;
    public bool active;

    public void Start()
    {
        active = false;
        switch (type)
        {
            case 1:
                if (PersistantGameManager.Instance.playerStats.playerLevel >= neededLevel)
                {
                    active = true;
                }
                myMessage = "Must Reach Level " + neededLevel;
                break;
             case 2:
                if(PersistantGameManager.Instance.skillLevels[skill] >= skillLevel)
                {
                    active = true;
                }
                myMessage = "Must Get " + AddSpacesToSentence(skill) + " To Level " + skillLevel;
                break;
            case 3:
                if(PersistantGameManager.Instance.completedQuests.Contains(questKey))
                {
                    active = true;
                }
                myMessage = "Must Complete " + questName + " From " + NPCName;
                break;
            case 4:
                try
                {
                    if(PersistantGameManager.Instance.currentEnemyKills[typeOfEnemy] >= ammountNeeded)
                    {
                        active = true;
                    }
                }
                catch
                {
                    myMessage = "Must kill " + ammountNeeded + " " + AddSpacesToSentence(typeOfEnemy) + "s,\n";
                }
                myMessage = "Must kill " + ammountNeeded + " " + AddSpacesToSentence(typeOfEnemy) + "s,\n";
                break;

        }
        if (!active)
        {
            normalC = GetComponent<Tilemap>().color;
            GetComponent<Tilemap>().color = greyedOutC;
            compCollider2D = GetComponent<CompositeCollider2D>();
            compCollider2D.isTrigger = true;
        }

    }
    void Update()
    {
        if (!active)
        {
            switch (type)
            {
                case 1:
                    if(PersistantGameManager.Instance.playerStats.playerLevel >= neededLevel)
                    {
                      Activate();
                    }
                    break;
                case 2:
                    if (PersistantGameManager.Instance.skillLevels[skill] >= skillLevel)
                    {
                        Activate();
                    }
                    break;
                case 3:
                    if (PersistantGameManager.Instance.completedQuests.Contains(questKey))
                    {
                        Activate();
                    }
                    break;
                case 4:
                    try
                    {
                        if (PersistantGameManager.Instance.currentEnemyKills[typeOfEnemy] >= ammountNeeded)
                        {
                            Activate();
                        }
                    }
                    catch
                    {
                    }
                    break;
            }

        }
    } 
    void Activate()
    {
        StartCoroutine(FadeIn());
        compCollider2D.isTrigger = false;
        active = true;
    }
    IEnumerator FadeIn()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        for (int i = 81; i < 255; i++)
        {
            tilemap.color = new Color32((byte)i, (byte)i, (byte)i, (byte)i);
            yield return new WaitForSeconds(0.81f / i);
        }
        tilemap.color = normalC;
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

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && panel != null)
        {
            if (!active && !panelOpen)
            {
                panel.SetActive(true);
                if(Input.mousePosition.x >= Camera.main.pixelWidth/2)
                {
                    panel.GetComponent<AreaRequirements>().offest.x = -110;
                }
                else
                {
                    panel.GetComponent<AreaRequirements>().offest.x = 110;
                }
                if (Input.mousePosition.y >= Camera.main.pixelHeight / 2)
                {
                    panel.GetComponent<AreaRequirements>().offest.y = -55;
                }
                else
                {
                    panel.GetComponent<AreaRequirements>().offest.y = 55;
                }

                if(type == 4)
                {
                    int numberNeeded;
                    try
                    {
                        numberNeeded = ammountNeeded - PersistantGameManager.Instance.currentEnemyKills[typeOfEnemy];
                    }
                    catch
                    {
                        numberNeeded = ammountNeeded;
                    }
                    message.text = myMessage + numberNeeded + " To Go";
                }
                else
                {
                    message.text = myMessage;
                }
                panelOpen = true;
            }
            else if (panelOpen)
            {
                panel.SetActive(false);
                panelOpen = false;
            }
        }

    }
    private void OnMouseExit()
    {
        if(panelOpen && panel != null) 
        {
            panelOpen = false;
            panel.SetActive(false);
        }

    }

}
