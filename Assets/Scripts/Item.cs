using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item : InteractiveObject
{
    public string itemName;
    public GameManager gameManager;

    public override void Interact()
    {
        Debug.Log("Picked up: " + itemName);
        gameObject.SetActive(false);
    }
}
