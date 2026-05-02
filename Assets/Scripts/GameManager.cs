using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Timer")]
    public float timeRemaining = 120f; // 2 minutes, change as needed
    public TextMeshProUGUI timerText;
    private bool gameOver = false;

    [Header("UI")]
    public GameObject gameOverPanel;

    void Update()
    {
        if (gameOver) return;

        // Count down
        timeRemaining -= Time.deltaTime;

        // Update UI
        UpdateTimerDisplay();

        // Death by radiation
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
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
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}