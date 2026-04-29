using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }

        Destroy(gameObject);
    }

    public void SetLanguage(string localeCode)
    {
        StartCoroutine(SetLanguageRoutine(localeCode));
    }

    private IEnumerator SetLanguageRoutine(string localeCode)
    {
        yield return LocalizationSettings.InitializationOperation;

        var availableLocales = LocalizationSettings.AvailableLocales.Locales;

        foreach (var locale in availableLocales)
        {
            if (locale.Identifier.Code == localeCode)
            {
                LocalizationSettings.SelectedLocale = locale;
            }
        }
    }
}
