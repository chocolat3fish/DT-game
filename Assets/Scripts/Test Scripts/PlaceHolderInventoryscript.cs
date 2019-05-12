using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlaceHolderInventoryscript : MonoBehaviour
{
    public int whatAreYouDoing;
    private PlayerControls playerControls;
    private Text output;
    void Start() {
        output = GetComponent<Text>();
        playerControls = FindObjectOfType<PlayerControls>();
     }


    private void Update()
    {

        if (whatAreYouDoing < 6)
        {

            if (PersistantGameManager.Instance.playerWeaponInventory[whatAreYouDoing - 1] != null)
            {
                output.text = whatAreYouDoing + ": " + PersistantGameManager.Instance.playerWeaponInventory[whatAreYouDoing - 1].itemName;
            }
            else { output.text = ""; }
        }

        switch (whatAreYouDoing)
        {

            case 6:
                if (PersistantGameManager.Instance.currentWeapon != null)
                {
                    output.text = (PersistantGameManager.Instance.currentIndex + 1) + ": " + PersistantGameManager.Instance.currentWeapon.itemName;
                }

                else { output.text = ""; }

                break;

            case 7:
                if (PersistantGameManager.Instance.currentWeapon != null)
                {
                    output.text = PersistantGameManager.Instance.currentWeapon.itemDamage.ToString();
                }
                else { output.text = ""; }

                break;

            case 8:
                if (PersistantGameManager.Instance.currentWeapon != null)
                {
                    output.text = PersistantGameManager.Instance.currentWeapon.trueItemSpeed.ToString();
                }
                else { output.text = ""; }

                break;

            case 9:
                if (PersistantGameManager.Instance.currentWeapon != null)
                {
                    output.text = PersistantGameManager.Instance.currentWeapon.itemRange.ToString();
                }
                else { output.text = ""; }
                break;

            case 10:
                if (PersistantGameManager.Instance.currentArmour != null)
                {
                    output.text = PersistantGameManager.Instance.currentArmour.armourType;
                }
                else { output.text = ""; }

                break;

            case 11:
                output.text = PersistantGameManager.Instance.playerStats.playerLevel.ToString();
                break;

            case 12:
                output.text = PersistantGameManager.Instance.playerStats.playerExperience.ToString();
                break;

            case 13:
                output.text = (PersistantGameManager.Instance.totalExperience - PersistantGameManager.Instance.playerStats.playerExperience).ToString();
                break;

            case 14:
                output.text = PersistantGameManager.Instance.playerStats.playerSkillPoints.ToString();
                break;

            case 15:
                //left item + number of that item, uncomment when ready
                if(PersistantGameManager.Instance.equippedItemOne == "Empty")
                {
                    output.text = "No items";
                    break;
                }
                output.text = PersistantGameManager.Instance.equippedItemOne + " x " + PersistantGameManager.Instance.amountOfConsumables[PersistantGameManager.Instance.equippedItemOne] + " (H)";
                break;

            case 16:
                //right item + number of that item, uncomment when ready
                if (PersistantGameManager.Instance.equippedItemTwo == "Empty")
                {
                    output.text = "No items";
                    break;
                }
                output.text = PersistantGameManager.Instance.equippedItemTwo + " x " + PersistantGameManager.Instance.amountOfConsumables[PersistantGameManager.Instance.equippedItemTwo] + " (J)";
                break;
            case 17:
                //output.text = PersistantGameManager.Instance.activePotionType + " Potion";
                output.text = PersistantGameManager.Instance.currentActiveAbility;
                break;

            case 18:
                output.text = Math.Round(playerControls.currentHealth, 1) + " / " + Math.Round(playerControls.totalHealth, 1);
                break;

            case 19:
                Weapon equippedWeapon = PersistantGameManager.Instance.currentWeapon;
                int weaponIndex = PersistantGameManager.Instance.playerWeaponInventory.IndexOf(equippedWeapon) + 1;
                output.text = equippedWeapon.itemName + " (Slot " + weaponIndex + ")" ;
                break;
            case 20:
                output.text = "Lvl: " + PersistantGameManager.Instance.playerStats.playerLevel;
                break;
            case 21:
                output.text = "Skill P: " + PersistantGameManager.Instance.playerStats.playerSkillPoints;
                break;
        }
    }

}
