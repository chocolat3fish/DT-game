using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{

    public bool projectile, patrol;
    public List<Transform> patrolPoints = new List<Transform>();
    private int currentPointIndex;
    public float moveSpeed;
    public GameObject enemy;
    private int waitTime;
    void Awake()
    {
        enemy = gameObject.transform.Find("Enemy").gameObject;
        if (patrol)
        {
            patrolPoints.Add(gameObject.transform.Find("Point 1"));
            patrolPoints.Add(gameObject.transform.Find("Point 2"));
        }
    }


    void Update()
    {
        if(patrol)
        {
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, patrolPoints[currentPointIndex].transform.position, Time.deltaTime * moveSpeed);
            if (Vector2.Distance(enemy.transform.position, patrolPoints[currentPointIndex].position) < .2f)
            {
                if (currentPointIndex == 0) { currentPointIndex = 1; }
                else if (currentPointIndex == 1) { currentPointIndex = 0; }
            }
        }

    }




}
