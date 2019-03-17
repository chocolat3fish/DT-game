using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour
{
    public Color hasSkill;

    public Button tripleJumpButton;
    public Button fireballButton;
    public Button gripWallButton;

    void Awake()
    {
        tripleJumpButton = gameObject.transform.Find("TJB").GetComponent<Button>();
        fireballButton = gameObject.transform.Find("FBB").GetComponent<Button>();
        gripWallButton = gameObject.transform.Find("GWB").GetComponent<Button>();

        /*
        if (PersistantGameManager.Instance.tripleJump)
        { 
            tripleJumpButton.GetComponent<Image>().color = hasSkill;
            tripleJumpButton.interactable = false;
        }
        */
    }
    

    public void GiveTripleJump()
    {
        if (PersistantGameManager.Instance.playerStats.playerSkillPoints >= 1)
        {
            PersistantGameManager.Instance.tripleJump = true;
            PersistantGameManager.Instance.playerStats.playerSkillPoints -= 1;
            tripleJumpButton.GetComponent<Image>().color = hasSkill;

            tripleJumpButton.interactable = false;
        }

    }

    public void GiveFireball()
    {
        if (PersistantGameManager.Instance.playerStats.playerSkillPoints >= 1)
        {
            PersistantGameManager.Instance.fireball = true;
            PersistantGameManager.Instance.playerStats.playerSkillPoints -= 1;
            fireballButton.GetComponent<Image>().color = hasSkill;

            fireballButton.interactable = false;
            PersistantGameManager.Instance.hasMagic = true;

        }
    }

    public void GiveGripWall()
    {
        if (PersistantGameManager.Instance.playerStats.playerSkillPoints >= 1)
        {
            PersistantGameManager.Instance.gripWalls = true;
            PersistantGameManager.Instance.playerStats.playerSkillPoints -= 1;
            gripWallButton.GetComponent<Image>().color = hasSkill;

            gripWallButton.interactable = false;

        }
    }
}
