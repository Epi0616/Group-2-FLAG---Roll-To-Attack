using System;
using UnityEngine;

public class PostProcessingSetting : InteractableTickBox
{
    public static event Action<bool> togglePostProcessing;

    public override void Toggle()
    {
        togglePostProcessing?.Invoke(isActive);
        PlayerPrefsManager.instance?.SetBool(PlayerValues.PostProcessing, isActive);
    }

    public override void TryLoadPrefs()
    {
        if (!PlayerPrefsManager.instance) return;

        if (PlayerPrefsManager.instance.GetBool(PlayerValues.PostProcessing, out bool postProcessing))
        {
            isActive = postProcessing;
            SetAlpha(isActive ? 1 : 0);
            togglePostProcessing?.Invoke(isActive);
        }
    }
}
