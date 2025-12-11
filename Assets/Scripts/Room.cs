using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public string roomName;
    public GameManager gameManager;

    // lista svih interaktivnih objekata u sobi
    private List<InteractiveObject> objekti = new List<InteractiveObject>();

    void Awake()
    {
        // automatski pronađi sve interaktivne objekte unutar sobe
        objekti.AddRange(GetComponentsInChildren<InteractiveObject>(true));
    }
}

