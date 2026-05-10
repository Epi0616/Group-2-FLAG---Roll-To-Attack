using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;

public class ReBindButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpAsset;
    [SerializeField] private LocalizedString setKeyString;
    [SerializeField] private InputActionReference actionReference;
    [SerializeField] private int bindingIndex;

    private InputActionRebindingExtensions.RebindingOperation rebind;

    private void Awake()
    {
        UpdateText(actionReference.action.bindings[bindingIndex].ToDisplayString());
    }

    public void ReBind()
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
            UpdateText(actionReference.action.bindings[bindingIndex].ToDisplayString());
            StartCoroutine(EnableAction());
            Debug.Log("sucess");
        });
        rebind.OnCancel(operation =>
        {
            UpdateText(actionReference.action.bindings[bindingIndex].ToDisplayString());
            StartCoroutine(EnableAction());
            rebind.Dispose();
            Debug.Log("failure");
        });

        rebind.Start();
    }

    public void ResetBinding()
    {
        actionReference.action.RemoveBindingOverride(bindingIndex);
        UpdateText(actionReference.action.bindings[bindingIndex].ToDisplayString());
    }

    private void UpdateText(string newText)
    {
        tmpAsset.text = newText;
    }

    private IEnumerator EnableAction()
    {
        yield return new WaitForSecondsRealtime(0.15f);

        actionReference.action.Enable();
    }
}
