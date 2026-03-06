using UnityEngine;

public class StunResistanceEffect : StatusEffect
{
    private float resistanceMultiplier;

    public StunResistanceEffect(float duration, float resistanceMultiplier, string effectText)
    {
        timer = duration;
        this.resistanceMultiplier = resistanceMultiplier;
        this.effectText = effectText;
    }

    public override void ApplyStatModifier(EnemyStateController enemy)
    {
        enemy.stunTimeStat.AddMultiplier(resistanceMultiplier);
    }
}
