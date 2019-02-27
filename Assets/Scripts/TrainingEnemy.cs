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

        //Fixes issue of having to move before being able to attack it again. Likely better solution but will work for now.
        enemyRigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;
        currentHealth = totalHealth;

        enemyRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

    }


}
