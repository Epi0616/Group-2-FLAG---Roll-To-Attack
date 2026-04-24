using UnityEngine;
using System;

public class GameSettings : MonoBehaviour
{
    public static event Action toggleFPSVisibility;
    [SerializeField] private GameObject languageNote;

    public void ToggleFPSVisibility()
    {
        toggleFPSVisibility.Invoke();
    }

    public void ToggleLanguageNoteVisibility()
    {
        languageNote.SetActive(!languageNote.activeSelf);
    }
}
