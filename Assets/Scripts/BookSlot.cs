using UnityEngine;

public class BookSlot : MonoBehaviour
{
    public int index;
    public BookColor requiredColor;
    public Book currentBook;
    
    public Vector3 bookScale = new Vector3(0.01f, 0.01f, 1f);

    private void Start()
    {
        // Postavi knjigu kada se scene učita
        if (currentBook != null)
        {
            SetBook(currentBook);
        }
    }

    public void SetBook(Book book)
    {
        Debug.Log("SetBook pozvan na slotu " + index + " s knjigom: " + (book != null ? book.gameObject.name : "null"));
        currentBook = book;
        if (book != null)
        {
            
            
            book.CurrentSlot = this;
            book.transform.SetParent(transform);
            book.transform.localPosition = Vector3.zero;
            book.transform.localScale = bookScale;
            
            
            book.RefreshBaseScale();
            Debug.Log("Knjiga " + book.gameObject.name + " je stavljena u slot " + index);
        }
    }


    public bool IsCorrect()
    {
        return currentBook != null && currentBook.color == requiredColor;
    }
}
