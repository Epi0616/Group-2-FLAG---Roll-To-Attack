using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public interface IEnhancedStatusEffect
{
    public int enhancementLevel { get; set; }
}

public class EnhancedFreezeStatus : FreezeStatus , IEnhancedStatusEffect
{
    public int enhancementLevel { get; set; }
    private bool hasProcced = false;
    private int damageTaken = 0;
    private int shatterThreshold = 50;
    private string shatteredText;
    private bool canBeShattered = true;
    public EnhancedFreezeStatus(float fragileMult, string effectText, Color colour, int enhancementLevel) : base(fragileMult, effectText, colour)
    {
        this.enhancementLevel = enhancementLevel;
        this.effectColour = Color.deepSkyBlue;
        this.isStackable = true;
        hasProcced = false;
        damageTaken = 0;
        if (entityRef is BaseBossEnemy)
        {
            canBeShattered = false;
        }
        shatteredText = LocalizationSettings.StringDatabase.GetLocalizedString("Damage Text Lables", "damageText.shattered");
    }
    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        if (type == DamageType.Shattered || hasProcced) { toBeRemoved = true; return; }

        if ((entityRef.healthSystem.currentHealth - damage.GetFinalValue()) < (entityRef.healthSystem.maxHealth * (0.3f + (enhancementLevel / 10))))
        {
            if (canBeShattered && !hasProcced)
            {
                hasProcced = true;
                Vector3 newPos = entityRef.transform.position;
                newPos.y += 5;
                ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.RockBurst01), newPos, Quaternion.Euler(90, 0, 0)).
                GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> {
                    new BurstCountEffectOverride(new rangePair(3, 5)),
                    new StartLifetimeEffectOverride(new rangePair(2f, 2.5f)),
                    new StartSpeedEffectOverride(new rangePair(5, 7)),
                    new ShapeRadiusEffectOverride(2),
                    new IceMeshEffectOverride()
                }));
                entityRef.textDisplaySystem.DisplayHigherText(shatteredText, Color.deepSkyBlue * 10, 64);
                entityRef.OnTakeDamage(entityRef.healthSystem.currentHealth, Color.deepSkyBlue, DamageType.Shattered);
               

            }
            
        }

        //damageTaken += (int)damage.GetFinalValue();
        //if (damageTaken > shatterThreshold && !hasProcced)
        //{
        //    hasProcced = true;
        //    float extraShatteredDamage = (damageTaken * 0.5f) * enhancementLevel;
        //    entityRef.OnTakeDamage((int)extraShatteredDamage, Color.deepSkyBlue, DamageType.Shattered);
        //    entityRef.textDisplaySystem.DisplayHigherText(shatteredText, Color.deepSkyBlue, 64);
        //}


    }

    protected override void OnRemoval()
    {
        base.OnRemoval();
        //Debug.Log("E-Freeze Removed");
    }
    public override StatusEffect Clone()
    {
        return new EnhancedFreezeStatus(fragileMultiplier, effectText, effectColour, enhancementLevel);
    }
}
