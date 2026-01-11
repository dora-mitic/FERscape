using UnityEngine;

public class BooksFromInventoryGate : MonoBehaviour
{
    public static BooksFromInventoryGate Instance;

    public BookInventoryDropSlot whiteDropSlot;
    public BookInventoryDropSlot blackDropSlot;
    public BooksManager booksManager; // referenca na tvoj BooksManager

    private void Awake()
    {
        Instance = this;
    }

    public void OnSlotFilled()
    {
        if (whiteDropSlot.isFilled && blackDropSlot.isFilled)
        {
            Debug.Log("Obje knjige iz inventoryja dodane – otključaj puzzle!");
            booksManager.EnablePuzzle();
        }
    }
}
