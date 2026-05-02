using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class PauseMenu : MonoBehaviour
{
    public static event Action GamePaused;
    public static event Action GameUnPaused;

    [SerializeField] private InputActionReference pauseGame, backButton;
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
        pauseGame.action.performed += HandlePauseGame;
        backButton.action.performed += HandleBackButton;
    }

    private void OnDisable()
    {
        HealthSystem.GameOver -= GameOver;
        SettingsUIManager.settingsClosed -= SetPauseButtonsVisibility;
        pauseGame.action.performed -= HandlePauseGame;
        backButton.action.performed -= HandleBackButton;
    }

    private void HandlePauseGame(InputAction.CallbackContext context)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            previousUiSelection = EventSystem.current.currentSelectedGameObject;
        }
        TogglePaused();
    }

    private void HandleBackButton(InputAction.CallbackContext context)
    {
        //if (!isGamePaused) return;
        //TogglePaused();
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
            EventSystem.current.firstSelectedGameObject = pauseMenuButtons[0];
            UISelectionManager.instance.TrySetSelectedGameObject(pauseMenuButtons[0]);
            //EventSystem.current.SetSelectedGameObject(pauseMenuButtons[0]);
            Time.timeScale = 0;
            GamePaused?.Invoke();
        }
        else
        {
            settingsManager.ClearSettingsScreen();
            SetPauseButtonsVisibility(true);
            pauseMenuUI.SetActive(false);

            if (previousUiSelection != null)
            {
                EventSystem.current.firstSelectedGameObject = previousUiSelection;
                UISelectionManager.instance.TrySetSelectedGameObject(previousUiSelection);
                //EventSystem.current.SetSelectedGameObject(previousUiSelection);
                previousUiSelection = null;
            }
            
            Time.timeScale = 1;
            GameUnPaused?.Invoke();
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
