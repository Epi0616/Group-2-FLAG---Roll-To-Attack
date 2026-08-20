using UnityEngine;

public class ShieldedStatus : StatusEffect
{
    private int stacks;
    public ShieldedStatus(int stacks)
    {
        type = StatusType.Shield;
        this.stacks = stacks;
    }

    protected override void OnApplication()
    {
        base.OnApplication();
        if (entityRef is IShieldable shieldable)
        {
            shieldable.shielded = true;
        }
    }

    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        damage.AddMultiplier(0);
        if (type == DamageType.Normal)
        {
            stacks--;
        }

        if (stacks <= 0)
        {
            toBeRemoved = true;
        }
    }

    protected override void OnRemoval()
    {
        base.OnRemoval();
        if (entityRef is IShieldable shieldable)
        { 
            shieldable.shielded = false;
        }
    }

    public override StatusEffect Clone()
    {
        return new ShieldedStatus(stacks);
    }
}
