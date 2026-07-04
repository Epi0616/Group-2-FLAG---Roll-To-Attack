using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnhancedKnockbackSlam : BaseSlamAction , IEnhancedAbility
{
    public float CrumblingDamageMod = 1.4f;
    private IKnockbackFieldSpawner IKBFS;
    public int enhancementLevel { get; set; }

    public EnhancedKnockbackSlam() { }

    public EnhancedKnockbackSlam(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, float CrumblingMod, bool DoesPrevent, int enhancementLevel) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        CrumblingDamageMod = CrumblingMod;
        this.enhancementLevel = enhancementLevel;
    }

    public override void StartAction(Entity entity)
    {
        base.StartAction(entity);
        IKBFS = ownerEntity as IKnockbackFieldSpawner;
    }

    public override void SpawnSlamStartVFX()
    {
        
    }

    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        hitEntity.OnTakeDamage(slamDamage, slamColour, DamageType.Normal);
    }

    public override void ExtraSlamEffect()
    {
        KnockbackField KBField = (ObjectPoolManager.SpawnObject(IKBFS.knockbackFieldPrefab, slamOrigin, Quaternion.identity)).GetComponent<KnockbackField>();
        KBField.Initialize(ownerEntity, CrumblingDamageMod, slamRange.GetFinalValue(), 5f, slamColour, enhancementLevel);
    }

    public override BaseEntityAction Clone()
    {
        return new EnhancedKnockbackSlam(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, CrumblingDamageMod, preventsMovement, enhancementLevel);
    }
}
