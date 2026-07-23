using System;
using UnityEngine;

public class VsyncSetting : MonoBehaviour
{
    public static event Action<bool> toggleVSync;
    [SerializeField] GameObject tickBox;

    public void ToggleVsync()
    {
        toggleVSync?.Invoke(tickBox.activeSelf);
    }
}
