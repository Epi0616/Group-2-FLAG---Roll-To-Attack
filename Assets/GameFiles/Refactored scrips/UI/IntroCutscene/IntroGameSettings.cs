using UnityEngine;

public class IntroGameSettings : MonoBehaviour
{
    [SerializeField] private GameObject fullScreenCheckMark;


    //Game Settings
    public void ToggleFullScreen()
    {
        fullScreenCheckMark.SetActive(!fullScreenCheckMark.activeSelf);
        Screen.fullScreen = fullScreenCheckMark.activeSelf;
    }

    //Language
}
