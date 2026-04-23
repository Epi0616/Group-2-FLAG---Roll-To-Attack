using UnityEngine;

public class LanguageNote : MonoBehaviour
{
    public void SwapToEn()
    {
        LanguageManager.instance.SetLanguage("en");
    }

    public void SwapToDe()
    {
        LanguageManager.instance.SetLanguage("de");
    }
}
