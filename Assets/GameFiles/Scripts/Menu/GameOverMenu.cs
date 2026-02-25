using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject GameOverMenuUI;

    private void OnEnable()
    {
        PlayerStateController.GameOver += GameOver;
    }

    private void OnDisable()
    {
        PlayerStateController.GameOver -= GameOver;
    }

    private void Start()
    {
        GameOverMenuUI.SetActive(false);
    }

    private void GameOver()
    {
        GameOverMenuUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("DO_NOT_MODIFY-MATT-TEST");
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

}
