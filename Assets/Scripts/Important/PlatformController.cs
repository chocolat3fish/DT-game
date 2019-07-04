using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class PlatformController : MonoBehaviour
{
    [Header("5 is kill a certian amount of enemies, 6 is have an item in your inventory")]
    [Header("3 is complete quest, 4 is unlock during and after quest")]
    [Header("1 is Level, 2 is Level of a Skill,")]

    public int type;
    public bool opposite;
    [Header("Stats for Level")]
    public int neededLevel;
    [Header("Stats for Level of a Skill")]
    public string skill;
    public int skillLevel;
    [Header("Stats for complete quest and during a quest")]
    public string questKey;
    public string NPCName;
    public string questName;
    [Header("Stats for kill certian amount of enemies")]
    public string typeOfEnemy;
    public int ammountNeeded;

    [Header("Item needed")]
    public string itemNeeded;

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

        //Checks if the are already active and sets the message for each type
        //The message is what appears on the panel when it is clicked
        switch (type)
        {
            case 1:
                if (PersistantGameManager.Instance.playerStats.playerLevel >= neededLevel)
                {
                    active = true;
                }
                myMessage = "Must reach Level " + neededLevel;
                break;
             case 2:
                if(PersistantGameManager.Instance.skillLevels[skill] >= skillLevel)
                {
                    active = true;
                }
                myMessage = "Must get " + AddSpacesToSentence(skill) + " to Level " + skillLevel;
                break;
            case 3:
                if(PersistantGameManager.Instance.completedQuests.Contains(questKey))
                {
                    active = true;
                }
                myMessage = "Must complete " + questName + " from " + NPCName;
                break;
            case 4:
                if(PersistantGameManager.Instance.completedQuests.Contains(questKey) || PersistantGameManager.Instance.activeQuests.Contains(questKey))
                {
                    active = true;
                }
                myMessage = "Must start " + questName + " from " + NPCName;
                break;
            case 5:
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
            case 6:
                if (PersistantGameManager.Instance.itemInventory[itemNeeded] > 0)
                {
                    active = true;
                }
                myMessage = "Must have " + itemNeeded;
                break;


        }
        if (!active)
        {
            //Sets it as a solid that will get turned off
            if (opposite)
            {
                //Lets player iteract with it
                gameObject.layer = LayerMask.NameToLayer("Map");
                normalC = GetComponent<Tilemap>().color;
                //slightly darker color
                GetComponent<Tilemap>().color = new Color32(192, 192, 192, 255);
                compCollider2D = GetComponent<CompositeCollider2D>();
                compCollider2D.isTrigger = false;
            }
            //Sets it as transparent that will get turned on
            else
            {
                //Makes it so player cannot jump off it
                gameObject.layer = LayerMask.NameToLayer("Default");
                normalC = GetComponent<Tilemap>().color;
                //Sets it to a greyed out color
                GetComponent<Tilemap>().color = greyedOutC;
                compCollider2D = GetComponent<CompositeCollider2D>();
                compCollider2D.isTrigger = true;
            }
        }
        if (active)
        {
            //Sets it to is good state
            if (opposite)
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
                normalC = GetComponent<Tilemap>().color;
                GetComponent<Tilemap>().color = greyedOutC;
                compCollider2D = GetComponent<CompositeCollider2D>();
                compCollider2D.isTrigger = true;
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Map");
                normalC = GetComponent<Tilemap>().color;
                GetComponent<Tilemap>().color = new Color32(192, 192, 192, 255);
                compCollider2D = GetComponent<CompositeCollider2D>();
                compCollider2D.isTrigger = false;
            }
        }

    }
    void Update()
    {
        if (!active)
        {
            //Checks it is is complete then activates it
            switch (type)
            {
                case 1:
                    if (PersistantGameManager.Instance.playerStats.playerLevel >= neededLevel)
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
                    if (PersistantGameManager.Instance.completedQuests.Contains(questKey) || PersistantGameManager.Instance.activeQuests.Contains(questKey))
                    {
                        Activate();
                    }
                    break;
                case 5:
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

                case 6:
                    if (PersistantGameManager.Instance.itemInventory[itemNeeded] > 0)
                    {
                        Activate();
                    }
                    break;
            }

        }

    } 

    //Activates it sets it to is turned on state
    void Activate()
    {
        if (opposite)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            StartCoroutine(FadeOut());
            compCollider2D.isTrigger = true;
            active = true;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Map");
            StartCoroutine(FadeIn());
            compCollider2D.isTrigger = false;
            active = true;
        }
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
    IEnumerator FadeOut()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        for(int i = 192; i >= 81; i--)
        {
            tilemap.color = new Color32((byte)i, (byte)i, (byte)i, (byte)i);
            yield return new WaitForSecondsRealtime(0.81f / i);
        }
        tilemap.color = greyedOutC;
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

    //When the mouse hovers over the platform and clicks a panel ops up with info on the panel
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && panel != null)
        {
            if (!active && !panelOpen)
            {
                panel.SetActive(true);

                //Offsets the panel the right dirrection
                if(Input.mousePosition.x >= Camera.main.pixelWidth/2)
                {
                    panel.GetComponent<AreaRequirements>().offest.x = -110*2;
                }
                else
                {
                    panel.GetComponent<AreaRequirements>().offest.x = 110*2;
                }
                if (Input.mousePosition.y >= Camera.main.pixelHeight / 2)
                {
                    panel.GetComponent<AreaRequirements>().offest.y = -55*2;
                }
                else
                {
                    panel.GetComponent<AreaRequirements>().offest.y = 55*2;
                }

                //Assigns the message  to the panel
                if(type == 5)
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
    //closes panel
    private void OnMouseExit()
    {
        if(panelOpen && panel != null) 
        {
            panelOpen = false;
            panel.SetActive(false);
        }

    }

}
