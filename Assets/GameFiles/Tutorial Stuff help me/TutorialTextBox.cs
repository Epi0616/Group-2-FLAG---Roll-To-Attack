using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialTextBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;

    public void DisplayText(string text)
    {
        tmp.text = text;
    }
}
