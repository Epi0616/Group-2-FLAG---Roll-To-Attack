using System;
using UnityEngine;

public class VsyncSetting : InteractableTickBox
{
    public static event Action<bool> toggleVSync;

    public override void Toggle()
    {
        toggleVSync?.Invoke(isActive);
        PlayerPrefsManager.SetBool(PlayerValues.VSync, isActive);
    }

    public override void TryLoadPrefs()
    {
        if (PlayerPrefsManager.GetBool(PlayerValues.VSync, out bool vsync))
        {
            isActive = vsync;
            SetAlpha(isActive ? 1 : 0);
            toggleVSync?.Invoke(isActive);
        }
    }
}
