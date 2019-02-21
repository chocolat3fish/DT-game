using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
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
        localScale.x = TrainingEnemy.currentHealth;
        transform.localScale = localScale;
    }
}
