using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public class WavePool : IWavePool
{
    [SerializeField] private List<EntityBlockObj> EntityBlockObjs = new List<EntityBlockObj>();
    [SerializeField] private int WaveRestriction = 0;
    private List<EntityBlock> EntityBlocks = new List<EntityBlock>();
    public LocalizedString SpawnAnnouncement;
    public List<EntityBlockObj> entityBlockObjs { get => EntityBlockObjs; set => EntityBlockObjs = value; }
    public List<EntityBlock> entityBlocks { get => EntityBlocks; set => EntityBlocks = value; }
    public int waveRestriction { get => WaveRestriction; set => WaveRestriction = value; }

    public WavePool(List<EntityBlockObj> entityBlockObjs, int waveRestriction, LocalizedString spawn) //for cloning
    {
        this.entityBlockObjs = entityBlockObjs;
        this.waveRestriction = waveRestriction;
        this.SpawnAnnouncement = spawn;
    }

    public WavePool Clone()
    {
        return new WavePool(this.entityBlockObjs, waveRestriction, SpawnAnnouncement);
    }
}
