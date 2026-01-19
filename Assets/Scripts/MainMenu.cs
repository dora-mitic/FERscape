using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] Image darkness;
    [SerializeField, Min(0f)] private float minTimeBetween;
    [SerializeField, Min(0f)] private float maxTimeBetween;
    private float timeBetween = 0f;
    private float timer = 0f;

    public void zapocniIgru()
    {
        SceneManager.LoadScene("Knjiznica");
    }


    public void izadiIzIgre() 
    {
        Application.Quit();
    }

    public void Awake()
    {
        if(minTimeBetween > maxTimeBetween)
        {
            (maxTimeBetween, minTimeBetween) = (minTimeBetween, maxTimeBetween);
        }
    }

    public void Update()
    {
        if (Application.isPlaying)
        {
            timer += Time.deltaTime;
            if (timer >= timeBetween)
            {
                darkness.color = new Color(0, 0, 0, Random.Range(0, 0.8f));
                timer = 0f;
                timeBetween = Random.Range(minTimeBetween, maxTimeBetween);
            }
        }
    }
}
