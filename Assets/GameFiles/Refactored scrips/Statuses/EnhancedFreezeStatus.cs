using UnityEngine;

public interface IEnhancedStatusEffect
{
    public int enhancementLevel { get; set; }
}

public class EnhancedFreezeStatus : FreezeStatus , IEnhancedStatusEffect
{
    public int enhancementLevel { get; set; }

    public EnhancedFreezeStatus(float fragileMult, string effectText, int enhancementLevel) : base(fragileMult, effectText)
    {
        this.enhancementLevel = enhancementLevel;
        this.effectColour = Color.deepSkyBlue;
    }
    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        if (type == DamageType.Shattered) { toBeRemoved = true; return; }
        float extraShatteredDamage = (damage.GetBaseValue() * 0.3f) * enhancementLevel;
        entityRef.OnTakeDamage((int)extraShatteredDamage, Color.yellow, DamageType.Shattered);
    }

}
