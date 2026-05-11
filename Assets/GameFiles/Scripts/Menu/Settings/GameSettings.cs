using UnityEngine;
using System;

public class GameSettings : MonoBehaviour
{
    //public static event Action<bool> toggleFullScreen;
    public static Action<bool> autoStart;

    [SerializeField] private GameObject languageNote;
    [SerializeField] private GameObject fullScreenCheckMark, autoStartCheckMark;
    public void ToggleLanguageNoteVisibility()
    {
        languageNote.SetActive(!languageNote.activeSelf);
    }

    public void ToggleFullScreen()
    { 
        fullScreenCheckMark.SetActive(!fullScreenCheckMark.activeSelf);
        Screen.fullScreen = fullScreenCheckMark.activeSelf;
    }

    public void ToggleAutoStart()
    {
        autoStartCheckMark.SetActive(!autoStartCheckMark.activeSelf);
        autoStart?.Invoke(autoStartCheckMark.activeSelf);
    }
}
