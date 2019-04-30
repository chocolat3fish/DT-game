using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {

    Vector3 localScale;
    public PlayerControls playerControls;
    public Color resistColour;
    public Color standard;
    //public GameObject healthBar;

    void Start()
    {
        playerControls = FindObjectOfType<PlayerControls>();
        //sets scale to default
        localScale = transform.localScale;
        GetComponent<Image>().color = standard;
    }

    void Update()
    {
        if (PersistantGameManager.Instance.damageResistMulti < 1)
        {
            GetComponent<Image>().color = resistColour;

        }
        else
        {
            GetComponent<Image>().color = standard;
        }
        //as health depletes, scales the x value down relative to health
        //uses a Lerp to smooth the depletion
        localScale.x = playerControls.currentHealth / playerControls.totalHealth;
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(localScale.x, transform.localScale.y, transform.localScale.z), 0.1f);
        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
        }
    }
}