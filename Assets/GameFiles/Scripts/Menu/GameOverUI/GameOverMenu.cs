using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public static event Action PackUpScene;
    public static event Action ReturnToIntro;

    [SerializeField] private GameObject gameOverMenuUI;
    [SerializeField] private GameObject playAgainButton;
    [SerializeField] private GameOverStatsDisplay gameOverStatsDisplay;
    [SerializeField] private PlayerAbilitiesDisplay playerAbilitiesDisplay;

    private void OnEnable()
    {
        PlayerHealthSystem.GameOver += GameOver;
    }

    private void OnDisable()
    {
        PlayerHealthSystem.GameOver -= GameOver;
    }

    private void Start()
    {
        gameOverMenuUI.SetActive(false);
    }

    private void GameOver()
    {
        gameOverMenuUI.SetActive(true);
        gameOverStatsDisplay.UpdateStatsDisplay("null");
        playerAbilitiesDisplay.DisplayLoadout();
        EventSystem.current.firstSelectedGameObject = playAgainButton;
        UISelectionManager.instance.TrySetSelectedGameObject(playAgainButton);
        Time.timeScale = 0;
    }

    public void PlayAgain()
    {

        if (TransitionManager.instance == null)
        {
            PackUpScene?.Invoke();
            Time.timeScale = 1;
            ReturnToIntro?.Invoke();
        }
        else
        {
            PackUpScene?.Invoke();
            Time.timeScale = 1;
            ReturnToIntro?.Invoke();
        }
    }

    public void Menu()
    {
        Time.timeScale = 1;
        if (TransitionManager.instance == null)
        {
            PackUpScene?.Invoke();
            Time.timeScale = 1;
            ReturnToIntro?.Invoke();
        }
        else
        {
            PackUpScene?.Invoke();
            Time.timeScale = 1;
            ReturnToIntro?.Invoke();
        }
    }

}
