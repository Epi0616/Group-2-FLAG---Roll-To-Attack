using UnityEngine;

public class SlowStatus : StatusEffect
{
    private float slowMultiplier;

    public SlowStatus(float slowMultiplier, string effectText)
    {
        type = StatusType.Slow;
        this.slowMultiplier = slowMultiplier;
        this.effectText = effectText;
        isStackable = true;
    }

    protected override void ApplyStatModifier()
    {
        enemyRef.moveSpeedStat.AddMultiplier(slowMultiplier);
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
