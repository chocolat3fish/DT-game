using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script makes a game object fly towards the player
public class BulletController : MonoBehaviour
{
    private GameObject player;
    public float range;
    public float speed;
    public float damage;
    public GameObject enemyWhoFiredThis;
    public Vector2 startPosition;
    private Vector2 launchAtPos;

    private void Awake()
    {
        player = FindObjectOfType<PlayerControls>().gameObject;
        startPosition = transform.position;
        //specified where the projectile should go to
        launchAtPos = (player.transform.position - transform.position) * 100;
        //Starts the projectiles movement
        transform.position = Vector2.MoveTowards(transform.position, launchAtPos, Time.deltaTime * speed);
    }

    private void Update()
    {
        //Keeps the projectile moving
        transform.position = Vector2.MoveTowards(transform.position, launchAtPos, Time.deltaTime * speed);


        //If the projectile is to far away from the enemy it disappers or if the enemy is dead if it is too far away from the start position
        try
        {
            if (Vector2.Distance(transform.position, enemyWhoFiredThis.transform.position) > range)
            {
                Destroy(gameObject);
            }
        }

        catch (MissingReferenceException)
        {
            if (Vector2.Distance(transform.position, startPosition) > range)
            {
                Destroy(gameObject);
            }
        }
        


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If it hits the player deal damage
        if (collision.gameObject == player)
        {
            if (Time.timeScale != 0f)
            {

                //calculates how much damage to apply to the character
                float enemyAttackDamage = damage * PersistantGameManager.Instance.damageResistMulti;

                // if the players defence cancels out the enemys attack to much i.e. making it negative sets the damage to 0.1
                if (enemyAttackDamage < 0.1)
                {
                    enemyAttackDamage = 0.1f;
                }
                player.GetComponent<PlayerControls>().attackTime = Time.time;

                //Applys the Damage
                player.GetComponent<PlayerControls>().currentHealth -= enemyAttackDamage;
                Destroy(gameObject);
            }
         

        }
        //If it hits anything other than the enemy that fired it destroys itself
        else if (collision.gameObject != enemyWhoFiredThis)
        {
            Destroy(gameObject);
        }
    }

    //Some math for calculating angles, returns a number which is the angle between 2 points
    private float FindAngleBetweenPoints(Vector2 A, Vector2 B)
    {
        Vector2 C = B - A;
        float Angle = Mathf.Atan2(C.y, C.x);
        float AngleInDegrees = Angle * Mathf.Rad2Deg;
        return AngleInDegrees;
    }

}
