using System;
using UnityEngine;

[Serializable]
public abstract class BaseWaveCondition : IWaveCondition
{
    protected WaveSpawner owner;
    public abstract void Initialize(WaveSpawner owner);
    public abstract void UpdateCondition();
    public abstract bool IsConditionMet();
    public abstract BaseWaveCondition Clone();
}
