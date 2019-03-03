using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorMonitors : MonoBehaviour
{
    private BoxCollider2D leftCollider;
    private BoxCollider2D rightCollider;

    private PlayerControls playerControls;
    // Start is called before the first frame update
    void Start()
    {
        leftCollider = gameObject.transform.Find("Left Detector").GetComponent<BoxCollider2D>();
        rightCollider = gameObject.transform.Find("Right Detector").GetComponent<BoxCollider2D>();

        playerControls = gameObject.GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void Update()
    {
        leftCollider.size = new Vector2(playerControls.range, 2);
        leftCollider.offset = new Vector2((playerControls.range / -2) - 0.6f, 0);
        rightCollider.size = new Vector2(playerControls.range, 2);
        rightCollider.offset = new Vector2((playerControls.range / 2) + 0.6f, 0);
    }
}
