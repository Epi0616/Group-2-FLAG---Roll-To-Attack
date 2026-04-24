using UnityEngine;
using UnityEngine.Localization;

public class StaticText : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI tmpAsset;
    [SerializeField] private LocalizedString localizedString;

    private void OnEnable()
    {
        localizedString.StringChanged += UpdateText;   
    }

    private void OnDisable()
    {
        localizedString.StringChanged -= UpdateText;
    }

    private void UpdateText(string newText)
    { 
        tmpAsset.text = localizedString.GetLocalizedString();
    }

}
