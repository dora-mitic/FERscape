using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("UI")]
    [HideInInspector] public Item item;
    public Image image;

    public ItemType requiredType;

    [HideInInspector] public Transform parentAfterDrag;

    public void Start()
    {
        InitialiseItem(item);
    }
    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
    }

    // Drag and drop
    public void OnBeginDrag(PointerEventData eventData) {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("UI element kliknut!");
        if (item.type != requiredType) return;
        var canvas = GameObject.Find("Canvas");              // ime točno kao u Hierarchy
        GameObject collectedZoomed = canvas.transform
                              .Find("CollectedZoomed")
                              .gameObject;
        collectedZoomed.SetActive(true);
    }
}
