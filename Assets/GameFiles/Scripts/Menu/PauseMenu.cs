using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseGame;
    [SerializeField] private GameObject pauseMenuUI;
    public static bool isGamePaused = false;
    private bool isGameOver = false;

    private void OnEnable()
    {
        HealthSystem.GameOver += GameOver;
    }

    private void OnDisable()
    {
        HealthSystem.GameOver -= GameOver;
    }

    private void Update()
    {
        if (pauseGame.action.WasPressedThisFrame())
        {
            TogglePaused();
        }
    }

    public void GameOver()
    { 
        isGameOver = true;
    }

    public void Resume()
    {
        TogglePaused();
    }

    public void Options()
    { 
    
    }

    public void Menu()
    {
        TogglePaused();
        SceneManager.LoadScene("Menu");
    }

    public void TogglePaused()
    {
        if (isGameOver) { return; }

        if (!isGamePaused)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1;
        }

        isGamePaused = !isGamePaused;
    }
}
