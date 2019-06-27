using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class EnemyStats : MonoBehaviour
{

    private Color32 tough = new Color32(255, 0, 0, 255);
    private Color32 average = new Color32(255, 255, 0, 255);
    private Color32 easy = new Color32(0, 255, 0, 255);

    private TextMeshProUGUI enemyLevel;
    private TextMeshProUGUI enemyName;
    private TextMeshProUGUI enemyHealth;
    private EnemyMonitor parent;

    void Start()
    {
        parent = GetComponentInParent<EnemyMonitor>();
        enemyLevel = gameObject.transform.Find("Level").GetComponent<TextMeshProUGUI>();
        enemyName = gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        enemyHealth = gameObject.transform.Find("Health").GetComponent<TextMeshProUGUI>();

        enemyName.text = parent.enemyStats.enemyName;

    }
    private void Update()
    {
        enemyLevel.text = "Lvl: " + parent.enemyStats.enemyLevel;
        enemyHealth.text = Math.Round(parent.currentHealth, 2).ToString() + " / " + Math.Round(parent.enemyStats.enemyHealth, 0).ToString();

        if (PersistantGameManager.Instance.playerStats.playerLevel >= parent.enemyStats.enemyLevel)
        {
            enemyLevel.color = easy;
        }
        else if (PersistantGameManager.Instance.playerStats.playerLevel - parent.enemyStats.enemyLevel > -5)
        {
            enemyLevel.color = average;
        }
        else
        {
            enemyLevel.color = tough;
        }
    }
}

