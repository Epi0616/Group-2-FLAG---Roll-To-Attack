using UnityEngine;
using System;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private GameObject languageNote;

    public void ToggleLanguageNoteVisibility()
    {
        languageNote.SetActive(!languageNote.activeSelf);
    }
}
