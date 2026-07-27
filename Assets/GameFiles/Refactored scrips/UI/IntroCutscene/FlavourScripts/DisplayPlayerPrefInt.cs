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
        if (!PlayerPrefsManager.instance) return;
        int number = PlayerPrefsManager.instance.GetInt(value);
        text.text = number.ToString();
    }
}
