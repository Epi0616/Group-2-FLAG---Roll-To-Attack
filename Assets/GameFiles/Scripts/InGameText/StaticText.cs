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
        if (localizedString.IsEmpty) return;

        UpdateText(localizedString.GetLocalizedString());
    }

    protected virtual void UpdateText(string newText)
    {
        if (localizedString.IsEmpty) return;

        tmpAsset.text = localizedString.GetLocalizedString();
    }

}
