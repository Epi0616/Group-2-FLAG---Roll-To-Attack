using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class NameScript : MonoBehaviour
{
    public static event Action nameChosen;
    [SerializeField] private TMP_InputField inputField;

    private void OnEnable()
    {
        TryLoadPrefs();
    }

    private void Start()
    {
        TryLoadPrefs();
    }

    public void UpdateName()
    {
        PlayerPrefsManager.SetString(PlayerValues.Name, inputField.text);
        nameChosen?.Invoke();
    }

    public void TryLoadPrefs()
    {
        string name = PlayerPrefsManager.GetString(PlayerValues.Name);
        inputField.text = name;
    }
}
