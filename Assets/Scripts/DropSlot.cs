using UnityEngine;

public class DropSlot : MonoBehaviour
{
    [HideInInspector] public DraggableItem currentItem;

    public PuzzleManager puzzleManager;   // povuci iz Inspectora

    public bool IsFree => currentItem == null;

    public void Place(DraggableItem item)
    {
        // ako već postoji item u slotu, makni ga sa slota i vrati ga na početnu poziciju
        if (currentItem != null)
        {
            currentItem.currentSlot = null;
            currentItem.ReturnToStart();
        }

        currentItem = item;
        item.currentSlot = this;
        item.transform.position = transform.position;

        if (puzzleManager != null)
            puzzleManager.CheckPuzzle();
    }

    public void Clear()
    {
        if (currentItem != null)
            currentItem.currentSlot = null;

        currentItem = null;

        if (puzzleManager != null)
            puzzleManager.CheckPuzzle();
    }
}
