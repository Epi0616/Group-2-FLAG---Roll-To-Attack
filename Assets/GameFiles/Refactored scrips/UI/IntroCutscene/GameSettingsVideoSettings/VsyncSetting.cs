using System;
using UnityEngine;

public class VsyncSetting : MonoBehaviour, ILoadPlayerPrefs
{
    public static event Action<bool> toggleVSync;
    [SerializeField] GameObject tickBox;

    private void OnEnable()
    {
        TryLoadPrefs();
    }

    private void Start()
    {
        TryLoadPrefs();
    }

    public void ToggleVsync()
    {
        tickBox.SetActive(!tickBox.activeSelf);
        toggleVSync?.Invoke(tickBox.activeSelf);
        PlayerPrefsManager.SetBool(PlayerValues.VSync, tickBox.activeSelf);
    }

    public void TryLoadPrefs()
    {
        if (PlayerPrefsManager.GetBool(PlayerValues.VSync, out bool vsync))
        {
            tickBox.SetActive(vsync);
            toggleVSync?.Invoke(vsync);
        }
    }
}
