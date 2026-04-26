using UnityEngine;

public class KeyBindsUI : MonoBehaviour
{
    [SerializeField] private ReBindButton[] reBindButtons;

    public void ResetAll()
    { 
        for (int i = 0; i < reBindButtons.Length; i++)
        {
            reBindButtons[i].ResetBinding();
        }
    }
}
