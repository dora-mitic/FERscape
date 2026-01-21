using UnityEngine;
using TMPro;

public class DKAController : MonoBehaviour
{
    // pocetno stanje (ne error)
    public int startState = 1;

    // trenutno stanje
    public int currentState;

    // tocno stanje
    public int winState = 7;
    public Animator animator;

    public GameObject Zoomed;

    public AudioClip correctSound;

    public GameObject Office;

    public GameObject scriptHolder;

    public TMP_Text stateDisplayText;





    // prijelazi: [stanje, input]
    // input: 1, 2, 3 -> indeksi 0,1,2
    // stanje 0 = error - vracanje na prizemlje
    private int[,] prijelazi =
    {
        // 1  2  3
        { 0, 0, 0 }, // stanje 0 (error)
        { 3, 2, 0 }, // stanje 1
        { 5, 0, 1 }, // stanje 2
        { 3, 4, 0 }, // stanje 3 (krivi kat)
        { 6, 3, 5 }, // stanje 4
        { 0, 5, 6 }, // stanje 5 (krivi kat)
        { 0, 7, 7 }, // stanje 6
        { 7, 7, 7 }  // stanje 7 (tocan kat)
    };

    void Start()
    {
        ResetDKA();
    }

    // gumb 1
    public void Input1()
    {
        prijelaz(1);
    }

    // gumb 2
    public void Input2()
    {
        prijelaz(2);
    }

    // gumb 3
    public void Input3()
    {
        prijelaz(3);
    }

    // gumb 0 ili error
    public void ResetDKA()
    {
        currentState = startState;
        UpdateStateDisplay();
        Debug.Log("reset -> stanje " + currentState);
    }

    private void prijelaz(int input)
    {
        int inputIndex = input - 1;
        int nextState = prijelazi[currentState, inputIndex];

        if (nextState == 0)
        {
            Debug.Log("error prijelaz -> reset");
            ResetDKA();
            Debug.Log("Kraj puzzlea, kreni ispocetka");
            return;
        }

        currentState = nextState;
        UpdateStateDisplay();
        Debug.Log("input " + input + " -> stanje " + currentState);

        provjeriStanje();
    }

    private void UpdateStateDisplay()
    {
        if (stateDisplayText != null)
            stateDisplayText.text = "Stanje: " + currentState;
    }

    private void provjeriStanje()
    {
        if(currentState == 3 | currentState == 5){
            Debug.Log("Tocno stanje, krivi kat - ovdje ide text bubble kada implementiramo");
            return;
        }
        if (currentState == winState )
        {
            Debug.Log("pobjeda - tocno stanje 7");

            animator.SetTrigger("OpenLiftDoor");

            Zoomed.SetActive(false);

            AudioSource.PlayClipAtPoint(correctSound, Camera.main.transform.position);

            Office.SetActive(true);

            scriptHolder.SetActive(false);
        }
    }
}
