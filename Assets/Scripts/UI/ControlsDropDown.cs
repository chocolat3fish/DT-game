using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsDropDown : MonoBehaviour
{
    public GameObject dropDown;

    private void Awake()
    {

        dropDown = gameObject.transform.Find("Dropdown").gameObject;
        dropDown.SetActive(false);
    }

    public void ToggleDropdown()
    {
        if (dropDown.activeSelf == false)
        {
            dropDown.SetActive(true);
        }
        else
        {
            dropDown.SetActive(false);
        }

    }
}
