using UnityEngine;
using System;

[Serializable]
public class EnhancedPoisonSlamAction : BaseSlamAction , IEnhancedAbility
{
    private IPoisonSpawner poisonAccess;
    public int enhancementLevel { get; set; }

    public EnhancedPoisonSlamAction() { }
    public EnhancedPoisonSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent, int enhancementLevel) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {
        this.enhancementLevel = enhancementLevel;
    }

    public override void StartAction(Entity entity)
    {
        base.StartAction(entity);
        poisonAccess = entity as IPoisonSpawner;
        if (poisonAccess == null)
        {
            EndAction();
        }
    }

    public override void SpawnSlamStartVFX()
    {
        impactField = ObjectPoolManager.SpawnObject(slamVariablesAccess.slamImpactField, slamOrigin, Quaternion.identity).GetComponent<ImpactFieldVisual>();
        impactField.PassInValuesColorRadiusChargeTimeFlash(slamColour, slamRange.GetFinalValue(), chargeTime, false);
    }


    public override void ExtraSlamEffect()
    {
        GameObject poisonField = ObjectPoolManager.SpawnObject(poisonAccess.enhancedPoisonFieldObj, slamOrigin, Quaternion.identity);
        poisonField.GetComponent<EnhancedPoisonField>().Initialize(ownerEntity, slamRange.GetFinalValue(), poisonAccess.fieldLifetime, poisonAccess.fieldTickDamage, slamColour, enhancementLevel);
    }

    protected override void ApplyHeavyEffect(Entity hitEntity)
    {

    }

    public override BaseEntityAction Clone()
    {
        return new EnhancedPoisonSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement, enhancementLevel);
    }
}
