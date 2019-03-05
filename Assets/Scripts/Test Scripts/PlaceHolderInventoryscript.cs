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
        if(whatAreYouDoing < 6)
        {

                if (PersistantGameManager.Instance.playerWeaponInventory[whatAreYouDoing - 1] != null)
                {
                    output.text = whatAreYouDoing + ": " + PersistantGameManager.Instance.playerWeaponInventory[whatAreYouDoing - 1].itemName;
                }
                else { output.text = ""; }
        }
        else if (whatAreYouDoing == 6)
        {
            if (PersistantGameManager.Instance.currentWeapon != null)
            {
                output.text = (PersistantGameManager.Instance.currentIndex + 1) + ": " + PersistantGameManager.Instance.currentWeapon.itemName;
            }
            else { output.text = ""; }
        }
        

        else if (whatAreYouDoing == 7)
        { 
            if(PersistantGameManager.Instance.currentWeapon != null)
            {
                output.text = PersistantGameManager.Instance.currentWeapon.itemDamage.ToString();
            }
            else { output.text = ""; }

        }
        else if (whatAreYouDoing == 8)
        {
            if (PersistantGameManager.Instance.currentWeapon != null)
            {
                output.text = PersistantGameManager.Instance.currentWeapon.itemSpeed.ToString();
            }
            else { output.text = ""; }

        }
        else if (whatAreYouDoing == 9)
        {
            if (PersistantGameManager.Instance.currentWeapon != null)
            {
                output.text = PersistantGameManager.Instance.currentWeapon.itemRange.ToString();
            }
            else { output.text = ""; }
        }
        else if (whatAreYouDoing == 10)
        {
            if (PersistantGameManager.Instance.currentArmor != null)
            {
                output.text = PersistantGameManager.Instance.currentArmor.armorType;
            }
            else { output.text = ""; }
        }
    }






}
