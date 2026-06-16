using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
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
        Time.timeScale = 1;
        if (TransitionManager.instance == null)
        {
            SceneManager.LoadScene("MainBuild");
        }
        else
        {
            TransitionManager.LoadScene("MainBuild", 0.5f, 1f);
        }
    }

    public void Menu()
    {
        Time.timeScale = 1;
        if (TransitionManager.instance == null)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            TransitionManager.LoadScene("Menu", 0.5f, 1f);
        }
    }

}
