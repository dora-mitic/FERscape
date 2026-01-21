using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class LockDropZone : MonoBehaviour, IDropHandler
{
    [Header("Koji ItemType prihvaća ova drop zona?")]
    public ItemType requiredType;

    public GameObject KeyObjekt;

    public GameObject FinalDoorObjekt;

    public GameObject Zoomed;

    public Animator animator;

    public GameObject lozinkaObjekt;

    public SFXManager sfxManager;

    public bool isOpened = false;

    IEnumerator WaitOneSecond()
    {
        Debug.Log("Čekam 1 sekundu...");
        yield return new WaitForSeconds(2f);
        Debug.Log("Gotovo!");
        sfxManager.PlayElectricDamage();
        yield return new WaitForSeconds(1f);
        Zoomed.SetActive(false);
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
            KeyObjekt.SetActive(true);
            animator.SetTrigger("KeyIn");


            
            StartCoroutine(WaitOneSecond());

            isOpened = true;


        }
        else
        {
            Debug.Log("Pogrešan item treba biti ovo: " + requiredType);
        }
    }
}
