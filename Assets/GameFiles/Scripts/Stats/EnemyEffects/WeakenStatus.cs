using UnityEngine;

public class WeakenStatus : StatusEffect
{
    private float weakMultiplier;

    public WeakenStatus(float weakMultiplier, string effectText)
    {
        type = StatusType.Weak;
        this.weakMultiplier = weakMultiplier;
        this.effectText = effectText;
        isStackable = true;
    }

    protected override void ApplyStatModifier()
    {
        enemyRef.damageTakenModifierStat.AddMultiplierFlat(weakMultiplier);
    }
    /*
    protected override void OnApplication()
    {
        base.OnApplication();       
    }

    protected override void OnUpdate()
    {
        
    }

    protected override void OnRemoval()
    {    
        base.OnRemoval();
    }
    */
}
