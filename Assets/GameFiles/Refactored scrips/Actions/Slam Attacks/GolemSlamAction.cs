using UnityEngine;
using System;

[Serializable]
public class GolemSlamAction : BaseSlamAction
{
    public GolemSlamAction() { }
    public GolemSlamAction(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool DoesPrevent) : base(slamDamage, chargeTime, slamRange, slamPositionOffset, slamColour, DoesPrevent)
    {

    }

    public override void SpawnSlamCompleteVFX()
    {
        Vector3 pos = slamOrigin;
        pos.y += 1.5f;
        ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.RockBurst01), pos, Quaternion.Euler(90, 0, 0)).
                GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(overrideColourHues: new rangePair(0.4f, 1f), overrideBurstCount: new rangePair(3, 10), overrideSpeed: new rangePair(15, 20), overrideShapeRadius: slamRange.GetFinalValue()));
        ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.RockBurst02), pos, Quaternion.Euler(0, 0, 0)).
                GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(overrideColourHues: new rangePair(0.4f, 1f)));
        ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.SmokeBurst01), pos, Quaternion.Euler(90, 0, 0)).
                GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(overrideShapeRadius: slamRange.GetFinalValue()));
    }

    public override BaseEntityAction Clone()
    {
        return new GolemSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement);
    }
}
