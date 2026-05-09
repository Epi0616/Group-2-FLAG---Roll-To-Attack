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

        rebind = actionReference.action.PerformInteractiveRebinding(bindingIndex);
        rebind.OnComplete(operation =>
        {
            actionReference.action.Enable();
            rebind.Dispose();
            UpdateText(actionReference.action.bindings[bindingIndex].ToDisplayString());
            Debug.Log("sucess");
        });
        rebind.OnCancel(operation =>
        {
            actionReference.action.Enable();
            UpdateText(actionReference.action.bindings[bindingIndex].ToDisplayString());
            rebind.Dispose();
            Debug.Log("failure");
        });


        Debug.Log("Rebinding index: " + bindingIndex);
        Debug.Log("Binding path before: " + actionReference.action.bindings[bindingIndex].path);
        Debug.Log("Expected control type: " + actionReference.action.expectedControlType);
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
    

}
