using UnityEngine;

public class CodeProjectClick : MonoBehaviour
{
    public GameObject lozinkaProjekt_0; // Povuci lozinku ovdje
    public GameObject desktopProjekt_1_0; // Povuci desktop ovdje

    void Start()
    {
        // Sakrij lozinku na startu
        if (lozinkaProjekt_0 != null)
        {
            lozinkaProjekt_0.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (lozinkaProjekt_0 != null)
        {
            lozinkaProjekt_0.SetActive(true); // Prikaži lozinku
        }

        if (desktopProjekt_1_0 != null)
        {
            desktopProjekt_1_0.SetActive(false); // Sakrij desktop
        }

        gameObject.SetActive(false); // Sakrij codeProjekt_0 (ovaj objekt)
    }
}