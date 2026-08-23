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
        base.ApplyCustomEffectPerEntity(hitEntity);
        if (!(slamRange.GetFinalValue() > slamRange.GetBaseValue()))
        {
            hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new KnockbackEffect(ownerEntity.transform.position, 7f),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true));
        }

        hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new CrumblingStatus(CrumblingDamageMod),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true));
    }

    protected override void ApplyHeavyEffectPerEntity(Entity hitEntity)
    {
        float percentage = slamRange.GetFinalValue() / slamRange.GetBaseValue();
        hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new KnockbackEffect(ownerEntity.transform.position, 7f * percentage),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true),
            Color.red);
    }
    public override void ExtraSlamEffect()
    {
        KnockbackField KBField = (ObjectPoolManager.SpawnObject(IKBFS.knockbackFieldPrefab, slamOrigin, Quaternion.identity)).GetComponent<KnockbackField>();
        KBField.Initialize(ownerEntity, CrumblingDamageMod, slamRange.GetBaseValue() / 2, 5f, slamColour, enhancementLevel);
    }

    public override BaseEntityAction Clone()
    {
        return new EnhancedKnockbackSlam(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, CrumblingDamageMod, preventsMovement, enhancementLevel);
    }
}
