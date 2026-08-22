using System;
using System.Collections.Generic;
using UnityEngine;

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
                GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> {                  
                    new BurstCountEffectOverride(new rangePair(3, 5)),
                    new StartSpeedEffectOverride(new rangePair(15, 20)),
                    new ShapeRadiusEffectOverride(slamRange.GetFinalValue())
                }));
        //ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.RockBurst02), pos, Quaternion.Euler(0, 0, 0)).
        //        GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> { }));
        ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.SmokeBurst01), pos, Quaternion.Euler(90, 0, 0)).
                GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> { new ShapeRadiusEffectOverride(slamRange.GetFinalValue()), new BurstCountEffectOverride(new rangePair(2, 3)) }));
    }

    public override BaseEntityAction Clone()
    {
        return new GolemSlamAction(slamDamage, chargeTime, slamRange.GetBaseValue(), slamPositionOffset, slamColour, preventsMovement);
    }
}
