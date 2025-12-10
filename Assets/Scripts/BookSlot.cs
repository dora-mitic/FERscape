using UnityEngine;

public class BookSlot : MonoBehaviour
{
    public int index;
    public BookColor requiredColor;
    public Book currentBook;
    
    public Vector3 bookScale = new Vector3(0.4f, 0.18f, 1f);

    private void OnValidate()
    {
        // Poziva se svaki put kad nešto promijeniš u Inspectoru
        if (currentBook != null)
        {
            SetBook(currentBook);
        }
    }

    public void SetBook(Book book)
    {
        currentBook = book;
        if (book != null)
        {
            book.CurrentSlot = this;

            book.transform.SetParent(transform);
            book.transform.localPosition = Vector3.zero;
            book.transform.localScale = bookScale;

            book.RefreshBaseScale();   // nakon što smo postavili bookScale
        }
    }


    public bool IsCorrect()
    {
        return currentBook != null && currentBook.color == requiredColor;
    }
}
