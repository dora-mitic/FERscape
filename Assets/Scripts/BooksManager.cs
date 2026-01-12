using UnityEngine;
using UnityEngine.SceneManagement;

public class BooksManager : MonoBehaviour
{
    public static BooksManager Instance;

    private Book selectedBook;

    [Header("Sljedeća scena nakon rješenja (nije obavezno)")]
    public string nextSceneName = "";

    [Header("Puzzle Lock")]
    public bool puzzleEnabled = false;

    [Header("Drawer - otvara se KAD JE RJEŠENO")]
    public GameObject drawer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void EnablePuzzle()
    {
        puzzleEnabled = true;
        Debug.Log("Puzzle otključan!");
        // NEMA OTVARANJA LADICE OVDJE!
    }

    public void OnBookClicked(Book book)
    {
        if (!puzzleEnabled)
        {
            Debug.Log("Puzzle još zaključan - trebaš prvo donijeti knjige iz inventoryja!");
            return;
        }

        Debug.Log("OnBookClicked: " + book.name);

        // prvi klik – odabir
        if (selectedBook == null)
        {
            selectedBook = book;
            selectedBook.Highlight(true);
            return;
        }

        // klik na istu knjigu – poništi
        if (book == selectedBook)
        {
            selectedBook.Highlight(false);
            selectedBook = null;
            return;
        }

        // drugi klik – swap
        SwapBooks(selectedBook, book);

        selectedBook.Highlight(false);
        selectedBook = null;

        CheckSolved();
    }

    private void SwapBooks(Book a, Book b)
    {
        BookSlot slotA = a.CurrentSlot;
        BookSlot slotB = b.CurrentSlot;

        if (slotA == null || slotB == null)
        {
            Debug.LogWarning("Jedna od knjiga nema slot!");
            return;
        }

        slotA.SetBook(b);
        slotB.SetBook(a);
    }

    private void CheckSolved()
    {
        Debug.Log("CheckSolved pozvan");

        BookSlot[] slots = FindObjectsByType<BookSlot>(FindObjectsSortMode.None);

        foreach (BookSlot slot in slots)
        {
            if (slot.currentBook == null)
            {
                Debug.Log("Slot " + slot.index + " nema knjigu!");
                return;
            }

            if (!slot.IsCorrect())
            {
                Debug.Log("Krivo na slotu " + slot.index +
                          " (required " + slot.requiredColor +
                          ", current " + slot.currentBook.color + ")");
                return;
            }
        }

        Debug.Log("Knjige tocno poslozene – EXIT!");

        // OTVORI LADICU TEK NAKON RJEŠENJA
        if (drawer != null)
        {
            drawer.SetActive(true);
            Debug.Log("Ladica otvorena!");
        }

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
