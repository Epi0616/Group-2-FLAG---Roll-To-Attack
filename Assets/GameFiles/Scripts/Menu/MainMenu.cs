using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseGame;
    [SerializeField] private SettingsUIManager SettingsUIManager;
    [SerializeField] private GameObject greyBackground;

    public void PlayGame()
    {
        if (TransitionManager.instance == null)
        {
            SceneManager.LoadScene("MainBuild");
        }
        else
        {
            TransitionManager.LoadScene("MainBuild", 0.5f, 1f);
        }

    }

    private void OnEnable()
    {
        SettingsUIManager.settingsClosed += HideGreyBackground;
    }
    private void OnDisable()
    {
        SettingsUIManager.settingsClosed -= HideGreyBackground;
    }

    private void Update()
    {
        if (pauseGame.action.WasPressedThisFrame())
        {
            if (greyBackground.activeSelf)
            {
                ToggleOptions();
            }
        }
    }

    public void Options()
    { 
        ToggleOptions();
    }

    private void ToggleOptions()
    {
        if (greyBackground.activeSelf)
        { 
            SettingsUIManager.ClearSettingsScreen();
            greyBackground.SetActive(false);
            return;
        }

        SettingsUIManager.MainSettings();
        greyBackground.SetActive(true);
    }

    private void HideGreyBackground(bool var)
    {
        greyBackground.SetActive(false);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
