using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static bool IsGameOver = false;

    [Header("Timer")]
    public float timeRemaining = 120f;
    public TextMeshProUGUI timerText;
    private bool gameOver = false;

    [Header("UI")]
    public GameObject gameOverPanel;

    void Start()
    {
        IsGameOver = false;
        gameOver = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (gameOver) return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerDisplay();

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            UpdateTimerDisplay();
            RadiationDeath();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void RadiationDeath()
    {
        gameOver = true;
        IsGameOver = true;

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        IsGameOver = false;

        SceneManager.LoadScene("DemoScene");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        IsGameOver = false;

        SceneManager.LoadScene("MainMenu");
    }
}