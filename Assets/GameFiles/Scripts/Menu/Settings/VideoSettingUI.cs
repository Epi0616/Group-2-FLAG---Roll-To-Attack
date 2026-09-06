using UnityEngine;
using System;

public class VideoSettingUI : MonoBehaviour, ILoadPlayerPrefs
{
    public static event Action<bool> toggleFPSVisibility;
    public static event Action<bool> toggleVSync;
    public static event Action<bool> togglePostProcessing;

    [SerializeField] private GameObject fpsCounterCheckMark;
    [SerializeField] private GameObject vSyncCheckMark;
    [SerializeField] private GameObject postProcessingCheckMark;

    private void OnEnable()
    {
        TryLoadPrefs();
    }

    private void Start()
    {
        TryLoadPrefs();
    }

    public void ToggleFPSVisibility()
    {
        fpsCounterCheckMark.SetActive(!fpsCounterCheckMark.activeSelf);
        toggleFPSVisibility?.Invoke(fpsCounterCheckMark.activeSelf);
        PlayerPrefsManager.instance?.SetBool(PlayerValues.FPS, fpsCounterCheckMark.activeSelf);
    }

    public void ToggleVsync()
    {
        vSyncCheckMark.SetActive(!vSyncCheckMark.activeSelf);
        toggleVSync?.Invoke(vSyncCheckMark.activeSelf);
        PlayerPrefsManager.instance?.SetBool(PlayerValues.VSync, vSyncCheckMark.activeSelf);
    }

    public void TogglePostProcessing()
    {
        postProcessingCheckMark.SetActive(!postProcessingCheckMark.activeSelf);
        togglePostProcessing?.Invoke(postProcessingCheckMark.activeSelf);
        PlayerPrefsManager.instance?.SetBool(PlayerValues.PostProcessing, postProcessingCheckMark.activeSelf);
    }

    public void TryLoadPrefs()
    {
        if (!PlayerPrefsManager.instance) return;

        if (PlayerPrefsManager.instance.GetBool(PlayerValues.FPS, out bool fps))
        {
            fpsCounterCheckMark.SetActive(fps);
            toggleFPSVisibility?.Invoke(fps);
        }

        if (PlayerPrefsManager.instance.GetBool(PlayerValues.VSync, out bool vsync))
        {
            vSyncCheckMark.SetActive(vsync);
            toggleVSync?.Invoke(vsync);
        }

        if (PlayerPrefsManager.instance.GetBool(PlayerValues.PostProcessing, out bool postProcessing))
        {
            postProcessingCheckMark.SetActive(postProcessing);
            togglePostProcessing?.Invoke(postProcessing);
        }
    }
}
