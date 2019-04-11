using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{

    double sizeOfBar;
    private EnemyMonitor parent;

    void Start()
    {
        //sets scale to default
        sizeOfBar = transform.localScale.x;
        parent = GetComponentInParent<EnemyMonitor>();
    }

    void Update()
    {
        //as health depletes, scales the x value down relative to health
        //uses a Lerp to smooth the depletion
        sizeOfBar = parent.currentHealth / parent.enemyStats.enemyHealth;
        gameObject.transform.localScale = Vector3.Lerp(transform.localScale, new Vector3((float)sizeOfBar, transform.localScale.y, transform.localScale.z), 0.1f);
    }
}
