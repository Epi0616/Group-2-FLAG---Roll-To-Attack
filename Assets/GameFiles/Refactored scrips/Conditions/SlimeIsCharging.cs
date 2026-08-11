using System;
using UnityEngine;

[Serializable]
public class SlimeIsCharging : BaseCondition
{
    private ISlimeTrail slimeTrail;

    public SlimeIsCharging() { }
    public SlimeIsCharging(bool inverse)
    {
        this.inverse = inverse;
    }

    public override void Initialize(Entity entity)
    {
        if (!(entity is ISlimeTrail slimeTrail)) { Debug.LogError("entity is not of type ISlimeSplit"); return; }
        this.slimeTrail = slimeTrail;
    }

    public override void ConditionUpdate()
    {

    }

    public override void ResetCondition()
    {

    }

    public override bool IsConditionMet()
    {
        return inverse? !slimeTrail.isCharging : slimeTrail.isCharging;
    }
    public override BaseCondition Clone()
    {
        return new SlimeIsCharging(inverse);
    }
}
