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

                if (PersistantGameManager.Instance.playerInventory[whatAreYouDoing - 1] != null)
                {
                    output.text = whatAreYouDoing + ": " + PersistantGameManager.Instance.playerInventory[whatAreYouDoing - 1].itemName;
                }
        }
        else if (whatAreYouDoing == 6)
        {
            output.text = (PersistantGameManager.Instance.currentIndex + 1) + ": " + PersistantGameManager.Instance.currentWeapon.itemName;

        }

        else if (whatAreYouDoing == 7)
        { 
            if(PersistantGameManager.Instance.currentWeapon != null)
            {
                output.text = PersistantGameManager.Instance.currentWeapon.itemDamage.ToString();
            }

        }
        else if (whatAreYouDoing == 8)
        {
            if (PersistantGameManager.Instance.currentWeapon != null)
            {
                output.text = PersistantGameManager.Instance.currentWeapon.itemSpeed.ToString();
            }

        }
        else if (whatAreYouDoing == 9)
        {
            if (PersistantGameManager.Instance.currentWeapon != null)
            {
                output.text = PersistantGameManager.Instance.currentWeapon.itemRange.ToString();
            }
        }
    }






}
