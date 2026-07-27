using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class LanguageTickBox : InteractableTickBox, IPointerDownHandler
{
    public static event Action newLanguageSelected;

    [SerializeField] private string language;

    private void OnEnable()
    {
        newLanguageSelected += HandleNewLanguage;
    }

    private void OnDisable()
    {
        newLanguageSelected -= HandleNewLanguage;
    }

    protected void HandleNewLanguage()
    { 
        isActive = false;
        SetAlpha(0);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        newLanguageSelected?.Invoke();
        isActive = !isActive;

        if (isActive)
        {
            animationManager.PlayAnimation(AnimationType.WakeUp, 1, MixerType.main, 0.2f);
        }
        Toggle();
        SetAlpha(isActive ? 1 : 0);
    }

    public override void Toggle()
    {
        LanguageManager.instance?.SetLanguage(language);
        PlayerPrefsManager.instance?.SetString(PlayerValues.Language, language);
    }

    public override void TryLoadPrefs()
    {
        string selectedLanguage = PlayerPrefsManager.instance?.GetString(PlayerValues.Language);
        if (selectedLanguage == language)
        {
            newLanguageSelected?.Invoke();
            isActive = true;
            Toggle();
            SetAlpha(1);
        }
    }
}
