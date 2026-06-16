using System;
using UnityEngine;

[Serializable]
public class AlwaysTrueWaveCondition : BaseWaveCondition
{
    public AlwaysTrueWaveCondition() { }
    public override void Initialize(WaveSpawner owner)
    {
        this.owner = owner;
    }
    public override void UpdateCondition()
    {
    }
    public override bool IsConditionMet()
    {
        return true;
    }
    public override BaseWaveCondition Clone()
    {
        return new AlwaysTrueWaveCondition();
    }
}
