using UnityEngine;
using System;

[Serializable]
public class PoisonSlamAction : BaseSlamAction
{
    public float lifespan = 10;
    public float tickDamage = 8;
    public GameObject poisonFieldPrefab;
    
    PoisonSlamAction() { }

    public override void SpawnSlamStartVFX()
    {
        impactField = ObjectPoolManager.SpawnObject(slamImpactField, slamOrigin, Quaternion.identity).GetComponent<ImpactFieldVisual>();
        impactField.PassInValuesColorRadiusChargeTimeFlash(slamColour, slamRange, chargeTime, false);
    }

    public override void ExtraSlamEffect()
    {
        GameObject poisonField = ObjectPoolManager.SpawnObject(poisonFieldPrefab, slamOrigin, Quaternion.identity);
        poisonField.GetComponent<PoisonField>().Initialize(ownerEntity, slamRange, lifespan, tickDamage, slamColour);
    }

    public override BaseEntityAction Clone()
    {
        return new PoisonSlamAction();
    }
}
