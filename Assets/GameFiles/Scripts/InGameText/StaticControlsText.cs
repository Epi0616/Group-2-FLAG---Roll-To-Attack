using UnityEngine;
using UnityEngine.Localization;

public class StaticControlsText : MonoBehaviour
{
    [SerializeField] protected TMPro.TextMeshProUGUI tmpAsset;
    [SerializeField] protected LocalizedString localizedStringKey;
    [SerializeField] protected LocalizedString localizedStringPad;

    private float timer = 1;

    protected virtual void OnEnable()
    {
        localizedStringKey.StringChanged += UpdateTextKey;
        localizedStringPad.StringChanged += UpdateTextPad;
        UISelectionManager.switchToKeyboard += SwitchControlsKey;
        UISelectionManager.switchToGamepad += SwitchControlsPad;
    }

    protected virtual void OnDisable()
    {
        localizedStringKey.StringChanged -= UpdateTextKey;
        localizedStringPad.StringChanged -= UpdateTextPad;
        UISelectionManager.switchToKeyboard -= SwitchControlsKey;
        UISelectionManager.switchToGamepad -= SwitchControlsPad;

    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0) return;
        timer = 1;

        if (UISelectionManager.instance.isGamepadActive)
        {
            SwitchControlsPad();
            return;
        }

        SwitchControlsKey();
    }

    protected virtual void Awake()
    {
        if (!localizedStringPad.IsEmpty)
        {
            UpdateTextPad(localizedStringPad.GetLocalizedString());
        }
        if (!localizedStringKey.IsEmpty)
        {
            UpdateTextKey(localizedStringKey.GetLocalizedString());
        }
    }

    protected virtual void UpdateTextKey(string newText)
    {
        if (!localizedStringKey.IsEmpty)
        {
            tmpAsset.text = localizedStringKey.GetLocalizedString();
        }
    }

    protected virtual void UpdateTextPad(string newText)
    {
        if (!localizedStringPad.IsEmpty)
        {
            tmpAsset.text = localizedStringPad.GetLocalizedString();
        }
    }

    private void SwitchControlsKey()
    {
        UpdateTextKey(null);
    }
    private void SwitchControlsPad()
    {
        UpdateTextPad(null);
    }
}
