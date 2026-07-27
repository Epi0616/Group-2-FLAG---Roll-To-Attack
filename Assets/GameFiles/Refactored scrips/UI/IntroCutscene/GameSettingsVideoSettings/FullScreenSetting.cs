using UnityEngine;

public class FullScreenSetting : InteractableTickBox
{
    public override void Toggle()
    {
        Screen.fullScreen = isActive;
        PlayerPrefsManager.SetBool(PlayerValues.FullScreen, targetGraphic.activeSelf);
    }

    public override void TryLoadPrefs()
    {
        if (PlayerPrefsManager.GetBool(PlayerValues.FullScreen, out bool fullScreen))
        {
            isActive = fullScreen;
            SetAlpha(isActive ? 1 : 0);
            Screen.fullScreen = isActive;
        }
    }
}
