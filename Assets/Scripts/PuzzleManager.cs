using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.Mathematics;

public class PuzzleManager : MonoBehaviour
{
    public DropSlot[] slots;
    public string nextSceneName = "Ferscape";

    bool levelCompleted = false;   // da se ne poziva više puta

    public Animator anim;
    public GameObject current;

    public GameObject currentSound;

    public GameObject mrak;

    void Start()
    {
        pocetakScene();
        CheckPuzzle();             // provjera odmah na početku
    }

    public void CheckPuzzle()
    {
        if (levelCompleted) return;

        int goodCount = 0;

        foreach (var slot in slots)
        {
            if (slot != null && slot.currentItem != null)
            {
                Debug.Log($"Slot {slot.name} drzi {slot.currentItem.name}, isCorrect = {slot.currentItem.isCorrectResistor}");

                if (slot.currentItem.isCorrectResistor)
                    goodCount++;
            }
            else
            {
                Debug.Log($"Slot {slot?.name} je prazan");
            }
        }

        Debug.Log($"goodCount = {goodCount}");

        IEnumerator WaitFiveSeconds()
        {
            Debug.Log("Čekam 1 sekundu...");
            yield return new WaitForSeconds(3.5f);
            Debug.Log("Gotovo!");
            Debug.Log("Pogodena kombinacija, loadam scenu!");
            levelCompleted = true;
            SceneManager.LoadScene(nextSceneName);
        }
        if (goodCount == 2)
        {
            current.SetActive(true);
            currentSound.SetActive(true);
            anim.SetTrigger("CurrentStart");
            StartCoroutine(WaitFiveSeconds());
        }
    }

    public void pocetakScene()
    {
        IEnumerator CekajKrajAnimacije()
        {
            Debug.Log("Čekam");
            yield return new WaitForSeconds(7f);
            mrak.SetActive(false);
        }
        StartCoroutine(CekajKrajAnimacije());
    }
}
