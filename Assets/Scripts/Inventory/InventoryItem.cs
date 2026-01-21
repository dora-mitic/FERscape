using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IDropHandler
{
    [Header("UI")]
    [HideInInspector] public Item item;
    public Image image;

    public ItemType requiredType1;
    public ItemType requiredType2;

    public ItemType requiredType3;
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
        if (item.type != requiredType1 && item.type != requiredType2 && item.type != requiredType3) return;
        var canvas = GameObject.Find("Canvas");  
        if (item.type == requiredType1)
        {         // ime točno kao u Hierarchy
            Debug.Log("papir kliknut!");
            GameObject collectedZoomed = canvas.transform
                              .Find("Library/CollectedZoomed")
                              .gameObject;
            collectedZoomed.SetActive(true);
            
        }
        if (item.type == requiredType2)
        {         // ime točno kao u Hierarchy
            GameObject collectedZoomed2 = canvas.transform
                              .Find("Hallway/CollectedZoomed2")
                              .gameObject;
            collectedZoomed2.SetActive(true);
        }
        if (item.type == requiredType3)
        {           // ime točno kao u Hierarchy
            Debug.Log("Robotic kliknut!");
            GameObject collectedZoomed3 = canvas.transform
                              .Find("Office/CollectedZoomed3")
                              .gameObject;
            collectedZoomed3.SetActive(true);
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (item.type == requiredType3) {
            var canvas = GameObject.Find("Canvas");  
            InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            if(droppedItem == null) return;
            if (droppedItem.item.type == requiredType)
            {
                Debug.Log("Ispravan item spušten: " + droppedItem.item.type);
                GameObject collectedZoomed3 = canvas.transform
                              .Find("Office/CollectedZoomed3")
                              .gameObject;
                collectedZoomed3.SetActive(true);
                Transform dijete = collectedZoomed3.transform.Find("Robot/Baterija");
                dijete.gameObject.SetActive(true);
                Destroy(droppedItem.gameObject);
            }
            else
            {
                Debug.Log("Pogrešan item: " + droppedItem.item.type);
            }
        }
    }
}
