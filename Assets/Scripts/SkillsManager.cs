using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour
{
    public Color hasSkill;

    public Button tripleJumpButton;

    void Awake()
    {
        tripleJumpButton = gameObject.transform.Find("TJB").GetComponent<Button>();

        /*
        if (PersistantGameManager.Instance.tripleJump)
        { 
            tripleJumpButton.GetComponent<Image>().color = hasSkill;
            tripleJumpButton.interactable = false;
        }
        */
    }

    void Update()
    {
        
    }

    public void giveTripleJump()
    {
        if (PersistantGameManager.Instance.playerStats.playerSkillPoints >= 1)
        {
            PersistantGameManager.Instance.tripleJump = true;
            PersistantGameManager.Instance.playerStats.playerSkillPoints -= 1;
            tripleJumpButton.GetComponent<Image>().color = hasSkill;

            tripleJumpButton.interactable = false;
        }

    }
}
