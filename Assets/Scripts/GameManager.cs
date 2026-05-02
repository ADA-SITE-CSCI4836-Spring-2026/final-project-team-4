using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool IsGameOver = false;

    [Header("Timer")]
    public float timeRemaining = 120f;
    public TextMeshProUGUI timerText;
    private bool gameOver = false;

    [Header("Item Tracking")]
    private bool hasGear = false;
    private bool hasFuel = false;
    private bool hasScrewdriver = false;

    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject winPanel;

    [Header("Item UI Texts")]
    public TextMeshProUGUI gearText;
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI screwdriverText;

    [Header("Helicopter Message")]
    public TextMeshProUGUI helicopterMessageText;
    private float messageTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        IsGameOver = false;
        gameOver = false;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (helicopterMessageText != null) helicopterMessageText.gameObject.SetActive(false);

        UpdateItemUI();
    }

    void Update()
    {
        if (gameOver) return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerDisplay();

        // Flash timer red when low
        if (timeRemaining <= 30f)
            timerText.color = Color.red;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            UpdateTimerDisplay();
            RadiationDeath();
        }

        // Hide helicopter message after 3 seconds
        if (messageTimer > 0)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0 && helicopterMessageText != null)
                helicopterMessageText.gameObject.SetActive(false);
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void CollectItem(ItemType type)
    {
        if (type == ItemType.Gear) hasGear = true;
        else if (type == ItemType.Fuel) hasFuel = true;
        else if (type == ItemType.Screwdriver) hasScrewdriver = true;

        UpdateItemUI();
    }

    public bool HasAllItems()
    {
        return hasGear && hasFuel && hasScrewdriver;
    }

    void UpdateItemUI()
    {
        if (gearText != null)
            gearText.text = "Gear: " + (hasGear ? "1/1" : "0/1");
        if (fuelText != null)
            fuelText.text = "Fuel: " + (hasFuel ? "1/1" : "0/1");
        if (screwdriverText != null)
            screwdriverText.text = "Screwdriver: " + (hasScrewdriver ? "1/1" : "0/1");
    }

    public void ShowHelicopterMessage()
    {
        if (helicopterMessageText != null)
        {
            helicopterMessageText.text = 
                "Collect all parts to fix the helicopter and escape!";
            helicopterMessageText.gameObject.SetActive(true);
            messageTimer = 3f;
        }
    }

    public void WinGame()
    {
        gameOver = true;
        IsGameOver = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (winPanel != null) winPanel.SetActive(true);
    }

    void RadiationDeath()
    {
        gameOver = true;
        IsGameOver = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
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