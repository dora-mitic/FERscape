using UnityEngine;
using UnityEngine.SceneManagement;

public class gumbi : MonoBehaviour
{

    public void zapocniIgru()
    {
        SceneManager.LoadScene("SampleScene");
    }


    public void izadiIzIgre() 
    {
        Application.Quit();
    }

}
