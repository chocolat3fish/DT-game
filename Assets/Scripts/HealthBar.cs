using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    Vector3 localScale;
    private EnemyMonitor parent;

    void Start()
    {
        //sets scale to default
        localScale = transform.localScale;
        parent = GetComponentInParent<EnemyMonitor>();
    }

    void Update()
    {
        //as health depletes, scales the x value down relative to health
        localScale.x = parent.currentHealth;
        transform.localScale = localScale;
    }
}
