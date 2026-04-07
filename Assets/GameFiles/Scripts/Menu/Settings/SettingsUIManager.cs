using System;
using UnityEngine;

public class SettingsUIManager : MonoBehaviour
{
    public static Action<bool> settingsClosed;

    [SerializeField] private GameObject mainSettingsUI;
    [SerializeField] private GameObject gameSettingsUI;
    [SerializeField] private GameObject audioSettingsUI;
    [SerializeField] private GameObject videoSettingsUI;

    private GameObject currentSettingsScreen;

    public void MainSettings()
    { 
        ClearSettingsScreen();
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
        ClearSettingsScreen();
    }

    public void ClearSettingsScreen()
    { 
        mainSettingsUI.SetActive(false);
        gameSettingsUI.SetActive(false);
        audioSettingsUI.SetActive(false);
        videoSettingsUI.SetActive(false);
    }
}
