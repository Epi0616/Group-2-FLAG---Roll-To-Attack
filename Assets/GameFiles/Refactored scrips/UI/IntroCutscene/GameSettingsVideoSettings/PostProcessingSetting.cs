using System;
using UnityEngine;

public class PostProcessingSetting : MonoBehaviour, ILoadPlayerPrefs
{
    public static event Action<bool> togglePostProcessing;
    [SerializeField] GameObject tickBox;

    private void OnEnable()
    {
        TryLoadPrefs();
    }

    private void Start()
    {
        TryLoadPrefs();
    }

    public void TogglePostProcessing()
    {
        tickBox.SetActive(!tickBox.activeSelf);
        togglePostProcessing?.Invoke(tickBox.activeSelf);
        PlayerPrefsManager.SetBool(PlayerValues.PostProcessing, tickBox.activeSelf);
    }

    public void TryLoadPrefs()
    {
        if (PlayerPrefsManager.GetBool(PlayerValues.PostProcessing, out bool postProcessing))
        {
            tickBox.SetActive(postProcessing);
            togglePostProcessing?.Invoke(postProcessing);
        }
    }
}
