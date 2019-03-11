using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceHolderInventoryscript : MonoBehaviour
{
    public int whatAreYouDoing;
    private Text output;
    void Start() {
        output = GetComponent<Text>();
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
                    output.text = PersistantGameManager.Instance.currentWeapon.itemSpeed.ToString();
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
        }
    }
            
}
