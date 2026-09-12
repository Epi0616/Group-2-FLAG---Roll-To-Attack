using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class IntroSceneMenuUI : MonoBehaviour
{
    public static event Action<float> settingsOpened, menuClosed, menuOpened;
    public static event Action<SceneType> arenaTypeSelected;
    public static event Action newGame, loadGame;

    [SerializeField] private InputActionReference pauseGame;

    private bool menuActive = false;
    private bool transitionStarted = false;

    private void OnEnable()
    {
        transitionStarted = false;
        DiceProp.TransitionStart += () => transitionStarted = true;
        DiceProp.TransitionOver += () => transitionStarted = false;
        pauseGame.action.performed += HandlePauseGame;
    }

    private void OnDisable()
    {
        pauseGame.action.performed -= HandlePauseGame;
    }

    private void HandlePauseGame(InputAction.CallbackContext context)
    {
        TogglePaused();
    }

    public void TogglePaused()
    {
        if (transitionStarted) return;

        if (menuActive)
        {
            MoveToRoomOverview();
        }
        else 
        {
            MoveToMenu();
        }
    }

    public void StartGame(float transitionLength = 0.5f)
    {
        arenaTypeSelected?.Invoke(SceneType.SandArena);
        newGame?.Invoke();
        MoveToRoomOverview(transitionLength);
    }

    public void ContinueGame(float transitionLength = 0.5f)
    {
        arenaTypeSelected?.Invoke(SceneType.SandArena);
        loadGame?.Invoke();
        MoveToRoomOverview(transitionLength);
    }

    public void StartTutorial(float transitionLength = 0.5f)
    {
        arenaTypeSelected?.Invoke(SceneType.TutorialArena);
        MoveToRoomOverview(transitionLength);
    }

    private void MoveToRoomOverview(float transitionLength = 0.5f)
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

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
