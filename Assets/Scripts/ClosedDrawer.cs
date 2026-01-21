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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isOpen)
        {
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
