using UnityEngine;

public class RobotVignette : MonoBehaviour
{
    [Header("Vignette Settings")]
    [SerializeField] private float visibleRadius = 1f;
    [SerializeField] private float fadeDistance = 1f;
    [SerializeField] private Color darknessColor = Color.black;
    [SerializeField][Range(0f, 1f)] private float maxDarkness = 0.95f;

    [Header("References")]
    [SerializeField] private Transform robot;

    private Material vignetteMaterial;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // Create the shader material
        Shader shader = Shader.Find("Hidden/RobotVignette");
        if (shader == null)
        {
            Debug.LogError("Vignette shader not found! Make sure to create the shader.");
            enabled = false;
            return;
        }

        vignetteMaterial = new Material(shader);

        // Auto-find robot if not assigned
        if (robot == null)
        {
            GameObject robotObj = GameObject.Find("robot_0");
            if (robotObj != null)
            {
                robot = robotObj.transform;
            }
            else
            {
                Debug.LogError("Robot not found! Please assign the robot transform in the inspector.");
            }
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (vignetteMaterial == null || robot == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        // Convert robot world position to viewport position (0-1 range)
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(robot.position);

        // Pass parameters to shader
        vignetteMaterial.SetVector("_RobotPosition", new Vector4(viewportPos.x, viewportPos.y, 0, 0));
        vignetteMaterial.SetFloat("_VisibleRadius", visibleRadius);
        vignetteMaterial.SetFloat("_FadeDistance", fadeDistance);
        vignetteMaterial.SetColor("_DarknessColor", darknessColor);
        vignetteMaterial.SetFloat("_MaxDarkness", maxDarkness);
        vignetteMaterial.SetFloat("_AspectRatio", (float)Screen.width / Screen.height);

        Graphics.Blit(source, destination, vignetteMaterial);
    }

    void OnDestroy()
    {
        if (vignetteMaterial != null)
        {
            Destroy(vignetteMaterial);
        }
    }
}