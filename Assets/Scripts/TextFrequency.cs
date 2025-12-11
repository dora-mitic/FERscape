using UnityEngine;
using TMPro;
public class TextFrequency : MonoBehaviour
{
    TMP_Text text;

    void Start()
    {
        text = GetComponent<TMP_Text>();

        if(text == null)
        {
            Debug.LogError("TMP_Text component not found!");
        }
    }



    public void NewFrequency(string frequency)
    {
        text.text = frequency;
    }

    public string get() {
        return text.text;
    }
}
