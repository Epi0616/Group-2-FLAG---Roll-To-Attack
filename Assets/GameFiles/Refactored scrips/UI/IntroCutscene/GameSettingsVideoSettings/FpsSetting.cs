using System;
using UnityEngine;

public class fpsSetting : MonoBehaviour
{
    public static event Action<bool> setFPSVisibility;
    [SerializeField] GameObject tickBox;

    public void ToggleFPSVisibility()
    {
        setFPSVisibility?.Invoke(tickBox.activeSelf);
    }
}
