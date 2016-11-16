using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour {

    private bool isOpened;

    private Animator doorAnimation;

    private int roomIndex;
    private int doorDirection;

    private GameObject[] roomList;

    // Use this for initialization
    void Start () {
        doorAnimation = gameObject.GetComponent<Animator>();
        isOpened = false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player" && isOpened == false)
        {
            doorAnimation.SetFloat("State", 1);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            isOpened = true;
            OpenDoor();
        }
    }

    // Method called when door is opened to set visibility of connected room to true
    public void OpenDoor(){
        roomList = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().getRoomList();
        GameObject roomtoActivate;
        if(doorDirection == 0){
            roomtoActivate = roomList[roomIndex - 1];
            roomtoActivate.SetActive(true);
        } else if(doorDirection == 1){
            roomtoActivate = roomList[roomIndex - 10];
            roomtoActivate.SetActive(true);
        } else if(doorDirection == 2){
            roomtoActivate = roomList[roomIndex + 1];
            roomtoActivate.SetActive(true);
        } else if(doorDirection == 3){
            roomtoActivate = roomList[roomIndex + 10];
            roomtoActivate.SetActive(true);
        }
    }

    // Method called to assign some door information needed for OpenDoor()
    public void SetDoortoRoom(int index, int direction) {
        roomIndex = index;
        doorDirection = direction;
    }
}
