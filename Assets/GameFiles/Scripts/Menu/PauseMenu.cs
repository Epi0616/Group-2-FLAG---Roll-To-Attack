using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseGame;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private SettingsUIManager settingsManager;
    [SerializeField] private GameObject[] pauseMenuButtons;
    [SerializeField] private GameObject previousUiSelection;
    public static bool isGamePaused = false;
    private bool isGameOver = false;

    private void OnEnable()
    {
        HealthSystem.GameOver += GameOver;
        SettingsUIManager.settingsClosed += SetPauseButtonsVisibility;
    }

    private void OnDisable()
    {
        HealthSystem.GameOver -= GameOver;
        SettingsUIManager.settingsClosed -= SetPauseButtonsVisibility;
    }

    private void Update()
    {
        if (pauseGame.action.WasPressedThisFrame())
        {
            previousUiSelection = EventSystem.current.currentSelectedGameObject;
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
        SetPauseButtonsVisibility(false);
        settingsManager.MainSettings();
    }

    public void Menu()
    {
        TogglePaused();
        if (TransitionManager.instance == null)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            TransitionManager.LoadScene("Menu", 0.5f, 1f);
        }

           
    }

    public void TogglePaused()
    {
        if (isGameOver) { return; }

        if (!isGamePaused)
        {
            pauseMenuUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(pauseMenuButtons[0]);
            Time.timeScale = 0;
        }
        else
        {
            settingsManager.ClearSettingsScreen();
            SetPauseButtonsVisibility(true);
            pauseMenuUI.SetActive(false);

            if (previousUiSelection != null)
            {
                EventSystem.current.SetSelectedGameObject(previousUiSelection);
                previousUiSelection = null;
            }
            
            Time.timeScale = 1;
        }

        isGamePaused = !isGamePaused;
    }

    public void SetPauseButtonsVisibility(bool visible)
    {
        for (int i = 0; i < pauseMenuButtons.Length; i++)
        {
            pauseMenuButtons[i].SetActive(visible);
        }
    }
}
