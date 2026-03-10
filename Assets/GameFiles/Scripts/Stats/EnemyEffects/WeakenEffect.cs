using Unity.VisualScripting;
using UnityEngine;

public class WeakenEffect : StatusEffect
{
    private float weakeningMultiplier;

    public WeakenEffect(float duration, float weakeningMultiplier, string effectText) //use 1.x to modify damage dealt
    { 
        timer = duration;
        this.weakeningMultiplier = weakeningMultiplier;
        this.effectText = effectText;
    }

    public override void ApplyStatModifier(EnemyStateController enemy)
    {
        enemy.damageTakenModifierStat.AddMultiplierFlat(weakeningMultiplier);
    }
}
