using UnityEngine;

public class SlowEffect : StatusEffect
{
    private float slowingMultiplier;

    public SlowEffect(float duration, float slowingMultiplier)
    { 
        timer = duration;
        this.slowingMultiplier = slowingMultiplier;
    }

    public override void ApplyStatModifier(EnemyStateController enemy)
    {
        enemy.moveSpeedStat.AddMultiplier(slowingMultiplier);
    }
}
