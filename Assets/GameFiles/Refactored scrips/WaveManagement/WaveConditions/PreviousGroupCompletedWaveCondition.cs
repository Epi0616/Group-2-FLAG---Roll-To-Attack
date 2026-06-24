using System;
using UnityEngine;

[Serializable]
public class PreviousGroupCompletedWaveCondition : BaseWaveCondition
{
    public PreviousGroupCompletedWaveCondition() { }
    public PreviousGroupCompletedWaveCondition(float timer)
    {

    }
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
        return new PreviousGroupCompletedWaveCondition();
    }
}
