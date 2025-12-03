using UnityEngine;
using UnityEngine.SceneManagement; // Obavezno!

public class OtvoriKartonskuKutiju : MonoBehaviour
{
    public string OtvorenaKutija;

    void OnMouseDown()
    {
        SceneManager.LoadScene("OtvorenaKutija");
    }
}
