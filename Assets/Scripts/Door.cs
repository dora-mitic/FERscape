using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : InteractiveObject
{
    public GameObject targetRoom;
    public GameManager gameManager;

    public override void Interact()
    {
        gameManager.GoToRoom(targetRoom);
    }
}
