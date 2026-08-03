using UnityEngine;
using System;

[Serializable]
public class EnemyBaseSlam : BaseSlamAction
{
    public EnemyBaseSlam() { }
    public EnemyBaseSlam(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {

    }

    public override void SpawnSlamCompleteVFX()
    {
        
    }

    public override BaseEntityAction Clone()
    {
        return new EnemyBaseSlam(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement);
    }
}
