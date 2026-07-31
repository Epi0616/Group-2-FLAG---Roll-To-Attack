using UnityEngine;

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

    public EnhancedFreezeStatus(float fragileMult, string effectText, Color colour, int enhancementLevel) : base(fragileMult, effectText, colour)
    {
        this.enhancementLevel = enhancementLevel;
        this.effectColour = Color.deepSkyBlue;
        this.isStackable = true;
        hasProcced = false;
        damageTaken = 0;
    }
    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        if (type == DamageType.Shattered || hasProcced) { toBeRemoved = true; return; }

        damageTaken += (int)damage.GetFinalValue();
        if (damageTaken > shatterThreshold)
        {
            float extraShatteredDamage = (damageTaken * 0.5f) * enhancementLevel;
            entityRef.OnTakeDamage((int)extraShatteredDamage, Color.deepSkyBlue, DamageType.Shattered);
            entityRef.textDisplaySystem.DisplayHigherText("SHATTERED", Color.deepSkyBlue, 64);
            hasProcced = true;
        }

        
    }

    protected override void OnRemoval()
    {
        base.OnRemoval();
        //Debug.Log("E-Freeze Removed");
    }

}
