using UnityEngine;
using System;

public class GameSettings : MonoBehaviour
{
    public static event Action toggleFPSVisibility;

    public void ToggleFPSVisibility()
    {
        toggleFPSVisibility.Invoke();
    }
}
