using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    [Header("Puzzle settings")]
    public bool isCorrectResistor;        // označi u Inspectoru koji su ispravni
    public PuzzleManager puzzleManager;   // povuci isti manager kao u DropSlotu

    [HideInInspector] public DropSlot currentSlot;

    Vector3 startPosition;
    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        startPosition = transform.position;

        if (puzzleManager == null)
            puzzleManager = FindObjectOfType<PuzzleManager>();
    }

    void OnMouseDown()
    {
        // ako je u slotu, privremeno ga isprazni da ga možemo premjestiti
        if (currentSlot != null)
        {
            currentSlot.Clear();
            currentSlot = null;
        }
    }

    void OnMouseDrag()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCam.transform.position.z;
        Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);
        transform.position = worldPos;
    }

    void OnMouseUp()
    {
        float radius = 0.5f;  // prilagodi ovisno o skali

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius); // [web:30]

        DropSlot bestSlot = null;
        float bestDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            DropSlot slot = hit.GetComponent<DropSlot>();
            if (slot == null) continue;

            float d = Vector2.Distance(transform.position, slot.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                bestSlot = slot;
            }
        }

        if (bestSlot != null)
        {
            bestSlot.Place(this);
        }
        else
        {
            ReturnToStart();
            if (puzzleManager != null)
                puzzleManager.CheckPuzzle();
        }
        if (puzzleManager != null)
            puzzleManager.CheckPuzzle();
    }

    public void ReturnToStart()
    {
        transform.position = startPosition;
    }
}
