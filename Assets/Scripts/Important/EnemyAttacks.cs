using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAttacks : MonoBehaviour
{

    public bool projectile, patrol, charge;
    public List<Transform> patrolPoints = new List<Transform>();
    private int currentPointIndex;
    public float moveSpeed;
    private GameObject enemy;
    public float shootSpeed;
    private float timeOfShot;
    public float range;
    public float projectileSpeed;
    public float projectileDamage;
    public GameObject player;
    private float distanceBetweenPoints;
    public float chargeCoolDown;
    public float timeOfCharge;
    public float time;
    private bool chargingLeft;
    private bool chargingRight;
    public float chargeSpeed;
    public bool hasJumped;
    public Rigidbody2D rb;
    public bool doJump;
    public float jumpPower;
    private float timeOfJump;
    private bool shouldCharge;
    public float timeBetweenJumpAndCharge;
    public float timeOfShake;
    private bool patrolling;
    private bool isInZone;
    void Awake()
    {
        enemy = gameObject.transform.Find("Enemy").gameObject;
        if (patrol || charge)
        { 
            patrolPoints.Add(gameObject.transform.Find("Left Point"));
            patrolPoints.Add(gameObject.transform.Find("Right Point"));
            distanceBetweenPoints = Vector2.Distance(patrolPoints[0].position, patrolPoints[1].position);
        }
        player = FindObjectOfType<PlayerControls>().gameObject;
        if(charge)
        {
            enemy.transform.position = patrolPoints[0].transform.position;
            currentPointIndex = 1;
        }
        rb = enemy.GetComponent<Rigidbody2D>();
        if(patrol)
        {
            patrolling = true;
        }
    }


    void Update()
    {
        time = Time.time;
        if(patrolling)
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
        if(charge)
        {
            float distanceFromZero = Vector2.Distance(patrolPoints[0].transform.position, player.transform.position);
            float distanceFromOne = Vector2.Distance(patrolPoints[1].transform.position, player.transform.position);
            bool isInYOfPointZero = patrolPoints[0].position.y + 2 > player.transform.position.y && patrolPoints[0].position.y - 2 < player.transform.position.y;
            bool isInYOfPointOne = patrolPoints[1].position.y + 2 > player.transform.position.y && patrolPoints[1].position.y - 2 < player.transform.position.y;
            isInZone = distanceFromZero < distanceBetweenPoints && distanceFromOne < distanceBetweenPoints && isInYOfPointZero && isInYOfPointOne;

            if (isInZone && !hasJumped && Time.time > timeOfCharge + chargeCoolDown)
            {
                if(patrol)
                {
                    patrolling = false;
                }
                hasJumped = true;
                timeOfJump = Time.time;
                StartCoroutine(Shake());
                if (player.transform.position.x < enemy.transform.position.x)
                {
                    chargingLeft = true;
                }
                else
                {
                    chargingRight = true;

                }
                if (doJump)
                {
                    rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                }
            }

        }
        if(chargingLeft && isInZone && Time.time > timeOfJump + timeBetweenJumpAndCharge)
        {
            timeOfCharge = Time.time;
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, patrolPoints[0].transform.position, Time.deltaTime * chargeSpeed);
            if (Vector2.Distance(enemy.transform.position, patrolPoints[0].position) < .2f)
            {

                hasJumped = false;
                shouldCharge = false; 
                chargingLeft = false;
                if(patrol)
                {
                    patrolling = true;
                }
            }
            
        }
        if (chargingRight && isInZone && Time.time > timeOfJump + timeBetweenJumpAndCharge)
        {
            timeOfCharge = Time.time;
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, patrolPoints[1].transform.position, Time.deltaTime * chargeSpeed);
            if (Vector2.Distance(enemy.transform.position, patrolPoints[1].position) < .2f)
            {
                hasJumped = false;
                shouldCharge = false;
                chargingRight = false;
                if (patrol)
                {
                    patrolling = true;
                }
            }

        }
    }

    private void Fire()
    {
        GameObject bullet = Instantiate((GameObject)Resources.Load("Bullet"), enemy.transform.position, Quaternion.identity);
        BulletController bulletStats = bullet.GetComponent<BulletController>();
        bulletStats.range = range;
        bulletStats.speed = projectileSpeed;
        bulletStats.enemyWhoFiredThis = enemy;
        bulletStats.damage = projectileDamage;
    }



    IEnumerator Shake()
    {
        Vector2 orignalPos = enemy.transform.position;
        for (int i = 0; i < 10; i++)
        {
            if(i % 2 == 0)
            {
                enemy.transform.position = new Vector2(orignalPos.x + 0.1f, enemy.transform.position.y);
            }
            else
            {
                enemy.transform.position = new Vector2(orignalPos.x - 0.1f, enemy.transform.position.y);
            }
            yield return null;
            yield return null;
        }
    }
}
