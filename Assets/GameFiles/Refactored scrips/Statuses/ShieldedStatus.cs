using UnityEngine;

public class ShieldedStatus : StatusEffect
{
    private int stacks;
    public ShieldedStatus(int stacks)
    {
        this.stacks = stacks;
    }

    protected override void OnApplication()
    {
        Debug.Log("ShieldedStatus applied to " + entityRef.name);
        base.OnApplication();
        if (entityRef is IShieldable shieldable)
        {
            shieldable.shielded = true;
        }
    }

    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        Debug.Log("blocking dmg through shield");
        damage.AddMultiplier(0);
        stacks--;

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
}
