using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject endScreenPanel;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Text titleText;
    [SerializeField] private Text subtitleText;
    [SerializeField] private Text timeText;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button quitButton;

    [Header("Text Settings")]
    [SerializeField] private string mainTitle = "POBJEDA!";
    [SerializeField] private string subtitle = "Uspješno si pobjegao iz sobe!";
    [SerializeField] private bool showPlayTime = true;

    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "Main Menu";
    [SerializeField] private string gameSceneName = "SampleScene";

    private float gameStartTime;

    void Start()
    {
        // Hide end screen at start
        if (endScreenPanel != null)
        {
            endScreenPanel.SetActive(false);
        }

        // Record game start time
        gameStartTime = Time.time;

        // Setup button listeners
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }

        if (playAgainButton != null)
        {
            playAgainButton.onClick.AddListener(PlayAgain);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    public void ShowEndScreen()
    {
        if (endScreenPanel != null)
        {
            endScreenPanel.SetActive(true);

            // Set title text
            if (titleText != null)
            {
                titleText.text = mainTitle;
            }

            // Set subtitle text
            if (subtitleText != null)
            {
                subtitleText.text = subtitle;
            }

            // Calculate and display play time
            if (timeText != null && showPlayTime)
            {
                float playTime = Time.time - gameStartTime;
                int minutes = Mathf.FloorToInt(playTime / 60);
                int seconds = Mathf.FloorToInt(playTime % 60);
                timeText.text = $"Vrijeme: {minutes:00}:{seconds:00}";
            }
            else if (timeText != null)
            {
                timeText.text = "";
            }

            // Pause the game
            Time.timeScale = 0f;
        }
    }

    void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    void PlayAgain()
    {
        Time.timeScale = 1f;
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting...");
    }
}