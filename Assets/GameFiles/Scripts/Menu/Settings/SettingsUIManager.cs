using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsUIManager : MonoBehaviour
{
    public static Action<bool> settingsClosed;

    [SerializeField] private GameObject mainSettingsUI;
    [SerializeField] private GameObject gameSettingsUI;
    [SerializeField] private GameObject audioSettingsUI;
    [SerializeField] private GameObject videoSettingsUI;
    [SerializeField] private GameObject KeysBindUI;
    [SerializeField] private GameObject background;

    [SerializeField] private GameObject previousMenuSelection;
    [SerializeField] private GameObject mainSettingsFirstSelected;
    [SerializeField] private GameObject gameSettingsFirstSelected;
    [SerializeField] private GameObject audioSettingsFirstSelected;
    [SerializeField] private GameObject videoSettingsFirstSelected;
    [SerializeField] private GameObject keysBindFirstSelected;

    private GameObject currentSettingsScreen;

    private void Start()
    {
        ClearSettingsScreen();
    }

    public void SetMenuFirstSelected(GameObject previousSelection) //must be an object that is selectable
    {
        previousMenuSelection = previousSelection;
    }

    public void MainSettings()
    { 
        ClearSettingsScreen();
        background.SetActive(true);
        mainSettingsUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(mainSettingsFirstSelected);
        currentSettingsScreen = mainSettingsUI;
    }

    public void GameSettings()
    {
        ClearSettingsScreen();
        gameSettingsUI.SetActive(true);
        background.SetActive(true);
        EventSystem.current.SetSelectedGameObject(gameSettingsFirstSelected);
        currentSettingsScreen = gameSettingsUI;
    }

    public void AudioSettings()
    {
        ClearSettingsScreen();
        audioSettingsUI.SetActive(true);
        background.SetActive(true);
        EventSystem.current.SetSelectedGameObject(audioSettingsFirstSelected);
        currentSettingsScreen = audioSettingsUI;
    }

    public void VideoSettings()
    {
        ClearSettingsScreen();
        videoSettingsUI.SetActive(true);
        background.SetActive(true);
        EventSystem.current.SetSelectedGameObject(videoSettingsFirstSelected);
        currentSettingsScreen = videoSettingsUI;
    }

    public void KeyBinds()
    { 
        ClearSettingsScreen();
        KeysBindUI.SetActive(true);
        background.SetActive(true);
        EventSystem.current.SetSelectedGameObject(keysBindFirstSelected);
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
        EventSystem.current.SetSelectedGameObject(previousMenuSelection);

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
