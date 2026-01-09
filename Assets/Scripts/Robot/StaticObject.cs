using UnityEngine;

public class StaticObject : MonoBehaviour
{
    [Header("Collider Settings")]
    [SerializeField] private bool autoAddCollider = true;

    void Start()
    {
        // Automatically add a collider if one doesn't exist
        if (autoAddCollider && GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            Debug.Log($"Auto-added BoxCollider2D to {gameObject.name}");
        }
    }
}