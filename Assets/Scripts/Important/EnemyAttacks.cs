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
    private int currentPointIndex;
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
    private bool jumpChargeCollison;
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
    private bool chargingLeft, chargingRight;

    [Header("Stats for Jump Charge ")]
    public float firingAngle;
    public float gravity;
    public float torque;
    private bool jumpCharging;
    

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
        if (charge)
        {
            enemy.transform.position = patrolPoints[0].transform.position;
            currentPointIndex = 1;
        }
        rb = enemy.GetComponent<Rigidbody2D>();
        if (patrol)
        {
            patrolling = true;
        }
    }


    void Update()
    {
        if (patrolling)
        {
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, patrolPoints[currentPointIndex].transform.position, Time.deltaTime * moveSpeed);
            if (Vector2.Distance(enemy.transform.position, patrolPoints[currentPointIndex].position) < .2f)
            {
                if (currentPointIndex == 0) { currentPointIndex = 1; }
                else if (currentPointIndex == 1) { currentPointIndex = 0; }
            }

        }

        if (projectile)
        {
            if (Time.time > timeOfShot + shootSpeed)
            {
                Fire();
                timeOfShot = Time.time;
            }
        }
        if (charge && !inFlight)
        {
            Debug.Log("Charge Loop Fired");
            float distanceFromZero = Vector2.Distance(patrolPoints[0].transform.position, player.transform.position);
            float distanceFromOne = Vector2.Distance(patrolPoints[1].transform.position, player.transform.position);
            bool isInYOfPointZero = patrolPoints[0].position.y + 2 > player.transform.position.y && patrolPoints[0].position.y - 2 < player.transform.position.y;
            bool isInYOfPointOne = patrolPoints[1].position.y + 2 > player.transform.position.y && patrolPoints[1].position.y - 2 < player.transform.position.y;
            isInZone = distanceFromZero < distanceBetweenPoints && distanceFromOne < distanceBetweenPoints && isInYOfPointZero && isInYOfPointOne;
            Vector3 playerPosInZone = player.transform.position;
            if (isInZone && !hasJumped && Time.time > timeOfCharge + chargeCoolDown)
            {
                if (patrol)
                {
                    patrolling = false;
                }
                hasJumped = true;
                timeOfJump = Time.time;
                StartCoroutine(Shake());
                if (player.transform.position.x < enemy.transform.position.x)
                {
                    if (!JumpCharge)
                    {
                        chargingLeft = true;
                    }
                    else
                    {
                        jumpCharging = true;
                    }
                }
                else
                {
                    if (!JumpCharge)
                    {
                        chargingRight = true;
                    }
                    else
                    {
                        jumpCharging = true;
                    }

                }
                if (doJump)
                {
                    rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                }
            }
            if (chargingLeft && isInZone && Time.time > timeOfJump + timeBetweenJumpAndCharge)
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
            if (chargingRight && isInZone && Time.time > timeOfJump + timeBetweenJumpAndCharge)
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
            if (jumpCharging && isInZone && Time.time > timeOfJump + timeBetweenJumpAndCharge)
            {
                timeOfCharge = Time.time;
                jumpCharging = false;
                hasJumped = false;

                
                inFlight = true;

                float target_Distance = Vector3.Distance(enemy.transform.position, player.transform.position);
                Vector2 direction = (player.transform.position - (enemy.transform.position)).normalized;


                // Calculate the velocity needed to throw the object to the target at specified angle.
                float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / (Physics2D.gravity.y * -1));

                // Extract the X Y componenent of the velocity
                float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
                float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

                Vector2 vel = new Vector2(Vx * direction.x, Vy);

                rb.AddForce(vel, ForceMode2D.Impulse);
                rb.GetComponent<Rigidbody2D>().AddTorque(torque);


                if (patrol)
                {
                    StartCoroutine(TurnOffPatrollingForTime(2f));
                }
                

                waitingForCollision = true;
                inFlight = false;
            }
            
 
        }
        if (jumpChargeCollison)
        {
            rb.velocity = Vector3.zero;
            jumpChargeCollison = false;
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
    /*
    IEnumerator DoJumpCharge(Vector3 target, bool left)
    {
        int leftMultiplier;
        if(left)
        {
            leftMultiplier = -1;
        }
        else
        {
            leftMultiplier = 1;
        }
        waitingForCollision = true;
        inFlight = true;
        // Calculate distance to target
        float target_Distance = Vector3.Distance(enemy.transform.position, target);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;


        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            if (jumpChargeCollison)
            {
                rb.velocity = new Vector3(0, 0, 0);
                jumpChargeCollison = false;
                inFlight = false;
                patrolling = true;
                break;
            }
            enemy.transform.Translate(((Vy - (gravity * elapse_time)) * Time.deltaTime), Vx * Time.deltaTime, 0);

            elapse_time += Time.deltaTime;

            yield return null;
        }

        rb.velocity = new Vector3(0, 0, 0);
        jumpChargeCollison = false;

        inFlight = false;
        patrolling = true;
            
        Debug.Log("Finished Jump Charge");
    }
    */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Floor" && waitingForCollision)
        { 
            jumpChargeCollison = true;
            waitingForCollision = false;
        }
    }
}



