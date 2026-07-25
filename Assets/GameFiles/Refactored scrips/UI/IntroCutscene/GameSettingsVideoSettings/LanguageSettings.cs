using UnityEngine;

public class LanguageSettings : MonoBehaviour, ILoadPlayerPrefs
{
    [SerializeField] private GameObject englishCheckMark, germanCheckMark, norwegianCheckMark;

    private void OnEnable()
    {
        TryLoadPrefs();
    }

    private void Start()
    {
        TryLoadPrefs();
    }

    public void SwapToEn()
    {
        ClearChecks();
        englishCheckMark.SetActive(true);
        string language = "en";
        LanguageManager.instance?.SetLanguage(language);
        PlayerPrefsManager.SetString(PlayerValues.Language, language);
    }

    public void SwapToDe()
    {
        ClearChecks();
        germanCheckMark.SetActive(true);
        string language = "de";
        LanguageManager.instance?.SetLanguage(language);
        PlayerPrefsManager.SetString(PlayerValues.Language, language);
    }

    public void SwapToNo()
    {
        ClearChecks();
        norwegianCheckMark.SetActive(true);
        string language = "no";
        LanguageManager.instance?.SetLanguage(language);
        PlayerPrefsManager.SetString(PlayerValues.Language, language);
    }

    private void ClearChecks()
    { 
        englishCheckMark.SetActive(false);
        germanCheckMark.SetActive(false);
        norwegianCheckMark.SetActive(false);
    }

    public void TryLoadPrefs()
    {
        string language = "";
        language = PlayerPrefsManager.GetString(PlayerValues.Language);

        switch (language)
        { 
            case "en":
                SwapToEn();
                break;
            case "de":
                SwapToDe();
                break;
            case "no":
                SwapToNo();
                break;
            default:
                break;
        }
    }
}

