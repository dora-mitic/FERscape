using UnityEngine;
using UnityEngine.EventSystems;

public class ClosedDrawer : MonoBehaviour, IPointerClickHandler
{
    [Header("States")]
    public bool isOpen = false;
    public bool firstOpen = true;

    [Header("References")]
    public GameObject openDrawer;
    public GameObject puzzleZoomed;
    public GameObject locker;

    // OVO SE POZIVA kada korisnik klikne UI element
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Klik! isOpen = " + isOpen);

        if (isOpen)
        {
            Debug.Log("unutra");
            if (firstOpen)
            {
                firstOpen = false;
                openDrawer.SetActive(true);
                this.gameObject.SetActive(false);
            }

            else
            {
                puzzleZoomed.SetActive(true);
            }
        }
        else
        {
            locker.SetActive(true);
        }
    }
}
