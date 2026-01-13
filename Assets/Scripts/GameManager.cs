using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Referencias de UI")]
    public GameObject pauseMenuUI;
    public GameObject victoryUI;
    public GameObject defeatUI;
    
    [Header("Estado del Juego")]
    public bool isPaused = false;
    public bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isGameOver) return;

        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pauseMenuUI) pauseMenuUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            if (pauseMenuUI) pauseMenuUI.SetActive(false);
        }
    }

    public void Victory()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;

        if (victoryUI) victoryUI.SetActive(true);
    }

    public void Defeat()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;

        if (defeatUI) defeatUI.SetActive(true);
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
