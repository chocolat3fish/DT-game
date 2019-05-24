using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//******MUST BE CHILDED TO A GAMEOBJECT ACTING AS A DOOR THE DOOR MUST BE NAMED "scene to be traveled to and from" Door ******\\

//A script for sending people to and from scences via doors
public class DoorMonitor : MonoBehaviour
{
    //Player
    public GameObject player;
    //Name of the nextScene and the currentScene
    public string nextScene;
    public string currentScene;
    //Can the player go through the door
    private bool CanEnterDoor = false;

    private void Awake()
    {
        //Gets the player GameObject
        player = FindObjectOfType<PlayerControls>().gameObject;

        //Finds the scene the door is sending the player to
        nextScene = gameObject.name.Replace(" Door", "");
        //Gets the name of the current scene
        currentScene = SceneManager.GetActiveScene().name;
    }
    //Sends person to new scene if they are on the door and press enter
    private void Update()
    {
        if (CanEnterDoor == true)
        {
            if (Input.GetKeyDown(KeyCode.Return) && (SceneManager.GetActiveScene().name != "Tutorial" || PersistantGameManager.Instance.tutorialComplete) && Time.timeScale != 0f)
            {
               
                SceneManager.LoadScene(nextScene);
                PersistantGameManager.Instance.dialogueSceneIsOpen = false;
            }
        }
    }
    //If the player enters the doors trigger lets them enter
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            CanEnterDoor = true;
            print("CAN");
        }
    }
    //If the player enters the doors trigger lets them enter
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            CanEnterDoor = false;
            print("CANNOT");
        }
    }
}

