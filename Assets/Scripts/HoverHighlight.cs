using UnityEngine;

public class HoverHighlight : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;
    public Color highlightColor = Color.yellow;
    public Texture2D clickableCursor;  // Postavi sliku kursora (npr. PNG s pointer rukom)
    private Texture2D defaultCursor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void OnMouseEnter()
    {
        sr.color = highlightColor; // Zasvijetli sprite
        Cursor.SetCursor(clickableCursor, Vector2.zero, CursorMode.Auto); // Promijeni cursor
    }

    void OnMouseExit()
    {
        sr.color = originalColor;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Vrati defaultni cursor
    }
}
