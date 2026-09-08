using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour,
    ILoadPlayerPrefs
{
    [SerializeField] private List<MonoBehaviour> PlayerPrefSettings = new();

    public void Start()
    {
        TryLoadPrefs();
    }

    public void TryLoadPrefs()
    { 
        foreach (MonoBehaviour monoBehaviour in PlayerPrefSettings)
        {
            if (monoBehaviour is ILoadPlayerPrefs loadableSetting)
            {
                loadableSetting.TryLoadPrefs();
            }
        }
    }
}
