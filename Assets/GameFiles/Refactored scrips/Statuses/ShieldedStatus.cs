using System.Buffers;
using UnityEngine;

public class ShieldedStatus : StatusEffect
{
    private IShieldable shieldable;
    private int stacks;
    public ShieldedStatus(int stacks)
    {
        type = StatusType.Shield;
        this.stacks = stacks;
    }

    protected override void OnApplication()
    {
        if (!(entityRef is IShieldable shieldable)) { Debug.Log("owner entity is not of type IShieldable"); return; }       
        this.shieldable = shieldable;
        shieldable.initialShieldStacks = stacks;
        shieldable.currentShieldStacks = stacks;
        shieldable.HandleUpdateShieldStacks();
    }

    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        //damage.AddMultiplier(0);
        if (type == DamageType.Normal)
        {
            shieldable.currentShieldStacks--;
        }

        if (shieldable.currentShieldStacks <= 0)
        {
            toBeRemoved = true;
        }
        shieldable.HandleUpdateShieldStacks();
    }

    protected override void OnRemoval()
    {
        base.OnRemoval();
    }

    public override StatusEffect Clone()
    {
        return new ShieldedStatus(stacks);
    }
}
