using UnityEngine;

public class GlavniIzbornikEsc : MonoBehaviour
{

    public GameObject IzbornikZoomed;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IzbornikZoomed.activeSelf)
            {
                IzbornikZoomed.SetActive(false);
            }
            else 
            { 
                IzbornikZoomed.SetActive(true);
            }
        }
    }
}
