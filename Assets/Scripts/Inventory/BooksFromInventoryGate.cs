using UnityEngine;

public class BooksFromInventoryGate : MonoBehaviour
{
    public static BooksFromInventoryGate Instance;

    public BookInventoryDropSlot whiteDropSlot;
    public BookInventoryDropSlot blackDropSlot;
    public BooksManager booksManager;
    
    public GameObject whiteBookPrefab;
    public GameObject blackBookPrefab;

    private void Awake()
    {
        Instance = this;
        
        // Postavi sve prefabe na obje drop slot komponente
        if (whiteDropSlot != null)
        {
            whiteDropSlot.whiteBookPrefab = whiteBookPrefab;
            whiteDropSlot.blackBookPrefab = blackBookPrefab;
            Debug.Log("whiteDropSlot prefabi postavljeni");
        }
        
        if (blackDropSlot != null)
        {
            blackDropSlot.whiteBookPrefab = whiteBookPrefab;
            blackDropSlot.blackBookPrefab = blackBookPrefab;
            Debug.Log("blackDropSlot prefabi postavljeni");
        }
    }

    public void OnSlotFilled()
    {
        Debug.Log("OnSlotFilled pozvan - white: " + whiteDropSlot.isFilled + " black: " + blackDropSlot.isFilled);
        if (whiteDropSlot.isFilled && blackDropSlot.isFilled)
        {
            Debug.Log("Obje knjige iz inventoryja dodane – otključaj puzzle!");
            booksManager.EnablePuzzle();
        }
    }
}
