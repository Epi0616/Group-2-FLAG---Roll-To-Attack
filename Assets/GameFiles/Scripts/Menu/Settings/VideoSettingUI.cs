using UnityEngine;
using System;

public class VideoSettingUI : MonoBehaviour
{
    public static event Action toggleFPSVisibility;
    public static event Action<bool> toggleVSync;
    public static event Action<bool> togglePostProcessing;

    [SerializeField] private GameObject fpsCounterCheckMark;
    [SerializeField] private GameObject vSyncCheckMark;
    [SerializeField] private GameObject postProcessingCheckMark;

    public void ToggleFPSVisibility()
    {
        fpsCounterCheckMark.SetActive(!fpsCounterCheckMark.activeSelf);
        toggleFPSVisibility.Invoke();
    }

    public void ToggleVsync()
    {
        vSyncCheckMark.SetActive(!vSyncCheckMark.activeSelf);
        toggleVSync.Invoke(vSyncCheckMark.activeSelf);
    }

    public void TogglePostProcessing()
    {
        postProcessingCheckMark.SetActive(!postProcessingCheckMark.activeSelf);
        togglePostProcessing.Invoke(postProcessingCheckMark.activeSelf);
    }
}
