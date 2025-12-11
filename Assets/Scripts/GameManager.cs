using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject room1;
    public GameObject room2;
    public GameObject room3;
    //public GameObject room4;

    //List<GameObject> rooms = Arrays.asList(room1, room2);

    private GameObject currentRoom;

    void Start()
    {
        currentRoom = room1;
        room1.gameObject.SetActive(true);
        room2.gameObject.SetActive(false);
        room3.gameObject.SetActive(false);
        //room4.gameObject.SetActive(false);
    }

    public void GoToRoom(GameObject nextRoom)
    {
        room1.gameObject.SetActive(false);
        room2.gameObject.SetActive(false);
        room3.gameObject.SetActive(false);
        nextRoom.SetActive(true);
        currentRoom = nextRoom;
    }

    public void PokreniPuzzle(GameObject puzzle){
        
    }
}

