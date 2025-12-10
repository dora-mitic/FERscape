using UnityEngine;

public enum BookColor
{
    White = 0,
    Black = 1
}

public class Book : MonoBehaviour
{
    public BookColor color;
    public BookSlot CurrentSlot;

    private Vector3 baseScale;

    private void Awake()
    {
        // spremi trenutni scale kao bazu (postavlja ga BookSlot.SetBook)
        baseScale = transform.localScale;
    }

    public void RefreshBaseScale()
    {
        // pozvat ćemo ovo iz SetBook nakon što promijeni scale
        baseScale = transform.localScale;
    }

    private void OnMouseDown()
    {
        if (BooksManager.Instance != null)
        {
            BooksManager.Instance.OnBookClicked(this);
        }
    }

    public void Highlight(bool on)
    {
        transform.localScale = on ? baseScale * 1.05f : baseScale;
    }
}
