using System;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [SerializeField] private GameObject previousMenuSelection;
    [SerializeField] private GameObject mainSettingsFirstSelected;
    [SerializeField] private GameObject gameSettingsFirstSelected;
    [SerializeField] private GameObject audioSettingsFirstSelected;
    [SerializeField] private GameObject videoSettingsFirstSelected;

    private GameObject currentSettingsScreen;
    private bool settingsOpen = false;

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
        settingsOpen = true;
        ClearSettingsScreen();
        mainSettingsUI.SetActive(true);
        EventSystem.current.firstSelectedGameObject = mainSettingsFirstSelected;
        UISelectionManager.instance.TrySetSelectedGameObject(mainSettingsFirstSelected);
        //EventSystem.current.SetSelectedGameObject(mainSettingsFirstSelected);
        currentSettingsScreen = mainSettingsUI;
    }

    public void GameSettings()
    {
        ClearSettingsScreen();
        gameSettingsUI.SetActive(true);
        EventSystem.current.firstSelectedGameObject = gameSettingsFirstSelected;
        UISelectionManager.instance.TrySetSelectedGameObject(gameSettingsFirstSelected);
        //EventSystem.current.SetSelectedGameObject(gameSettingsFirstSelected);
        currentSettingsScreen = gameSettingsUI;
    }

    public void AudioSettings()
    {
        ClearSettingsScreen();
        audioSettingsUI.SetActive(true);
        EventSystem.current.firstSelectedGameObject = audioSettingsFirstSelected;
        UISelectionManager.instance.TrySetSelectedGameObject(audioSettingsFirstSelected);
        //EventSystem.current.SetSelectedGameObject(audioSettingsFirstSelected);
        currentSettingsScreen = audioSettingsUI;
    }

    public void VideoSettings()
    {
        ClearSettingsScreen();
        videoSettingsUI.SetActive(true);
        EventSystem.current.firstSelectedGameObject = videoSettingsFirstSelected;
        UISelectionManager.instance.TrySetSelectedGameObject(videoSettingsFirstSelected);
        //EventSystem.current.SetSelectedGameObject(videoSettingsFirstSelected);
        currentSettingsScreen = videoSettingsUI;
    }

    public void KeyBinds()
    { 
        ClearSettingsScreen();
        KeysBindUI.SetActive(true);
        //EventSystem.current.SetSelectedGameObject(keysBindFirstSelected);
        currentSettingsScreen = KeysBindUI;
        keyBindUIOpened?.Invoke();
    }

    private void HandleBackButton(InputAction.CallbackContext context)
    {
        BackButton();
    }

    public void BackButton()
    {
        if (!settingsOpen) return;

        if (currentSettingsScreen == KeysBindUI)
        {
            //keyBindUIClosed?.Invoke();
            GameSettings();
            return;
        }

        if (currentSettingsScreen != mainSettingsUI)
        { 
            MainSettings();
            return;
        }

        settingsClosed?.Invoke(true);
        settingsOpen = false;
        EventSystem.current.firstSelectedGameObject = previousMenuSelection;
        UISelectionManager.instance.TrySetSelectedGameObject(previousMenuSelection);
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
