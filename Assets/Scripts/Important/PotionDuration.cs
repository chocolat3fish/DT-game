using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDuration : MonoBehaviour
{
    public GameObject potionBar;

    private void Awake()
    {
        potionBar = transform.Find("Potion Duration").gameObject;
    }
    private void Update()
    {
        if (PersistantGameManager.Instance.potionIsActive && (!potionBar.activeSelf))
        {   
            potionBar.SetActive(true);
        }
        else if (!PersistantGameManager.Instance.potionIsActive && (potionBar.activeSelf))
        {
            potionBar.SetActive(false);
        }
    }
}
