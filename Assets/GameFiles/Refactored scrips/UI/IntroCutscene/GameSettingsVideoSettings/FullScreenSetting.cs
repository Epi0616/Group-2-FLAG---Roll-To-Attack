using UnityEngine;

public class FullScreenSetting : MonoBehaviour
{
    [SerializeField] private GameObject tickBox;

    public void ToggleFullScreen()
    {
        tickBox.SetActive(!tickBox.activeSelf);
        Screen.fullScreen = tickBox.activeSelf;
    }
}
