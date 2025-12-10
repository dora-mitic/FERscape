using UnityEngine;
using UnityEngine.UI;
using TMPro;   // ako koristiš TextMeshPro

public class LockController : MonoBehaviour
{
    [Header("UI reference")]
    public TextMeshProUGUI[] digitTexts;   // 4 polja iznad
    public Button[] buttons;               // 4 gumba ispod

    [Header("Raspon znamenki (uključivo)")]
    public int minDigit = 0;   // npr. 0
    public int maxDigit = 1;   // npr. 1 za binarno, 9 za decimalno

    [Header("Tocna kombinacija")]
    public string correctCode = "1010";    // ili "1234", kako želiš

    private int[] currentDigits;

    private void Awake()
    {
        currentDigits = new int[digitTexts.Length];

        // inicijaliziraj na minDigit
        for (int i = 0; i < currentDigits.Length; i++)
        {
            currentDigits[i] = minDigit;
            digitTexts[i].text = currentDigits[i].ToString();
        }

        // spoji gumbe na handler
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // capture
            buttons[i].onClick.AddListener(() => OnButtonPressed(index));
        }
    }

    private void OnButtonPressed(int index)
    {
        // uvecaj znamenku i wrap-aj u rasponu
        currentDigits[index]++;
        if (currentDigits[index] > maxDigit)
            currentDigits[index] = minDigit;

        digitTexts[index].text = currentDigits[index].ToString();

        CheckCode();
    }

    private void CheckCode()
    {
        string code = "";
        for (int i = 0; i < currentDigits.Length; i++)
            code += currentDigits[i].ToString();

        if (code == correctCode)
        {
            Debug.Log("Lokot otkljucan! Kod = " + code);
            // ovdje pokreni otvaranje vrata / promjenu scene itd.
        }
    }
}
