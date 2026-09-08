using System;
using UnityEngine;

public class fpsSetting : InteractableTickBox
{
    public static event Action<bool> setFPSVisibility;

    public override void Toggle()
    {
        setFPSVisibility?.Invoke(isActive);
        PlayerPrefsManager.instance?.SetBool(PlayerValues.FPS, isActive);
    }

    public override void TryLoadPrefs()
    {
        if (!PlayerPrefsManager.instance) return;

        if (PlayerPrefsManager.instance.GetBool(PlayerValues.FPS, out bool fps))
        {
            isActive = fps;
            SetAlpha(isActive ? 1 : 0);
            setFPSVisibility?.Invoke(isActive);
        }
    }
}
