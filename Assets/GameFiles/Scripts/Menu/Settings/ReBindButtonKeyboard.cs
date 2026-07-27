using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;

public class ReBindButton : MonoBehaviour, ILoadPlayerPrefs
{
    [SerializeField] protected TextMeshProUGUI tmpAsset;
    [SerializeField] protected LocalizedString setKeyString;
    [SerializeField] protected InputActionReference actionReference;
    [SerializeField] protected int bindingIndex;

    protected InputActionRebindingExtensions.RebindingOperation rebind;

    protected virtual void Awake()
    {
        UpdateText(actionReference.action.bindings[bindingIndex].ToDisplayString());
    }

    public virtual void ReBind()
    {
        Debug.Log("rebinding");

        actionReference.action.Disable();

        tmpAsset.text = setKeyString.GetLocalizedString();

        rebind = actionReference.action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Gamepad>/leftStick/x")
            .WithControlsExcluding("<Gamepad>/leftStick/y")
            .WithControlsExcluding("<Gamepad>/rightStick/x")
            .WithControlsExcluding("<Gamepad>/rightStick/y");
        rebind.OnComplete(operation =>
        {
            rebind.Dispose();
            PlayCompleteAnimation();
            StartCoroutine(EnableAction());

            PlayerPrefsManager.instance?.SaveInputBindings();
        });
        rebind.OnCancel(operation =>
        {
            UpdateText(actionReference.action.bindings[bindingIndex].ToDisplayString());
            StartCoroutine(EnableAction());
            rebind.Dispose();
        });

        rebind.Start();
    }

    public virtual void ResetBinding()
    {
        actionReference.action.RemoveBindingOverride(bindingIndex);
        UpdateText(actionReference.action.bindings[bindingIndex].ToDisplayString());
        PlayerPrefsManager.instance?.SaveInputBindings();
    }

    protected virtual void UpdateText(string newText)
    {
        tmpAsset.text = newText;
    }

    protected virtual IEnumerator EnableAction()
    {
        yield return new WaitForSecondsRealtime(0.15f);

        actionReference.action.Enable();
    }

    protected virtual void PlayCompleteAnimation()
    {
        UpdateText(actionReference.action.bindings[bindingIndex].ToDisplayString());
    }

    void ILoadPlayerPrefs.TryLoadPrefs()
    {
        UpdateText(actionReference.action.bindings[bindingIndex].ToDisplayString());
    }
}
