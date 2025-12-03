using UnityEngine;
using UnityEngine.SceneManagement; // Obavezno!

public class OtvoriKutijuNaZidu : MonoBehaviour
{
    public string OtvorenaKutijaNaZidu;

    void OnMouseDown()
    {
        SceneManager.LoadScene("OtvorenaKutijaNaZidu");
    }
}
