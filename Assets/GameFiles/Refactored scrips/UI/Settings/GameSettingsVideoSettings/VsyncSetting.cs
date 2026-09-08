using System;
using UnityEngine;

public class VsyncSetting : InteractableTickBox
{
    public static event Action<bool> toggleVSync;

    public override void Toggle()
    {
        toggleVSync?.Invoke(isActive);
        PlayerPrefsManager.instance?.SetBool(PlayerValues.VSync, isActive);
    }

    public override void TryLoadPrefs()
    {
        if (!PlayerPrefsManager.instance) return;

        if (PlayerPrefsManager.instance.GetBool(PlayerValues.VSync, out bool vsync))
        {
            isActive = vsync;
            SetAlpha(isActive ? 1 : 0);
            toggleVSync?.Invoke(isActive);
        }
    }
}
