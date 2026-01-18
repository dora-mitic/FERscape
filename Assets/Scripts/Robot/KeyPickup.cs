using UnityEngine;
using UnityEngine.UI;

public class KeyPickup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject keyFoundPanel;
    [SerializeField] private Image keyImage;
    [SerializeField] private Text keyFoundText;
    [SerializeField] private RawImage stripeBackground;

    [Header("Settings")]
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private float stripeRotationSpeed = 100f;
    [SerializeField][Range(0f, 1f)] private float stripeAlpha = 0.8f;

    private bool keyCollected = false;
    private Material stripeMaterial;

    void Start()
    {
        // Make sure the panel is hidden at start
        if (keyFoundPanel != null)
        {
            keyFoundPanel.SetActive(false);
        }

        // Add a collider to the key if it doesn't have one
        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            Debug.Log("Auto-added trigger collider to key");
        }
        else
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        // Create the stripe material
        if (stripeBackground != null)
        {
            Shader shader = Shader.Find("Hidden/RadialStripes");
            if (shader != null)
            {
                stripeMaterial = new Material(shader);
                stripeBackground.material = stripeMaterial;
            }
        }
    }

    void Update()
    {
        // Check for mouse click on the key
        if (Input.GetMouseButtonDown(0) && !keyCollected)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);

            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                CollectKey();
            }
        }

        // Rotate stripes if panel is active
        if (keyFoundPanel != null && keyFoundPanel.activeSelf && stripeMaterial != null)
        {
            float rotation = Time.time * stripeRotationSpeed;
            stripeMaterial.SetFloat("_Rotation", rotation);
            stripeMaterial.SetFloat("_Alpha", stripeAlpha);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the robot collided with the key
        if (other.CompareTag("Player") || other.name.Contains("robot"))
        {
            CollectKey();
        }
    }

    void CollectKey()
    {
        if (keyCollected) return;

        keyCollected = true;

        // Hide the key in the game world
        GetComponent<SpriteRenderer>().enabled = false;

        // Show the key found panel
        ShowKeyFoundScreen();

        // Hide the panel after duration
        Invoke("HideKeyFoundScreen", displayDuration);
    }

    void ShowKeyFoundScreen()
    {
        if (keyFoundPanel != null)
        {
            keyFoundPanel.SetActive(true);

            // Set the key sprite in the UI
            if (keyImage != null)
            {
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite != null)
                {
                    keyImage.sprite = sr.sprite;
                }
            }

            // Set the text
            if (keyFoundText != null)
            {
                keyFoundText.text = "KEY FOUND!";
            }
        }
    }

    void HideKeyFoundScreen()
    {
        if (keyFoundPanel != null)
        {
            keyFoundPanel.SetActive(false);
        }
    }

    void OnDestroy()
    {
        if (stripeMaterial != null)
        {
            Destroy(stripeMaterial);
        }
    }
}