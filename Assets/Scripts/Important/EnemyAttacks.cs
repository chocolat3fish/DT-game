using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{

    public bool projectile, patrol;
    public List<Transform> patrolPoints = new List<Transform>();
    private int currentPointIndex;
    public float moveSpeed;
    private GameObject enemy;
    public float shootSpeed;
    private float timeOfShot;
    public float range;
    public float projectileSpeed;
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
        if(projectile)
        {
            if(Time.time > timeOfShot + shootSpeed)
            {
                Fire();
                timeOfShot = Time.time;
            }
        }
    }

    private void Fire()
    {
        GameObject bullet = Instantiate((GameObject)Resources.Load("Bullet"), transform.position, Quaternion.identity);
        BulletController bulletStats = bullet.GetComponent<BulletController>();
        bulletStats.range = range;
        bulletStats.speed = projectileSpeed;
        bulletStats.enemyWhoFiredThis = enemy;
    }




}
