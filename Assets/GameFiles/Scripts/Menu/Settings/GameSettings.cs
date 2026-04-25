using UnityEngine;
using System;

public class GameSettings : MonoBehaviour
{
    public static event Action<bool> toggleFullScreen;

    [SerializeField] private GameObject languageNote;
    [SerializeField] private GameObject fullScreenCheckMark;
    public void ToggleLanguageNoteVisibility()
    {
        languageNote.SetActive(!languageNote.activeSelf);
    }

    public void ToggleFullScreen()
    { 
        fullScreenCheckMark.SetActive(!fullScreenCheckMark.activeSelf);
        Screen.fullScreen = fullScreenCheckMark.activeSelf;
    }
}
