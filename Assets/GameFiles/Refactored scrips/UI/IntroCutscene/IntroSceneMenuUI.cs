using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class IntroSceneMenuUI : MonoBehaviour
{
    public static event Action<float> settingsOpened, menuClosed, menuOpened;

    [SerializeField] private InputActionReference pauseGame, backButton;

    private bool menuActive = false;

    private void OnEnable()
    {
        pauseGame.action.performed += HandlePauseGame;
        backButton.action.performed += HandleBackButton;
    }

    private void OnDisable()
    {
        pauseGame.action.performed -= HandlePauseGame;
        backButton.action.performed -= HandleBackButton;
    }

    private void HandlePauseGame(InputAction.CallbackContext context)
    {
        TogglePaused();
    }

    private void HandleBackButton(InputAction.CallbackContext context)
    {
        //if (!isGamePaused) return;
        //TogglePaused();
    }

    public void TogglePaused()
    {
        if (menuActive)
        {
            MoveToRoomOverview();
        }
        else 
        {
            MoveToSettings();
        }
    }

    public void MoveToRoomOverview(float transitionLength = 0.5f)
    {
        menuClosed?.Invoke(transitionLength);
        menuActive = false;
    }
    public void MoveToMenu(float transitionLength = 0.5f)
    {
        menuOpened?.Invoke(transitionLength);
        menuActive = true;
    }

    public void MoveToSettings(float transitionLength = 0.5f)
    {
        settingsOpened?.Invoke(transitionLength);
        menuActive = true;
    }
}
