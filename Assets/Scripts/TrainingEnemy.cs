using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingEnemy : MonoBehaviour
{

    Rigidbody2D enemyRigidbody;

    public string enemyName;

    public float totalHealth;
    public static float currentHealth;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        currentHealth = totalHealth;

        enemyRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

    }

}
