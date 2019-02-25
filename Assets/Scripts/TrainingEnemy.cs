using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingEnemy : MonoBehaviour
{

    Rigidbody2D enemyRigidbody;

    public string enemyName;
    GameObject enemys;
    public float totalHealth;
    public float currentHealth;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        currentHealth = totalHealth;

        enemyRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

    }


}
