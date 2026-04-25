using UnityEngine;
using System;

public class VideoSettingUI : MonoBehaviour
{
    public static event Action toggleFPSVisibility;
    public static event Action<bool> toggleVSync;

    [SerializeField] private GameObject fpsCounterCheckMark;
    [SerializeField] private GameObject VSyncCheckMark;

    public void ToggleFPSVisibility()
    {
        fpsCounterCheckMark.SetActive(!fpsCounterCheckMark.activeSelf);
        toggleFPSVisibility.Invoke();
    }

    public void ToggleVsync()
    {
        VSyncCheckMark.SetActive(!VSyncCheckMark.activeSelf);
        toggleVSync.Invoke(VSyncCheckMark.activeSelf);
    }
}
