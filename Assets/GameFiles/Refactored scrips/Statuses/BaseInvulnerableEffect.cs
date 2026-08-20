using UnityEngine;

public class BaseInvulnerableEffect : StatusEffect
{
    public BaseInvulnerableEffect() 
    {
        type = StatusType.Invulnerable;
    }

    protected override void OnApplication()
    {
        isInvulnerable = true;
    }

    protected override void OnRemoval()
    {
        base.OnRemoval();
    }

    public override StatusEffect Clone()
    {
        return new BaseInvulnerableEffect();
    }
}
