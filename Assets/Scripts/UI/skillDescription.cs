using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class skillDescription : MonoBehaviour
{
    public Vector3 offest;
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    void Update()
    { 
        transform.position = Input.mousePosition + offest;
        

    }
}
