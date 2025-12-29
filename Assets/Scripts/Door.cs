using UnityEngine;
using UnityEngine.EventSystems;

public class Door : MonoBehaviour,  IPointerClickHandler
{
    [Header("States")]
    public int isOpen = 0;

    [Header("References")]
    public GameObject nextScene;
    public GameObject thisScene;
    public GameObject locker;

    

    // OVO SE POZIVA kada korisnik klikne UI element
    public void OnPointerClick(PointerEventData eventData)
    {

        if (isOpen == 3)
        {
            nextScene.SetActive(true);
            thisScene.SetActive(false);
        }
        else
        {
            locker.SetActive(true);
        }
    } 
}
