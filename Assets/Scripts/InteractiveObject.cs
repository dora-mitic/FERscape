using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public string objectName;

    public abstract void Interact();

    void OnMouseDown() {
        Interact();
    }
}
