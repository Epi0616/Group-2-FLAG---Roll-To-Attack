using UnityEngine;
using System;

public class VideoSettingUI : MonoBehaviour
{
    public static event Action toggleFPSVisibility;

    public void ToggleFPSVisibility()
    {
        toggleFPSVisibility.Invoke();
    }
}
