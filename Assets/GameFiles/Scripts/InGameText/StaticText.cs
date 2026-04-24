using UnityEngine;
using UnityEngine.Localization;

public class StaticText : MonoBehaviour
{
    [SerializeField] protected TMPro.TextMeshProUGUI tmpAsset;
    [SerializeField] protected LocalizedString localizedString;

    protected virtual void OnEnable()
    {
        localizedString.StringChanged += UpdateText;   
    }

    protected virtual void OnDisable()
    {
        localizedString.StringChanged -= UpdateText;
    }

    protected virtual void Awake()
    {
        tmpAsset.text = localizedString.GetLocalizedString();
    }

    protected virtual void UpdateText(string newText)
    { 
        tmpAsset.text = localizedString.GetLocalizedString();
    }

}
