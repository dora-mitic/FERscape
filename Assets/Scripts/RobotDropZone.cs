using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
public class RobotDropZone : MonoBehaviour, IDropHandler
{
    [Header("Koji ItemType prihvaća ova drop zona?")]
    public ItemType requiredType;

    public GameObject BaterijaObjekt;

    public bool isActivated = false;

    public void Start()
    {
        Transform dijete = transform.Find("Baterija");
        if (dijete.gameObject.activeSelf == true) {
            isActivated = true;
        }
    }
    

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (droppedItem == null)
            return;

        if (droppedItem.item.type == requiredType)
        {
            Debug.Log("Ispravan item spušten: " + droppedItem.item.type);
            Destroy(droppedItem.gameObject);
            BaterijaObjekt.SetActive(true);
            isActivated = true;
        }
        else
        {
            Debug.Log("Pogrešan item: " + droppedItem.item.type);
        }
    }
 
    // Start is called once before the first execution of Update after the MonoBehaviour is create
}
