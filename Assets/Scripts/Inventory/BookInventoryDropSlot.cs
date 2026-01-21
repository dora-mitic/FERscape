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

        Debug.Log("Prebacujem knjugu: " + invItem.item.name);

        // provjera po imenu - sada prihvaćamo bilo koju boju
        BookColor droppedColor = invItem.item.name.Contains("Bijela") ? BookColor.White : BookColor.Black;

        // obriši item iz inventoryja
        Destroy(invItem.gameObject);
        Debug.Log("Provjera 1: prefabi");
        
        // kreiraj novu Book instance u puzzle
        GameObject bookPrefab = (droppedColor == BookColor.White) ? whiteBookPrefab : blackBookPrefab;
        Debug.Log("bookPrefab: " + (bookPrefab != null ? bookPrefab.name : "NULL"));
        Debug.Log("targetBookSlot: " + (targetBookSlot != null ? targetBookSlot.gameObject.name : "NULL"));
        // Kreira knjigu s targetBookSlot kao roditeljom (SetBook će to podesiti svejedno)
        GameObject newBookGo = Instantiate(bookPrefab, targetBookSlot.transform);
        Book newBook = newBookGo.GetComponent<Book>();
        // Postavi boju na osnovu prefaba koji je korišten
        newBook.color = droppedColor;
        
        Debug.Log("Stvorena nova knjiga: " + newBook.gameObject.name + " boja: " + newBook.color);

        // stavi je u slot
        targetBookSlot.SetBook(newBook);
        
        Debug.Log("Knjiga stavljena u slot!");

        isFilled = true;

        DestroyImmediate(gameObject);

        // provjeri je li puzzle gotov
        BooksFromInventoryGate.Instance.OnSlotFilled();
    }
}
