using UnityEngine;

public class FreezeStatus : BaseStunEffect
{
    private float fragileMultiplier; 

    public FreezeStatus(float fragileMult, string effectText)
    {
        type = StatusType.Freeze;
        fragileMultiplier = fragileMult;
        this.effectText = effectText;
        
    }

    protected override void ApplyStatModifier()
    {
        enemyRef.wallSlamDamageModifierStat.AddMultiplier(fragileMultiplier);
    }

    protected override void OnApplication()
    {
        base.OnApplication();
        enemyRef.StartVibrating();
    }

    protected override void OnUpdate()
    {
        enemyRef.Vibrate();
    }

    protected override void OnRemoval()
    {
        if (!enemyRef.isSpawning)
        {
            enemyRef.StopVibrating();
        }
        base.OnRemoval();
    }

}
