using TMPro;
using UnityEngine;

public class DisplayPlayerPrefInt : MonoBehaviour, ILoadPlayerPrefs
{
    [SerializeField] private PlayerValues value;
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        TryLoadPrefs();
    }

    private void Start()
    {
        TryLoadPrefs();
    }

    public void TryLoadPrefs()
    {
        int number = PlayerPrefsManager.GetInt(value);
        text.text = number.ToString();
    }
}
