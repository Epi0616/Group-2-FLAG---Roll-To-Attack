using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable]
public class BaseSlamActionLevel3 : BaseSlamAction
{
    public BaseSlamActionLevel3() { }
    public BaseSlamActionLevel3(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent)
    {
        this.slamDamage = slamDamage;
        this.chargeTime = chargeTime;
        this.slamRange = new Stat(slamRange);
        this.slamPositionOffset = slamPositionOffset;
        this.slamColour = slamColour;
        preventsMovement = DoesPrevent;
    }

    /// <summary>
    /// IGNOREEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE ABOVEEEEEE
    /// </summary>
    /// <returns></returns>

    public override void SpawnSlamStartVFX()
    {
        //2. how can you spawn this impact field on the player no matter where the attack starts from?

        if (attackInterrupted) { return; }
        impactField = ObjectPoolManager.SpawnObject(slamVariablesAccess.slamImpactField, ownerEntity.target.transform.position, Quaternion.identity).GetComponent<ImpactFieldVisual>();

        bool flashRed = false;
        if (chargeTime > 0)
        {
            flashRed = true;
        }
        impactField.PassInValuesColorRadiusChargeTimeFlash(slamColour, slamRange.GetFinalValue(), chargeTime, flashRed);
    }

    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {

        if (slamDamage == 0) { return; }
        //hitEntity.OnTakeDamage(100, new Color(0, 255, 255, 255), DamageType.Normal);

    }



    /// <summary>
    /// IGNOREEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE BELOWWWWWWW
    /// </summary>
    /// <returns></returns>

    public override BaseEntityAction Clone()
    {
        return new BaseSlamActionLevel3(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement);
    }
}
// slamVariablesAccess.defaultSlamColour