using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Wave : IWave
{
    [SerializeField] private List<WaveGroupObj> WaveGroupObjs = new List<WaveGroupObj>();
    private List<WaveGroup> WaveGroups = new List<WaveGroup>();

    public Wave(List<WaveGroup> waveGroups) //for creation at runtime
    {
        this.waveGroups = waveGroups;
    }
    public Wave(List<WaveGroupObj> waveGroupObjs) //for cloning
    { 
        this.waveGroupObjs = waveGroupObjs;
    }

    public List<WaveGroupObj> waveGroupObjs { get => WaveGroupObjs; set => WaveGroupObjs = value; }
    public List<WaveGroup> waveGroups { get => WaveGroups; set => WaveGroups = value; }

    public Wave Clone()
    { 
        return new Wave(waveGroupObjs);
    }
}
