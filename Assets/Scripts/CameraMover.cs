using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Camera Cam;
    public GameObject camLocation;
    public GameObject mainRoom;
    public GameObject leftRoom;
    public GameObject rightRoom;
    public GameObject topRoom;

    // Start is called before the first frame update
    void Start()
    {
        Cam.orthographicSize = 25f;
        camLocation.transform.position = mainRoom.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && this.gameObject.name == "LeftEnterDoor")
        {
            Cam.orthographicSize = 19f;
            camLocation.transform.position = leftRoom.transform.position;
        }

        if (collision.gameObject.tag == "Player" && this.gameObject.tag == "ExitDoor")
        {
            Cam.orthographicSize = 25f;
            camLocation.transform.position = mainRoom.transform.position;
        }

        if (collision.gameObject.tag == "Player" && this.gameObject.name == "TopEnterDoor")
        {
            Cam.orthographicSize = 25f;
            camLocation.transform.position = topRoom.transform.position;
        }

        if (collision.gameObject.tag == "Player" && this.gameObject.name == "RightEnterDoor")
        {
            Cam.orthographicSize = 19f;
            camLocation.transform.position = leftRoom.transform.position;
        }
    }
}
