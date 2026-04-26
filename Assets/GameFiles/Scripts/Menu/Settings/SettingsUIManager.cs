using System;
using UnityEngine;

public class SettingsUIManager : MonoBehaviour
{
    public static Action<bool> settingsClosed;

    [SerializeField] private GameObject mainSettingsUI;
    [SerializeField] private GameObject gameSettingsUI;
    [SerializeField] private GameObject audioSettingsUI;
    [SerializeField] private GameObject videoSettingsUI;
    [SerializeField] private GameObject KeysBindUI;
    [SerializeField] private GameObject background;

    private GameObject currentSettingsScreen;

    private void Start()
    {
        ClearSettingsScreen();
    }

    public void MainSettings()
    { 
        ClearSettingsScreen();
        background.SetActive(true);
        mainSettingsUI.SetActive(true);
        currentSettingsScreen = mainSettingsUI;
    }

    public void GameSettings()
    {
        ClearSettingsScreen();
        gameSettingsUI.SetActive(true);
        background.SetActive(true);
        currentSettingsScreen = gameSettingsUI;
    }

    public void AudioSettings()
    {
        ClearSettingsScreen();
        audioSettingsUI.SetActive(true);
        background.SetActive(true);
        currentSettingsScreen = audioSettingsUI;
    }

    public void VideoSettings()
    {
        ClearSettingsScreen();
        videoSettingsUI.SetActive(true);
        background.SetActive(true);
        currentSettingsScreen = videoSettingsUI;
    }

    public void KeyBinds()
    { 
        ClearSettingsScreen();
        KeysBindUI.SetActive(true);
        background.SetActive(true);
        currentSettingsScreen = KeysBindUI;

    }

    public void BackButton()
    {
        if (currentSettingsScreen == KeysBindUI)
        {
            GameSettings();
            return;
        }

        if (currentSettingsScreen != mainSettingsUI)
        { 
            MainSettings();
            return;
        }

        settingsClosed?.Invoke(true);
        background.SetActive(false);
        ClearSettingsScreen();
    }

    public void ClearSettingsScreen()
    {
        background.SetActive(false);
        mainSettingsUI.SetActive(false);
        gameSettingsUI.SetActive(false);
        audioSettingsUI.SetActive(false);
        videoSettingsUI.SetActive(false);
        KeysBindUI.SetActive(false);
    }
}
