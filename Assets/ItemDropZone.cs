using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class ItemDropZone : MonoBehaviour, IDropHandler
{
    [Header("Koji ItemType prihvaća ova drop zona?")]
    public ItemType requiredType;

    public GameObject lozinkaObjekt;

    public GameObject USBObjekt;

    public Animator animator;

    IEnumerator WaitOneSecond()
    {
        Debug.Log("Čekam 1 sekundu...");
        yield return new WaitForSeconds(2f);
        Debug.Log("Gotovo!");
        lozinkaObjekt.SetActive(true);
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
            USBObjekt.SetActive(true);
            animator.SetTrigger("PluggIn");
            
            StartCoroutine(WaitOneSecond());


        }
        else
        {
            Debug.Log("Pogrešan item: " + droppedItem.item.type);
        }
    }
}
