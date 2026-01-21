using UnityEngine;
using UnityEngine.SceneManagement;

public class gumbi : MonoBehaviour
{
    public void zapocniIgru()
    {
        SceneManager.LoadScene("OtvorenPrekidac");   
    }

    public void ucitajMenu()
    {
        SceneManager.LoadScene("main menu");
    }


    public void izadiIzIgre() 
    {
        Application.Quit();
    }

}
