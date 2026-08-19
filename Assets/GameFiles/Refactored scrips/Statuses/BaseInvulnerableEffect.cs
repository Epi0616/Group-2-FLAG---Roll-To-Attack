using UnityEngine;

public class BaseInvulnerableEffect : StatusEffect
{
    protected override void OnApplication()
    {
        Debug.Log("applying invulnerable");
        isInvulnerable = true;
    }

    protected override void OnRemoval()
    {
        base.OnRemoval();
        Debug.Log("removing invulnerable");
    }

    public override StatusEffect Clone()
    {
        return new BaseInvulnerableEffect();
    }
}
