using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class RupaDropZone : MonoBehaviour, IDropHandler
{
    [Header("Koji ItemType prihvaća ova drop zona?")]
    public ItemType requiredType;

    public GameObject Objekt;

    public GameObject MainCamera;

    public GameObject Office;

    public GameObject EventSys;

    public GameObject Magnifier;

    public RobotDropZone robotDropZone;
    public GameObject textBezBaterije;

    public GameObject textBezRobota;
    public GameObject canvas;

    public GameObject AudioBezBaterije;

    public GameObject AudioBezRobota;

    IEnumerator WaitOneSecond()
    {
        Debug.Log("Čekam 1 sekundu...");
        yield return new WaitForSeconds(2.5f);
        Debug.Log("Gotovo!");
        textBezBaterije.SetActive(false);
        textBezRobota.SetActive(false);
        AudioBezBaterije.SetActive(false);
        AudioBezRobota.SetActive(false);
    }


    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (droppedItem == null)
            return;

        if (droppedItem.item.type == requiredType)
        {
            Debug.Log("Ispravan item spušten: " + droppedItem.item.type);
            // Osvježi stanje robotDropZone prije provjeravanja
            Transform baterijaTransform = robotDropZone.transform.Find("Baterija");
            if (baterijaTransform != null && baterijaTransform.gameObject.activeSelf)
            {
                robotDropZone.isActivated = true;
            }
            
            if (robotDropZone.isActivated == false)
            {
                Debug.Log("Robot nije aktiviran, ne može se nastaviti.");
                textBezBaterije.SetActive(true);
                AudioBezBaterije.SetActive(true);
                StartCoroutine(WaitOneSecond());
                return;
            }
            Destroy(droppedItem.gameObject);
            Objekt.SetActive(true);
            MainCamera.SetActive(false);
            Office.SetActive(false);
            EventSys.SetActive(false);
            Magnifier.SetActive(false);
            canvas.SetActive(false);

        }
        else
        {
            Debug.Log("Pogrešan item: " + droppedItem.item.type);
        }
    }

    public void OnClicked()
    {
        Debug.Log("Rupa kliknuta.");
        textBezRobota.SetActive(true);
        AudioBezRobota.SetActive(true);
        StartCoroutine(WaitOneSecond());
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is create
}