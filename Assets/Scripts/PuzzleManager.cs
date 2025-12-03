using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public DropSlot[] slots;
    public string nextSceneName = "SampleScene";

    bool levelCompleted = false;   // da se ne poziva više puta

    void Start()
    {
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

        if (goodCount == 2)
        {
            Debug.Log("Pogodena kombinacija, loadam scenu!");
            levelCompleted = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }

}
