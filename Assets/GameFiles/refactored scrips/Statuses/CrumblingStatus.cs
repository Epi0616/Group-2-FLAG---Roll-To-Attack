using UnityEngine;

public class CrumblingStatus : StatusEffect
{
    protected float crumblingDamageMult;

    public CrumblingStatus(float crumbleMult)
    {
        type = StatusType.Crumbling;
        crumblingDamageMult = crumbleMult;
        //this.effectText = effectText;
        isStackable = true;
    }

    protected override void ApplyStatModifier()
    {
        (entityRef as IKnockbackable).slammedDamageMod.AddMultiplierFlat(crumblingDamageMult);
    }

    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        if (type == DamageType.Slammed) { toBeRemoved = true; return; }
    }

}
