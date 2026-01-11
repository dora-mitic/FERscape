using UnityEngine;
using UnityEngine.EventSystems;

public class BookInventoryDropSlot : MonoBehaviour, IDropHandler
{
    public BookColor requiredColor;
    [HideInInspector] public bool isFilled = false;
    
    public BookSlot targetBookSlot;
    public GameObject whiteBookPrefab;
    public GameObject blackBookPrefab;

    public void OnDrop(PointerEventData eventData)
    {
        if (isFilled) return;

        InventoryItem invItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (invItem == null) return;

        // provjera po imenu
        BookColor droppedColor = invItem.item.name.Contains("Bijela") ? BookColor.White : BookColor.Black;
        if (droppedColor != requiredColor) return;

        // obriši item iz inventoryja
        Destroy(invItem.gameObject);

        // kreiraj novu Book instance u puzzle
        GameObject bookPrefab = (droppedColor == BookColor.White) ? whiteBookPrefab : blackBookPrefab;
        GameObject newBookGo = Instantiate(bookPrefab);
        Book newBook = newBookGo.GetComponent<Book>();

        // stavi je u slot
        targetBookSlot.SetBook(newBook);

        isFilled = true;

        DestroyImmediate(gameObject);

        // provjeri je li puzzle gotov
        BooksFromInventoryGate.Instance.OnSlotFilled();
    }
}
