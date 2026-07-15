using System;
using UnityEngine;

public class VsyncSetting : MonoBehaviour
{
    public static event Action<bool> toggleVSync;
    [SerializeField] GameObject tickBox;

    public void ToggleVsync()
    {
        tickBox.SetActive(!tickBox.activeSelf);
        toggleVSync?.Invoke(tickBox.activeSelf);
    }
}
