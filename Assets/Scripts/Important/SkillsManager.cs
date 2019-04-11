using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour
{
    public Color hasSkill;

    public Button tripleJumpButton;
    public Button highDamageButton;
    public Button gripWallButton;
    public Button attackSpeedButton;
    public Text attackSpeedQuantity;



    void Awake()
    {
        tripleJumpButton = gameObject.transform.Find("TJB").GetComponent<Button>();
        gripWallButton = gameObject.transform.Find("GWB").GetComponent<Button>();
        highDamageButton = gameObject.transform.Find("HDB").GetComponent<Button>();
        attackSpeedButton = gameObject.transform.Find("ASB").GetComponent<Button>();
        attackSpeedQuantity = attackSpeedButton.transform.Find("Quantity").GetComponent<Text>();

        if (PersistantGameManager.Instance.tripleJump == true) { tripleJumpButton.GetComponent<Image>().color = hasSkill; }
        if (PersistantGameManager.Instance.gripWalls == true) { gripWallButton.GetComponent<Image>().color = hasSkill; }

        if (PersistantGameManager.Instance.highDamage == true) { highDamageButton.GetComponent<Image>().color = hasSkill; }
        if (PersistantGameManager.Instance.maxedSpeed == true) { attackSpeedButton.GetComponent<Image>().color = hasSkill; }




        /*
        if (PersistantGameManager.Instance.tripleJump)
        { 
            tripleJumpButton.GetComponent<Image>().color = hasSkill;
            tripleJumpButton.interactable = false;
        }
        */
    }

    private void Update()
    {
        attackSpeedQuantity.text = PersistantGameManager.Instance.attackSpeedUpgrades.ToString();
    }

    // These functions are ordered in the way you can get them
    // Regions separate trees
    // Within a region they are ordered
    // Don't change it
    //

    #region Mobility

    public void GiveTripleJump()
    {
        if (PersistantGameManager.Instance.playerStats.playerSkillPoints >= 1 && PersistantGameManager.Instance.tripleJump == false)
        {
            PersistantGameManager.Instance.tripleJump = true;
            PersistantGameManager.Instance.playerStats.playerSkillPoints -= 1;
            tripleJumpButton.GetComponent<Image>().color = hasSkill;

            tripleJumpButton.interactable = false;
            if (PersistantGameManager.Instance.mobilityProgress == 0) { PersistantGameManager.Instance.mobilityProgress = 1; }
        }

    }

    public void GiveGripWall()
    {
        if (PersistantGameManager.Instance.playerStats.playerSkillPoints >= 1 && PersistantGameManager.Instance.mobilityProgress >= 1 && PersistantGameManager.Instance.gripWalls == false)
        {
            PersistantGameManager.Instance.gripWalls = true;
            PersistantGameManager.Instance.playerStats.playerSkillPoints -= 1;
            gripWallButton.GetComponent<Image>().color = hasSkill;

            gripWallButton.interactable = false;
            if (PersistantGameManager.Instance.mobilityProgress == 1) { PersistantGameManager.Instance.mobilityProgress = 2; }

        }
    }

    #endregion

    #region Damage

    public void GiveHighDamage()
    {
        if (PersistantGameManager.Instance.playerStats.playerSkillPoints >= 1 && PersistantGameManager.Instance.highDamage == false)
        {
            PersistantGameManager.Instance.highDamage = true;
            PersistantGameManager.Instance.playerStats.playerSkillPoints -= 1;
            highDamageButton.GetComponent<Image>().color = hasSkill;

            highDamageButton.interactable = false;
            PersistantGameManager.Instance.hasMagic = true;
            if (PersistantGameManager.Instance.damageProgress == 0) { PersistantGameManager.Instance.damageProgress = 1; }

        }
    }

    public void GiveAttackSpeed()
    {
        if (PersistantGameManager.Instance.playerStats.playerSkillPoints >= 1 && PersistantGameManager.Instance.damageProgress >= 1 && PersistantGameManager.Instance.attackSpeedUpgrades < 10)
        {
            PersistantGameManager.Instance.attackSpeedMulti -= 0.05;
            PersistantGameManager.Instance.playerStats.playerSkillPoints -= 1;
            PersistantGameManager.Instance.attackSpeedUpgrades += 1;

            foreach (Weapon weapon in PersistantGameManager.Instance.playerWeaponInventory)
            {
                weapon.trueItemSpeed = weapon.stockItemSpeed * (float)PersistantGameManager.Instance.attackSpeedMulti;
            }

            if (PersistantGameManager.Instance.damageProgress == 1) { PersistantGameManager.Instance.damageProgress = 2; }
            if (PersistantGameManager.Instance.attackSpeedUpgrades >= 10) 
            { 
                attackSpeedButton.GetComponent<Image>().color = hasSkill;
                attackSpeedButton.interactable = false;
                PersistantGameManager.Instance.maxedSpeed = true;
            }

        }
    }

    #endregion
}
