using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceBar : MonoBehaviour {

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
        localScale.x = PersistantGameManager.Instance.playerStats.playerExperience / PersistantGameManager.Instance.totalExperience;
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(localScale.x, transform.localScale.y, transform.localScale.z), 0.1f);

    }
}