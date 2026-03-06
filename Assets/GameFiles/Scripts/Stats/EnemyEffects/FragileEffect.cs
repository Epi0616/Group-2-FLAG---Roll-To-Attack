using UnityEngine;

public class FragileEffect : StatusEffect
{
    private float fragileMultiplier;

    public FragileEffect(float duration, float fragileMultiplier, string effectText)
    {
        timer = duration;
        this.fragileMultiplier = fragileMultiplier;
        this.effectText = effectText;
    }

    public override void ApplyStatModifier(EnemyStateController enemy)
    {
        enemy.wallSlamDamageModifierStat.AddMultiplier(fragileMultiplier);
    }
}
