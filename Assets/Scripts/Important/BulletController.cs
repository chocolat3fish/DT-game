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

    private void Awake()
    {
        player = FindObjectOfType<PlayerControls>().gameObject;
    }

    private void Start()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        Vector2 moveDirection = (player.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, enemyWhoFiredThis.transform.position) > range)
        {
            Debug.Log(Vector2.Distance(transform.position, enemyWhoFiredThis.transform.position));
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            if (Time.timeScale != 0f)
            {

                //calculates how much damage to apply to the character
                float enemyAtackDamage = damage - player.GetComponent<PlayerControls>().defence;

                // if the players defence cancels out the enemys attack to much i.e. making it negative sets the damage to 0.1
                if (enemyAtackDamage < 0.1)
                {
                    enemyAtackDamage = 0.1f;
                }

                //Applys the Damage
                player.GetComponent<PlayerControls>().currentHealth -= enemyAtackDamage;
                Destroy(gameObject);
            }
         

        }
        else if (collision.gameObject != enemyWhoFiredThis)
        {
            Destroy(gameObject);
        }
    }

}
