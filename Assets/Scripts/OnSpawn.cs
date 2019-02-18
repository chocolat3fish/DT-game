using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSpawn : MonoBehaviour
{
    private string previousScene;
    public GameObject[] gameObjects;
    public Transform player;
    public bool StartCameraMovement;
    // Start is called before the first frame update
    void Start()
    {
        previousScene = PlayerPrefs.GetString("Previous Scene");
        foreach (GameObject door in gameObjects)
        {
            if (door.name == previousScene)
            {
                player.transform.position = door.transform.position;
                StartCameraMovement = true;
            }
        }
    }

    
}
