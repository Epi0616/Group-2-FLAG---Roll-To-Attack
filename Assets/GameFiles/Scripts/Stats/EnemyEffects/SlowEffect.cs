using Unity.VisualScripting;
using UnityEngine;

public class SlowEffect : StatusEffect
{
    private float slowingMultiplier;

    public SlowEffect(float duration, float slowingMultiplier, string effectText)
    { 
        timer = duration;
        this.slowingMultiplier = slowingMultiplier;
        this.effectText = effectText;
    }

    public override void ApplyStatModifier(EnemyStateController enemy)
    {
        enemy.moveSpeedStat.AddMultiplier(slowingMultiplier);
    }
}
