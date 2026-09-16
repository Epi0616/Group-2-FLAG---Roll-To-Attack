using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class PauseMenu : MonoBehaviour
{
    public static event Action PackUpScene;
    public static event Action ReturnToIntro;
    public static event Action GamePaused;
    public static event Action GameUnPaused;

    [SerializeField] private MusicPackage menuMusic;
    [SerializeField] private InputActionReference pauseGame;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private SettingsUIManager settingsManager;
    [SerializeField] private GameObject[] pauseMenuButtons;
    [SerializeField] private GameObject previousUiSelection;
    public static bool isGamePaused = false;
    private bool isGameOver = false;
    private bool transitioning = false;

    private void OnEnable()
    {
        transitioning = true;
        SceneTransitionManager.TransitionComplete += HandleTransitionComplete;
        PlayerHealthSystem.GameOver += GameOver;
        SettingsUIManager.settingsClosed += SetPauseButtonsVisibility;
        pauseGame.action.performed += HandlePauseGame;
    }

    private void OnDisable()
    {
        SceneTransitionManager.TransitionComplete -= HandleTransitionComplete;
        PlayerHealthSystem.GameOver -= GameOver;
        SettingsUIManager.settingsClosed -= SetPauseButtonsVisibility;
        pauseGame.action.performed -= HandlePauseGame;
    }
    private void HandlePauseGame(InputAction.CallbackContext context)
    {
        if (SceneTransitionManager.instance != null && transitioning) return;

        if (EventSystem.current.currentSelectedGameObject != null)
        {
            previousUiSelection = EventSystem.current.currentSelectedGameObject;
        }

        TogglePaused();
    }

    private void HandleTransitionComplete()
    {
        Debug.Log("transition complete");
        transitioning = false;
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

        PackUpScene?.Invoke();
        ReturnToIntro?.Invoke();
        transitioning = true;
    }

    public void TogglePaused()
    {
        if (isGameOver) { return; }

        if (!isGamePaused)
        {
            pauseMenuUI.SetActive(true);
            //MusicPlayer.instance.DampenMusic();
            MusicPlayer.instance.PlayMusicWithFade(menuMusic, 2);
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
            //MusicPlayer.instance.UndampenMusic();

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
