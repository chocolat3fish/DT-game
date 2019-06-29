using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAttacks : MonoBehaviour
{

    [HideInInspector]
    public List<Transform> patrolPoints = new List<Transform>();
    [HideInInspector]
    public GameObject player;
    public PlayerStats playerStats;
    public int currentPointIndex;
    private GameObject enemy;
    private float distanceBetweenPoints;
    [HideInInspector]
    public bool hasJumped;
    [HideInInspector]
    public Rigidbody2D rb;
    private float timeOfJump;
    [HideInInspector]
    public float timeOfShake;
    private bool patrolling;
    private bool isInZone;
    [HideInInspector]
    public bool inFlight;
    [HideInInspector]
    public Vector3 playerPos;
    public bool jumpChargeCollision;
    private bool waitingForCollision;

    [Header("Choose Type Of Enemy")]
    public bool projectile;
    public bool patrol;
    public bool charge;
    public bool JumpCharge;

    [Header("Stats for Patrol")]
    public float moveSpeed;

    [Header("Stats for Projectile")]
    public float shootSpeed;
    public float range;
    public float projectileSpeed;
    public float projectileDamage;
    private float timeOfShot;

    [Header("Stats for Charge")]
    public float chargeCoolDown;
    public float timeOfCharge;
    public float chargeSpeed;
    public bool doJump;
    public float jumpPower;
    public float timeBetweenJumpAndCharge;
    public bool chargingLeft, chargingRight;

    [Header("Stats for Jump Charge ")]
    public float firingAngle;
    public float gravity;
    public float torque;
    private bool jumpCharging;

    private float _firingAngle;

    private EnemyMonitor enemyMonitor;

    void Awake()
    {
        enemy = gameObject.transform.Find("Enemy").gameObject;
        enemyMonitor = enemy.GetComponent<EnemyMonitor>();
        player = FindObjectOfType<PlayerControls>().gameObject;
        rb = enemy.GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), enemy.GetComponent<BoxCollider2D>());

        //If the attack type needs patrol points it finds them, for redundency if it fails then debug it then destroy itself
        if (patrol || charge)
        {
            try
            {
                patrolPoints.Add(gameObject.transform.Find("Left Point"));
                patrolPoints.Add(gameObject.transform.Find("Right Point"));
            }
            catch
            {
                Debug.Log("!!!!!No Patrol Points for " + gameObject.name + "!!!!!");
                Destroy(gameObject);
            }
            
            distanceBetweenPoints = Vector2.Distance(patrolPoints[0].position, patrolPoints[1].position);
        }

        //if it is a charge enemy set it to the left most point then set it to move right
        if (charge)
        {
            enemy.transform.position = patrolPoints[0].transform.position;
            currentPointIndex = 1;
        }

        if (patrol)
        {
            patrolling = true;
        }

    }


    void Update()
    {

        if (patrolling)
        {
            //Move Back and forth between points
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, patrolPoints[currentPointIndex].transform.position, Time.deltaTime * moveSpeed);
            if (Vector2.Distance(enemy.transform.position, patrolPoints[currentPointIndex].position) < .2f)
            {
                //Makes the enemy face the right way and make it move towards the correct point
                if (currentPointIndex == 0) { currentPointIndex = 1; enemy.GetComponent<SpriteRenderer>().flipX = false; }
                else if (currentPointIndex == 1) { currentPointIndex = 0; enemy.GetComponent<SpriteRenderer>().flipX = true; }
            }

        }

        if (projectile)
        {
            //Face towards the player
            if (player.transform.position.x > enemy.transform.position.x)
            {
                enemy.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (player.transform.position.x < enemy.transform.position.x)
            {
                enemy.GetComponent<SpriteRenderer>().flipX = true;
            }

            //Launch shot at the correct delay
            if (Time.time > timeOfShot + shootSpeed)
            {
                Fire();
                timeOfShot = Time.time;

            }
        }
        //If is preparing for a charge
        if (charge && !inFlight)
        {
            //Check if the player is within the enemies zone
            float distanceFromZero = Vector2.Distance(patrolPoints[0].transform.position, player.transform.position);
            float distanceFromOne = Vector2.Distance(patrolPoints[1].transform.position, player.transform.position);
            bool isInYOfPointZero = patrolPoints[0].position.y + 2 > player.transform.position.y && patrolPoints[0].position.y - 2 < player.transform.position.y;
            bool isInYOfPointOne = patrolPoints[1].position.y + 2 > player.transform.position.y && patrolPoints[1].position.y - 2 < player.transform.position.y;
            isInZone = distanceFromZero < distanceBetweenPoints && distanceFromOne < distanceBetweenPoints && isInYOfPointZero && isInYOfPointOne;

            //If the player is in zone and hasn't jumped yet and should
            if (isInZone && !hasJumped && Time.time > timeOfCharge + chargeCoolDown)
            {
                //Stop moving if moving
                if (patrol)
                {
                    patrolling = false;
                }
                hasJumped = true;
                timeOfJump = Time.time;

                //Shake
                StartCoroutine(Shake());

                //Save the player position and works out the direction to charge or charge
                if (player.transform.position.x < enemy.transform.position.x)
                {
                    if (!JumpCharge)
                    {
                        chargingLeft = true;
                        playerPos = player.gameObject.transform.position;
                    }
                    else
                    {
                        jumpCharging = true;
                        playerPos = player.gameObject.transform.position;
                    }
                }
                else
                {
                    if (!JumpCharge)
                    {
                        chargingRight = true;
                        playerPos = player.gameObject.transform.position;
                    }
                    else
                    {
                        jumpCharging = true;
                        playerPos = player.gameObject.transform.position;
                    }

                }
                //Do a small jump if wanted
                if (doJump)
                {
                    rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                }
            }
            //Charge to the left
            if (chargingLeft && Time.time > timeOfJump + timeBetweenJumpAndCharge)
            {
                timeOfCharge = Time.time;
                enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, patrolPoints[0].transform.position, Time.deltaTime * chargeSpeed);
                if (Vector2.Distance(enemy.transform.position, patrolPoints[0].position) < .2f)
                {

                    hasJumped = false;
                    chargingLeft = false;
                    if (patrol)
                    {
                        patrolling = true;
                    }
                }

            }
            //Charge to the right
            if (chargingRight && Time.time > timeOfJump + timeBetweenJumpAndCharge)
            {
                timeOfCharge = Time.time;
                enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, patrolPoints[1].transform.position, Time.deltaTime * chargeSpeed);
                if (Vector2.Distance(enemy.transform.position, patrolPoints[1].position) < .2f)
                {
                    hasJumped = false;
                    chargingRight = false;
                    if (patrol)
                    {
                        patrolling = true;
                    }
                }

            }

            //Proform the jump charge
            if (jumpCharging && Time.time > timeOfJump + timeBetweenJumpAndCharge)
            { 
                float targetDistance = Vector3.Distance(enemy.transform.position, playerPos);

                if((enemy.transform.position.x - playerPos.x < -0.5 || enemy.transform.position.x - playerPos.x > 0.5))
                {
                    timeOfCharge = Time.time;
                    jumpCharging = false;
                    hasJumped = false;


                    inFlight = true;
                    if (targetDistance > 10f)
                    {
                        _firingAngle = firingAngle - 30;
                    }
                    else if (targetDistance > 7.5f)
                    {
                        _firingAngle = firingAngle - 22.5f;
                    }
                    else if (targetDistance > 5f)
                    {
                        _firingAngle = firingAngle - 15f;
                    }
                    else if (targetDistance > 2.5f)
                    {
                        _firingAngle = firingAngle - 7.5f;
                    }

                    Vector2 direction = (playerPos - (enemy.transform.position)).normalized;

                    // Calculate the velocity needed to throw the object to the target at specified angle.
                    float projectile_Velocity = targetDistance / (Mathf.Sin(2 * _firingAngle * Mathf.Deg2Rad) / (Physics2D.gravity.y * -1));

                    // Extract the X Y componenent of the velocity
                    float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(_firingAngle * Mathf.Deg2Rad);
                    float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(_firingAngle * Mathf.Deg2Rad);

                    Vector2 vel = new Vector2(Vx * direction.x, Vy);

                    //Stops errors which randomly occur
                    if(!float.IsPositiveInfinity(vel.x) && !float.IsPositiveInfinity(vel.y) && !float.IsNegativeInfinity(vel.x) && !float.IsNegativeInfinity(vel.y))
                    {
                        rb.AddForce(vel, ForceMode2D.Impulse);
                        rb.GetComponent<Rigidbody2D>().AddTorque(torque);
                    }
                    //makes the enemy not move for a little bit after jumping
                    if (patrol)
                    {
                        StartCoroutine(TurnOffPatrollingForTime(2.5f));
                        if (rb.velocity.x < 0)
                        {
                            currentPointIndex = 0;
                        }
                        else
                        {
                            currentPointIndex = 1;
                        }


                    }
                    enemyMonitor.waitingForCollision = true;
                        inFlight = false;
                        timeOfCharge = Time.time;


                }

            }


        }
        //Freezes the enemy when it touches down after a jump
        if (jumpChargeCollision)
        {
            rb.velocity = Vector3.zero;
            jumpChargeCollision = false;
        }
    }

        //Launches Projectile
        private void Fire()
        {

            float multiplier = 1;
            switch (enemyMonitor.enemyStats.enemyTier)
            {
                case "Light":
                    multiplier *= 0.8f;
                    break;

                case "Medium":
                    multiplier *= 1f;
                    break;

                case "Heavy":
                    multiplier *= 1.2f;
                    break;
            }

            if (PersistantGameManager.Instance.playerStats.playerLevel < 0)
            {
                projectileDamage = (float)(multiplier * (5 * Math.Pow(enemyMonitor.enemyStats.enemyLevel, 2) + 10) * 0.4f);
            }
            else
            {

                projectileDamage = (float)(multiplier * (5 * Math.Pow(enemyMonitor.enemyStats.enemyLevel, 2) + 10));
            }
            //Creates the bullet then assigns its stats
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
                if (i % 2 == 0)
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


    IEnumerator TurnOffPatrollingForTime(float time)
    {
        patrolling = false;
        yield return new WaitForSeconds(time);
        patrolling = true;
    }
   
}
