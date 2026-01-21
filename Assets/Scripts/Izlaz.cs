using UnityEngine;
using UnityEngine.EventSystems;

public class Izlaz : MonoBehaviour,  IPointerClickHandler
{
    [Header("States")]

    [Header("References")]
    public GameObject openDoor;
    public GameObject locker;
    public LockDropZone lockDropZone;

    

    // OVO SE POZIVA kada korisnik klikne UI element
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(lockDropZone.isOpened);

        if (lockDropZone.isOpened == true)
        {
            openDoor.SetActive(true);
        }
        else
        {
            locker.SetActive(true);
        }
    } 
}
