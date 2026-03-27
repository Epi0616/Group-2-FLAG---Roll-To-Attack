using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseGame;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private SettingsUIManager settingsManager;
    [SerializeField] private GameObject[] pauseMenuButtons;
    public static bool isGamePaused = false;
    private bool isGameOver = false;

    private void OnEnable()
    {
        HealthSystem.GameOver += GameOver;
        SettingsUIManager.settingsClosed += setPauseButtonsVisibility;
    }

    private void OnDisable()
    {
        HealthSystem.GameOver -= GameOver;
        SettingsUIManager.settingsClosed -= setPauseButtonsVisibility;
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
        setPauseButtonsVisibility(false);
        settingsManager.MainSettings();
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
            settingsManager.ClearSettingsScreen();
            setPauseButtonsVisibility(true);
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1;
        }

        isGamePaused = !isGamePaused;
    }

    public void setPauseButtonsVisibility(bool visible)
    {
        for (int i = 0; i < pauseMenuButtons.Length; i++)
        {
            pauseMenuButtons[i].SetActive(visible);
        }
    }
}
