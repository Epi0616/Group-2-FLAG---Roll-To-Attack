using UnityEngine;
using System;

[Serializable]
public class PoisonSlamAction : BaseSlamAction
{
    private IPoisonSpawner poisonAccess;
    public float lifespan = 10;
    public float tickDamage = 8;
    private GameObject poisonFieldPrefab;
    
    public PoisonSlamAction() { }

    public PoisonSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, float PoisonLifeSpan, float PoisonTickDamage) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour)
    {
        lifespan = PoisonLifeSpan;
        tickDamage = PoisonTickDamage;
    }

    public override void StartAction(Entity entity)
    {
        base.StartAction(entity);
        poisonAccess = entity as IPoisonSpawner;
        if (poisonAccess != null)
        {
            poisonFieldPrefab = poisonAccess.PoisonFieldObj;
        }
       
        
    }

    public override void SpawnSlamStartVFX()
    {
        impactField = ObjectPoolManager.SpawnObject(slamVariablesAccess.SlamImpactField, slamOrigin, Quaternion.identity).GetComponent<ImpactFieldVisual>();
        impactField.PassInValuesColorRadiusChargeTimeFlash(slamColour, slamRange, chargeTime, false);
    }

    public override void ExtraSlamEffect()
    {
        GameObject poisonField = ObjectPoolManager.SpawnObject(poisonAccess.PoisonFieldObj, slamOrigin, Quaternion.identity);
        poisonField.GetComponent<PoisonField>().Initialize(ownerEntity, slamRange, lifespan, tickDamage, slamColour);
    }

    public override BaseEntityAction Clone()
    {
        return new PoisonSlamAction(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, lifespan, tickDamage);
    }
}
