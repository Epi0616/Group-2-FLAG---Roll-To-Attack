using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ParticleEffectDatabase : MonoBehaviour
{
    public static ParticleEffectDatabase Instance { get; private set; }
    private Dictionary<ParticleType, GameObject> EffectStorage = new();
    [SerializeField] private List<EffectDictEntry> EffectDictEntries;
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        EffectStorage = new Dictionary<ParticleType, GameObject>();

        foreach (EffectDictEntry entry in EffectDictEntries)
        {
            EffectStorage[entry.type] = entry.prefab;
        }
    }

    public GameObject ReturnParticlePrefab(ParticleType type)
    {
        if (EffectStorage.ContainsKey(type))
        {
            return EffectStorage[type];
        }
        else
        {
            Debug.LogError("Attempted to return Invalid Effect Key");
            return null;
        }
    }
}

[Serializable]
public class EffectDictEntry
{
    public ParticleType type;
    public GameObject prefab;
}
public enum ParticleType { SimpleBurst01, WaveBurst01 }