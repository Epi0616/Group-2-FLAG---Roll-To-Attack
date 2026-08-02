using UnityEngine;

public class SlamBlockedStatus : StatusEffect
{
    //Based on ShieldedStatus

    private int stacks;
    private float multiplier;

    public SlamBlockedStatus(int stacks, float multiplier)
    {
        this.stacks = stacks;
        this.multiplier = multiplier;
    }

    protected override void OnApplication()
    {
        base.OnApplication();
        if (entityRef is ISlamBlock slamBlock)
        {
            slamBlock.blockingSlam = true;
        }
    }

    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        damage.AddMultiplier(multiplier);

        if (type == DamageType.Normal)
        {
            stacks--;

            if (stacks <= 0)
            {
                toBeRemoved = true;
            }
        }
    }

    protected override void OnRemoval()
    {
        base.OnRemoval();
        if (entityRef is ISlamBlock slamBlock)
        {
            slamBlock.blockingSlam = false;
        }
    }
}
