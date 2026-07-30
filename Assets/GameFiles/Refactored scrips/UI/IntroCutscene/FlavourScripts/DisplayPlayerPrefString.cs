using TMPro;
using UnityEngine;

public class DisplayPlayerPrefString : MonoBehaviour, ILoadPlayerPrefs
{
    [SerializeField] protected PlayerValues value;
    [SerializeField] protected TextMeshProUGUI text;

    protected virtual void OnEnable()
    {
        TryLoadPrefs();
    }

    protected virtual void Start()
    {
        TryLoadPrefs();
    }

    public virtual void TryLoadPrefs()
    {
        string word = PlayerPrefsManager.instance?.GetString(value);
        text.text = word;
    }
}
