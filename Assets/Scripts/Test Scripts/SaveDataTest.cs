using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataTest : MonoBehaviour
{
    public PlayerStats playerStats;
    private void Start()
    {
        playerStats = PlayerStats.PlayerStatsConstrutor(5, 2, 3);
        Save_ReadData.SaveObjectToFile(playerStats, "Testing");
    }

}
