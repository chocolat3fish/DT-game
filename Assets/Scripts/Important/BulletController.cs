using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        launchAtPos = (player.transform.position - transform.position) * 100;
        print(launchAtPos);
        transform.position = Vector2.MoveTowards(transform.position, launchAtPos, Time.deltaTime * speed);
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, launchAtPos, Time.deltaTime * speed);

        try
        {
            if (Vector2.Distance(transform.position, enemyWhoFiredThis.transform.position) > range)
            {
                Debug.Log(Vector2.Distance(transform.position, enemyWhoFiredThis.transform.position));
                Destroy(gameObject);
            }
        }

        catch (MissingReferenceException)
        {
            if (Vector2.Distance(transform.position, startPosition) > range)
            {
                Debug.Log("no error");
                Destroy(gameObject);
            }
        }
        


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            if (Time.timeScale != 0f)
            {

                //calculates how much damage to apply to the character
                float enemyAttackDamage = damage - player.GetComponent<PlayerControls>().defence;

                // if the players defence cancels out the enemys attack to much i.e. making it negative sets the damage to 0.1
                if (enemyAttackDamage < 0.1)
                {
                    enemyAttackDamage = 0.1f;
                }

                //Applys the Damage
                player.GetComponent<PlayerControls>().currentHealth -= enemyAttackDamage;
                Destroy(gameObject);
            }
         

        }
        else if (collision.gameObject != enemyWhoFiredThis)
        {
            Destroy(gameObject);
        }
    }
    private float FindAngleBetweenPoints(Vector2 A, Vector2 B)
    {
        Vector2 C = B - A;
        float Angle = Mathf.Atan2(C.y, C.x);
        float AngleInDegrees = Angle * Mathf.Rad2Deg;
        return AngleInDegrees;
    }

}
