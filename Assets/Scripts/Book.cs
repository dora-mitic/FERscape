using UnityEngine;
using UnityEngine.UI;


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
        Debug.Log("Book Awake pozvan na: " + gameObject.name + " boja: " + color);
        baseScale = transform.localScale;
       
        Button button = GetComponent<Button>();
        Debug.Log("Book Awake - Button pronađen: " + (button != null));
       
        if (button != null)
        {
            button.onClick.AddListener(OnBookClicked);
            Debug.Log("Event dodan!");
        }
        else
        {
            Debug.LogError("GREŠKA: Nema Button komponente na " + gameObject.name);
        }
    }


    public void RefreshBaseScale()
    {
        baseScale = transform.localScale;
    }


    public void OnBookClicked()
    {
        Debug.Log("OnBookClicked pozvan!");
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