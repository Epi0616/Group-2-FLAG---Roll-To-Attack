using System;
using UnityEngine;

public class fpsSetting : MonoBehaviour, ILoadPlayerPrefs
{
    public static event Action<bool> setFPSVisibility;
    [SerializeField] GameObject tickBox;

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
        tickBox.SetActive(!tickBox.activeSelf);
        setFPSVisibility?.Invoke(tickBox.activeSelf);
        PlayerPrefsManager.SetBool(PlayerValues.FPS, tickBox.activeSelf);
    }

    public void TryLoadPrefs()
    {
        if (PlayerPrefsManager.GetBool(PlayerValues.FPS, out bool fps))
        {
            tickBox.SetActive(fps);
            setFPSVisibility?.Invoke(fps);
        }
    }
}
