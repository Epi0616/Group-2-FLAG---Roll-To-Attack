using UnityEngine;

public interface IEnhancedStatusEffect
{
    public int enhancementLevel { get; set; }
}

public class EnhancedFreezeStatus : FreezeStatus , IEnhancedStatusEffect
{
    public int enhancementLevel { get; set; }
    private bool hasProcced = false;

    public EnhancedFreezeStatus(float fragileMult, string effectText, int enhancementLevel) : base(fragileMult, effectText)
    {
        this.enhancementLevel = enhancementLevel;
        this.effectColour = Color.deepSkyBlue;
        this.isStackable = true;
        hasProcced = false;
    }
    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        if (type == DamageType.Shattered || hasProcced) { toBeRemoved = true; return; }

        float extraShatteredDamage = (damage.GetBaseValue() * 0.3f) * enhancementLevel;

        entityRef.OnTakeDamage((int)extraShatteredDamage, Color.yellow, DamageType.Shattered);
        entityRef.textDisplaySystem.DisplayHigherText("SHATTERED", Color.yellow, 64);
        hasProcced = true;
    }

    protected override void OnRemoval()
    {
        base.OnRemoval();
        Debug.Log("E-Freeze Removed");
    }

}
