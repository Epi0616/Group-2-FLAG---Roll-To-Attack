using UnityEngine;

public class VideoSettingsManager : MonoBehaviour
{
    public static GameObject instance;

    private void OnEnable()
    {
        VideoSettingUI.toggleVSync += ToggleVsync;
    }

    private void OnDisable()
    {
        VideoSettingUI.toggleVSync -= ToggleVsync;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = gameObject;
            return;
        }

        Destroy(gameObject);
    }

    private void ToggleVsync(bool state)
    {
        QualitySettings.vSyncCount = state ? 1 : 0;
    }
}
