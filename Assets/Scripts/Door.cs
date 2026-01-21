using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class Door : MonoBehaviour,  IPointerClickHandler
{
    [Header("States")]
    public int isOpen = 0;

    [Header("References")]
    public GameObject openDoor;
    public GameObject locker;

    public SFXManager sfxManager;

    // OVO SE POZIVA kada korisnik klikne UI element
    public void OnPointerClick(PointerEventData eventData)
    {

        if (isOpen == 3)
        {
            openDoor.SetActive(true);
            sfxManager.PlayDoorOpen();

        }
        else
        {
            locker.SetActive(true);
        }
    } 
}
