using UnityEngine;

public class StunResistanceEffect : StatusEffect
{
    private float resistanceMultiplier;

    public StunResistanceEffect(float duration, float resistanceMultiplier)
    {
        timer = duration;
        this.resistanceMultiplier = resistanceMultiplier;
    }

    public override void ApplyStatModifier(EnemyStateController enemy)
    {
        enemy.stunTimeStat.AddMultiplier(resistanceMultiplier);
    }
}
