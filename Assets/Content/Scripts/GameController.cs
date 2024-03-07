using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("UI Menus")]
    [SerializeField] private GameObject homeMenuUI;
    [SerializeField] private GameObject gameplayMenuUI;
    [SerializeField] private GameObject pausedMenuUI;
    [SerializeField] private GameObject gameEndMenuUI;

    [Header("UI Text Elements")]
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI oldTimeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Game Objects")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject spawnerObject;

    [SerializeField] private GameObject mainObjects;
    [SerializeField] private GameObject gameObjects;

    private float gameTime = 0f;
    private int score = 0;
    private bool isGameRunning = false;

    private const string OldTimeKey = "OldTime";
    private const string TotalScoreKey = "TotalScore";

    private void Start()
    {
        InitializeGame();
        LoadTotalScore(); // Load total score from PlayerPrefs
        LoadOldTime(); // Load old time from PlayerPrefs
    }

    // Method to set UI element state
    private void SetUIState(GameObject uiElement, bool state)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(state);
        }
    }

    // Method to set game object state
    private void SetObjectState(GameObject gameObject, bool state)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(state);
        }
    }

    // Method to initialize the game
    private void InitializeGame()
    {
        SetUIState(homeMenuUI, true);
        SetUIState(gameplayMenuUI, false);
        SetUIState(pausedMenuUI, false);
        SetUIState(gameEndMenuUI, false);

        SetObjectState(mainObjects, true);
        SetObjectState(gameObjects, false);

        SetObjectState(playerObject, false);
        SetObjectState(spawnerObject, false);
    }

    // Method to start the game
    public void PlayBtn()
    {
        SetUIState(homeMenuUI, false);
        SetUIState(gameplayMenuUI, true);
        SetObjectState(mainObjects, false);
        SetObjectState(gameObjects, true);
        SetObjectState(playerObject, true);
        SetObjectState(spawnerObject, true);

        StartGameTimer();
        DestroyExistingBarriers();
    }

    // Method to start the game timer
    private void StartGameTimer()
    {
        isGameRunning = true;
        gameTime = 0f;
        UpdateTimeText(); // Reset time text
        score = 0; // Reset score
        UpdateScoreText(); // Reset score text
    }

    // Method to update the time text
    private void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Method to update the score text
    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    // Method to update game logic each frame
    private void Update()
    {
        if (isGameRunning)
        {
            gameTime += Time.deltaTime;
            UpdateTimeText();
        }
    }

    // Method to destroy existing barrier game objects
    private void DestroyExistingBarriers()
    {
        // Find all barrier game objects in the scene
        Barrier[] barriers = FindObjectsOfType<Barrier>();

        // Iterate through each barrier game object found
        foreach (Barrier barrier in barriers)
        {
            // Destroy the barrier game object
            Destroy(barrier.gameObject);
        }
    }

    // Method to handle game end event
    public void EndGameBtn()
    {
        SetUIState(gameEndMenuUI, true);
        Time.timeScale = 0f;

        SaveBestTime();
        SaveBestScore();
    }

    // Method to restart the scene
    public void RestartScene()
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Method to pause the game
    public void PauseGame()
    {
        Time.timeScale = 0f;
        SetUIState(pausedMenuUI, true);
        isGameRunning = false; // Pause the timer
    }

    // Method to resume the game
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        SetUIState(pausedMenuUI, false);
        isGameRunning = true; // Resume the timer
    }

    // Method to exit the game
    public void ExitGameBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // Method to load the best score from PlayerPrefs
    private void LoadTotalScore()
    {
        int bestScore = PlayerPrefs.GetInt(TotalScoreKey, 0);
        totalScoreText.text = bestScore.ToString();
    }

    // Method to save the total score to PlayerPrefs
    private void SaveBestScore()
    {
        int prevBestScore = PlayerPrefs.GetInt(TotalScoreKey, 0);

        int totalScore = prevBestScore + score; // Add current score to previous score

        PlayerPrefs.SetInt(TotalScoreKey, totalScore);
        PlayerPrefs.Save();
        totalScoreText.text = totalScore.ToString();
    }

    // Method to save the old time to PlayerPrefs
    private void SaveBestTime()
    {
        PlayerPrefs.SetFloat(OldTimeKey, gameTime);
        PlayerPrefs.Save();

        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        oldTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Method to load the old time from PlayerPrefs
    private void LoadOldTime()
    {
        float bestTime = PlayerPrefs.GetFloat(OldTimeKey, Mathf.Infinity);

        if (bestTime != Mathf.Infinity)
        {
            int minutes = Mathf.FloorToInt(bestTime / 60);
            int seconds = Mathf.FloorToInt(bestTime % 60);
            oldTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            oldTimeText.text = "00:00"; // Default value if no old time is saved
        }
    }

    // Method to increase the score
    public void IncreaseScore(int value)
    {
        score += value;
        UpdateScoreText();
    }

    // Method to open a link
    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }
}
