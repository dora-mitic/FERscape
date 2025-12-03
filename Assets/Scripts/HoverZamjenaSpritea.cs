using UnityEngine;

public class HoverZamjenaSpritea : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite hoverSprite;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite;
    }

    void OnMouseOver()
    {
        spriteRenderer.sprite = hoverSprite;
    }

    void OnMouseExit()
    {
        spriteRenderer.sprite = normalSprite;
    }
}
