using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Wave : IWave
{
    [SerializeField] private List<WaveGroupObj> WaveGroupObjs = new List<WaveGroupObj>();
    [SerializeField] private WaveType WaveType;
    private List<WaveGroup> WaveGroups = new List<WaveGroup>();

    public Wave(List<WaveGroup> waveGroups, WaveType waveType) //for creation at runtime
    {
        this.waveGroups = waveGroups;
        this.waveType = waveType;
    }
    public Wave(List<WaveGroupObj> waveGroupObjs, WaveType waveType) //for cloning
    { 
        this.waveGroupObjs = waveGroupObjs;
        this.waveType = waveType;
    }

    public List<WaveGroupObj> waveGroupObjs { get => WaveGroupObjs; set => WaveGroupObjs = value; }
    public List<WaveGroup> waveGroups { get => WaveGroups; set => WaveGroups = value; }
    public WaveType waveType { get => WaveType; set => WaveType = value; }

    public Wave Clone()
    { 
        return new Wave(waveGroupObjs, waveType);
    }
}

public enum WaveType
{ 
    normal,
    DragonBoss,
    SlimeBoss
}
