using UnityEngine;
using System;

public class GameSettings : MonoBehaviour, ILoadPlayerPrefs
{
    [SerializeField] private GameObject fullScreenCheckMark;

    private void OnEnable()
    {
        TryLoadPrefs();
    }

    private void Start()
    {
        TryLoadPrefs();
    }

    public void ToggleFullScreen()
    { 
        fullScreenCheckMark.SetActive(!fullScreenCheckMark.activeSelf);
        Screen.fullScreen = fullScreenCheckMark.activeSelf;
        PlayerPrefsManager.instance?.SetBool(PlayerValues.FullScreen, fullScreenCheckMark.activeSelf);
    }

    public void TryLoadPrefs()
    {
        if (!PlayerPrefsManager.instance) return;

        if (PlayerPrefsManager.instance.GetBool(PlayerValues.FullScreen, out bool fullScreen))
        {
            fullScreenCheckMark.SetActive(fullScreen);
            Screen.fullScreen = fullScreen;
        }
    }
}
