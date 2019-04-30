using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDuration : MonoBehaviour
{
    public GameObject abilityBar;

    private void Awake()
    {
        abilityBar = transform.Find("Ability Duration").gameObject;
    }
    private void Update()
    {
        if (PersistantGameManager.Instance.abilityIsActive && (!abilityBar.activeSelf))
        {   
            abilityBar.SetActive(true);
        }
        else if (!PersistantGameManager.Instance.abilityIsActive && (abilityBar.activeSelf))
        {
            abilityBar.SetActive(false);
        }
    }
}
