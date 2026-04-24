using System;
using UnityEngine;

public class SettingsUIManager : MonoBehaviour
{
    public static Action<bool> settingsClosed;

    [SerializeField] private GameObject mainSettingsUI;
    [SerializeField] private GameObject gameSettingsUI;
    [SerializeField] private GameObject audioSettingsUI;
    [SerializeField] private GameObject videoSettingsUI;
    [SerializeField] private GameObject background;

    private GameObject currentSettingsScreen;

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
        currentSettingsScreen = gameSettingsUI;
    }

    public void AudioSettings()
    {
        ClearSettingsScreen();
        audioSettingsUI.SetActive(true);
        currentSettingsScreen = audioSettingsUI;
    }

    public void VideoSettings()
    {
        ClearSettingsScreen();
        videoSettingsUI.SetActive(true);
        currentSettingsScreen = videoSettingsUI;
    }

    public void BackButton()
    { 
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
    }
}
