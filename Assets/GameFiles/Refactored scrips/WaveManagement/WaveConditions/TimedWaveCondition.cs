using System;
using UnityEngine;

[Serializable]
public class TimedWaveCondition : BaseWaveCondition
{
    public float timer;
    public TimedWaveCondition() { }
    public TimedWaveCondition(float timer)
    { 
        this.timer = timer;    
    }
    public override void Initialize(WaveSpawner owner)
    { 
        this.owner = owner;
    }
    public override void UpdateCondition()
    { 
        timer -= Time.deltaTime;
    }
    public override bool IsConditionMet()
    {
        return (timer <= 0);
    }
    public override BaseWaveCondition Clone()
    {
        return new TimedWaveCondition(timer);
    }
}
