using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AreaRequirements : MonoBehaviour
{
    public Vector3 offest;
    private void Awake()
    {
        Text text = transform.Find("Message").GetComponent<Text>();
        PlatformController[] controllers = FindObjectsOfType<PlatformController>();
        foreach(PlatformController controller in controllers)
        {
            controller.panel = gameObject;
            controller.message = text;
        }
        gameObject.SetActive(false);
    }
    void Update()
    { 
        transform.position = Input.mousePosition + offest;
        

    }
}
