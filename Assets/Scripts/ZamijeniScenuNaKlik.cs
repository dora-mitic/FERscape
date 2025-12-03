using UnityEngine;
using UnityEngine.SceneManagement;

public class ZamijeniScenuNaKlik : MonoBehaviour
{
    public string OtvorenPrekidac;     // primjer: "UvecanaKutija"
    public string SampleScene;   // primjer: "Pocetna"

    void OnMouseDown()
    {
        string trenutnaScena = SceneManager.GetActiveScene().name;
        Debug.Log("Klik! Aktivna scena je: " + trenutnaScena);
        if (trenutnaScena == "SampleScene")
        {
            Debug.Log("Učitavam: " + OtvorenPrekidac);
            SceneManager.LoadScene("OtvorenPrekidac"); // idi na sljedeću
        }
        else if (trenutnaScena == "OtvorenPrekidac")
        {
            Debug.Log("Učitavam: " + SampleScene);
            SceneManager.LoadScene("SampleScene"); // vrati natrag
        }
    }
}
