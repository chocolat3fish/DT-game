using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
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
        //uses a Lerp to smooth the depletion
        localScale.x = parent.currentHealth / parent.enemyStats.enemyHealth;
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(localScale.x, transform.localScale.y, transform.localScale.z), 0.1f);
    }
}
