using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DoorMonitor : MonoBehaviour
{
    private bool CanEnterDoor = false;
    public GameObject player;
    public string nextScene;
    public string currentScene;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            CanEnterDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            CanEnterDoor = false;
        }
    }

    //Sends person to new scene if they are on the door and press enter
    private void FixedUpdate()
    {
        if (CanEnterDoor == true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PlayerPrefs.SetString("Previous Scene", currentScene+" Door");
                Debug.Log(SceneManager.GetActiveScene().ToString() + " Door");
                SceneManager.LoadScene(nextScene);
            }
        }
    }
}

