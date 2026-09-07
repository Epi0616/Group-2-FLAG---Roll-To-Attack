using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsUIManager : MonoBehaviour
{
    public static Action<bool> settingsClosed;
    public static Action keyBindUIOpened, keyBindUIClosed;
    public static Action settingsCleared;
    [SerializeField] private InputActionReference backButtonAction;

    [SerializeField] private GameObject mainSettingsUI;
    [SerializeField] private GameObject gameSettingsUI;
    [SerializeField] private GameObject audioSettingsUI;
    [SerializeField] private GameObject videoSettingsUI;
    [SerializeField] private GameObject KeysBindUI;

    private GameObject currentSettingsScreen;
    private bool settingsOpen = false;

    private void Start()
    {
        ClearSettingsScreen();
    }

    public void MainSettings()
    { 
        settingsOpen = true;
        ClearSettingsScreen();
        mainSettingsUI.SetActive(true);
        //EventSystem.current.SetSelectedGameObject(mainSettingsFirstSelected);
        currentSettingsScreen = mainSettingsUI;
    }

    public void KeyBinds()
    { 
        ClearSettingsScreen();
        KeysBindUI.SetActive(true);
        //EventSystem.current.SetSelectedGameObject(keysBindFirstSelected);
        currentSettingsScreen = KeysBindUI;
        keyBindUIOpened?.Invoke();
    }

    public void BackButton()
    {
        if (!settingsOpen) return;

        if (currentSettingsScreen == KeysBindUI)
        {
            //keyBindUIClosed?.Invoke();
            return;
        }

        if (currentSettingsScreen != mainSettingsUI)
        { 
            MainSettings();
            return;
        }

        settingsClosed?.Invoke(true);
        settingsOpen = false;
        //EventSystem.current.SetSelectedGameObject(previousMenuSelection);

        ClearSettingsScreen();
    }

    public void ClearSettingsScreen()
    {
        settingsCleared?.Invoke();

        mainSettingsUI.SetActive(false);
        KeysBindUI.SetActive(false);
    }
}
