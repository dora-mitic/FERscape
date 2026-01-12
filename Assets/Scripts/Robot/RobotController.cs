using UnityEngine;

public class RobotController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Sprite Settings")]
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteRight;
    [SerializeField] private Sprite spriteUpLeft;
    [SerializeField] private Sprite spriteUpRight;
    [SerializeField] private Sprite spriteDownLeft;
    [SerializeField] private Sprite spriteDownRight;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // Configure Rigidbody2D for proper collision
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f; // No gravity for top-down view
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation
        }

        // Make sure robot has a collider
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
            Debug.Log("Auto-added BoxCollider2D to robot");
        }
    }

    void Update()
    {
        // Get input from WASD or Arrow keys
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY);

        // Update sprite based on movement direction
        if (movement != Vector2.zero)
        {
            UpdateSprite(movement);
        }
    }

    void FixedUpdate()
    {
        // Normalize diagonal movement so it's not faster
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        // Move the robot using Rigidbody2D for proper physics collision
        if (rb != null)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            transform.position += (Vector3)(movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void UpdateSprite(Vector2 direction)
    {
        if (spriteRenderer == null) return;

        // Determine which sprite to use based on direction
        if (direction.x > 0 && direction.y > 0)
        {
            // Up-Right
            if (spriteUpRight != null) spriteRenderer.sprite = spriteUpRight;
        }
        else if (direction.x < 0 && direction.y > 0)
        {
            // Up-Left
            if (spriteUpLeft != null) spriteRenderer.sprite = spriteUpLeft;
        }
        else if (direction.x > 0 && direction.y < 0)
        {
            // Down-Right
            if (spriteDownRight != null) spriteRenderer.sprite = spriteDownRight;
        }
        else if (direction.x < 0 && direction.y < 0)
        {
            // Down-Left
            if (spriteDownLeft != null) spriteRenderer.sprite = spriteDownLeft;
        }
        else if (direction.y > 0)
        {
            // Up
            if (spriteUp != null) spriteRenderer.sprite = spriteUp;
        }
        else if (direction.y < 0)
        {
            // Down
            if (spriteDown != null) spriteRenderer.sprite = spriteDown;
        }
        else if (direction.x > 0)
        {
            // Right
            if (spriteRight != null) spriteRenderer.sprite = spriteRight;
        }
        else if (direction.x < 0)
        {
            // Left
            if (spriteLeft != null) spriteRenderer.sprite = spriteLeft;
        }
    }
}