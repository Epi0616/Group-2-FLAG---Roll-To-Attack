using UnityEngine;

public class FullScreenSetting : MonoBehaviour
{
    [SerializeField] private GameObject tickBox;

    public void ToggleFullScreen()
    {
        Screen.fullScreen = tickBox.activeSelf;
    }
}
