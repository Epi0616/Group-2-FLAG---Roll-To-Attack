using UnityEngine;

public class LanguageNote : MonoBehaviour
{
    private void OnEnable()
    {
        SettingsUIManager.settingsCleared += Hide;
    }

    private void OnDisable()
    {
        SettingsUIManager.settingsCleared -= Hide;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SwapToEn()
    {
        LanguageManager.instance.SetLanguage("en");
    }

    public void SwapToDe()
    {
        LanguageManager.instance.SetLanguage("de");
    }

    public void SwapToNo()
    {
        LanguageManager.instance.SetLanguage("no");
    }
}
