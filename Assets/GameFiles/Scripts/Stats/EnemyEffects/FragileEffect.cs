using UnityEngine;

public class FragileEffect : StatusEffect
{
    private float fragileMultiplier;

    public FragileEffect(float duration, float fragileMultiplier)
    {
        timer = duration;
        this.fragileMultiplier = fragileMultiplier;
    }

    public override void ApplyStatModifier(EnemyStateController enemy)
    {
        enemy.wallSlamDamageModifierStat.AddMultiplier(fragileMultiplier);
    }
}
