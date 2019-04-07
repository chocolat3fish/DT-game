using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDurationBar : MonoBehaviour
{
    Vector3 localScale;

    void Start()
    {
        //sets scale to default
        localScale = transform.localScale;
    }

    void Update()
    {
        //as health depletes, scales the x value down relative to health
        //uses a Lerp to smooth the depletion
        if (PersistantGameManager.Instance.activePotionType == "Attack")
        {
            localScale.x = (1 - (Time.time - PersistantGameManager.Instance.timeOfAttackMultiplierChange) / PersistantGameManager.Instance.potionCoolDownTime);
        }
        else if (PersistantGameManager.Instance.activePotionType == "Leech")
        {
            localScale.x = (1 - (Time.time - PersistantGameManager.Instance.timeOfLeechMultiplierChange) / PersistantGameManager.Instance.potionCoolDownTime);
        }
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(localScale.x, transform.localScale.y, transform.localScale.z), 0.1f);

    }
}
