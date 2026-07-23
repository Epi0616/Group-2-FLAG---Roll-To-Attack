using System;
using UnityEngine;

public class PostProcessingSetting : MonoBehaviour
{
    public static event Action<bool> togglePostProcessing;
    [SerializeField] GameObject tickBox;

    public void TogglePostProcessing()
    {
        togglePostProcessing?.Invoke(tickBox.activeSelf);
    }
}
