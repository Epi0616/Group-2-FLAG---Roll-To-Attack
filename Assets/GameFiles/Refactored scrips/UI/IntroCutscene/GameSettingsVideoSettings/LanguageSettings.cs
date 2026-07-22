using UnityEngine;

public class LanguageSettings : MonoBehaviour
{
    //[SerializeField] private GameObject englishCheckMark, germanCheckMark, norwegianCheckMark;

    public void SwapToEn()
    {
        ClearChecks();
        //englishCheckMark.SetActive(true);
        LanguageManager.instance?.SetLanguage("en");
    }

    public void SwapToDe()
    {
        ClearChecks();
        //germanCheckMark.SetActive(true);
        LanguageManager.instance?.SetLanguage("de");
    }

    public void SwapToNo()
    {
        ClearChecks();
        //norwegianCheckMark.SetActive(true);
        LanguageManager.instance?.SetLanguage("no");
    }

    private void ClearChecks()
    { 
        //englishCheckMark.SetActive(false);
        //germanCheckMark.SetActive(false);
        //norwegianCheckMark.SetActive(false);
    }
}

