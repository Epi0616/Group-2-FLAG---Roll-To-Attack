using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class WaveGroup : IWaveGroup
{
    [SerializeField] private List<EntityBlockObj> EntityBlockObjs = new List<EntityBlockObj>();
    [SerializeReference, SubclassSelector] private List<BaseWaveCondition> Conditions = new List<BaseWaveCondition>();
    private List<EntityBlock> EntityBlocks = new List<EntityBlock>();

    public WaveGroup(List<EntityBlock> entityBlocks, List<BaseWaveCondition> conditions) //for creation at runtime
    { 
        this.entityBlocks = entityBlocks;
        this.conditions = conditions;
    }

    public WaveGroup(List<EntityBlockObj> entityBlockObjs, List<BaseWaveCondition> conditions) //for cloning
    { 
        this.entityBlockObjs = entityBlockObjs;
        this.conditions = conditions;
    }

    public List<EntityBlockObj> entityBlockObjs { get => EntityBlockObjs; set => EntityBlockObjs = value; }
    public List<EntityBlock> entityBlocks { get => EntityBlocks; set => EntityBlocks = value; }
    public List<BaseWaveCondition> conditions { get => Conditions; set => Conditions = value; }

    public WaveGroup Clone()
    { 
        return new WaveGroup(this.entityBlockObjs, conditions.Select(c => c.Clone()).ToList());
    }
}
