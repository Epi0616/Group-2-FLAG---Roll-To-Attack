using UnityEngine;

public class FullScreenSetting : MonoBehaviour, ILoadPlayerPrefs
{
    [SerializeField] private GameObject tickBox;

    private void OnEnable()
    {
        TryLoadPrefs();
    }

    private void Start()
    {
        TryLoadPrefs();
    }

    public void ToggleFullScreen()
    {
        tickBox.SetActive(!tickBox.activeSelf);
        Screen.fullScreen = tickBox.activeSelf;
        PlayerPrefsManager.SetBool(PlayerValues.FullScreen, tickBox.activeSelf);
    }

    public void TryLoadPrefs()
    {
        if (PlayerPrefsManager.GetBool(PlayerValues.FullScreen, out bool fullScreen))
        {
            tickBox.SetActive(fullScreen);
            Screen.fullScreen = fullScreen;
        }
    }
}
