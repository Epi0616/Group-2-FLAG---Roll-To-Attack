using UnityEngine;

public interface IWaveCondition
{
    public void Initialize(WaveSpawner owner);
    public void UpdateCondition();
    public bool IsConditionMet();
}
