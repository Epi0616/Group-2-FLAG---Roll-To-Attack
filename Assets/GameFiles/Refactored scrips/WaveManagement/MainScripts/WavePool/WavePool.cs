using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WavePool : IWavePool
{
    [SerializeField] private List<EntityBlockObj> EntityBlockObjs = new List<EntityBlockObj>();
    [SerializeField] private int WaveRestriction = 0;
    private List<EntityBlock> EntityBlocks = new List<EntityBlock>();

    public List<EntityBlockObj> entityBlockObjs { get => EntityBlockObjs; set => EntityBlockObjs = value; }
    public List<EntityBlock> entityBlocks { get => EntityBlocks; set => EntityBlocks = value; }
    public int waveRestriction { get => WaveRestriction; set => WaveRestriction = value; }

    public WavePool(List<EntityBlockObj> entityBlockObjs, int waveRestriction) //for cloning
    {
        this.entityBlockObjs = entityBlockObjs;
        this.waveRestriction = waveRestriction;
    }

    public WavePool Clone()
    {
        return new WavePool(this.entityBlockObjs, waveRestriction);
    }
}
