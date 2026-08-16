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
    public EnhancedFreezeStatus(float fragileMult, string effectText, Color colour, int enhancementLevel) : base(fragileMult, effectText, colour)
    {
        this.enhancementLevel = enhancementLevel;
        this.effectColour = Color.deepSkyBlue;
        this.isStackable = true;
        hasProcced = false;
        damageTaken = 0;
        shatteredText = LocalizationSettings.StringDatabase.GetLocalizedString("Damage Text Lables", "damageText.shattered");
    }
    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        if (type == DamageType.Shattered || hasProcced) { toBeRemoved = true; return; }

        damageTaken += (int)damage.GetFinalValue();
        if (damageTaken > shatterThreshold && !hasProcced)
        {
            hasProcced = true;
            float extraShatteredDamage = (damageTaken * 0.5f) * enhancementLevel;
            entityRef.OnTakeDamage((int)extraShatteredDamage, Color.deepSkyBlue, DamageType.Shattered);
            entityRef.textDisplaySystem.DisplayHigherText(shatteredText, Color.deepSkyBlue, 64);
        }

        
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
