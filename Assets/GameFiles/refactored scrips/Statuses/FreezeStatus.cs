using UnityEngine;

public class FreezeStatus : BaseStunEffect
{
    private float fragileMultiplier; 

    public FreezeStatus(float fragileMult, string effectText)
    {
        type = StatusType.Freeze;
        fragileMultiplier = fragileMult;
        this.effectText = effectText;
        Debug.Log("Freeze Applied, it is: " + isActive);
    }

    protected override void ApplyStatModifier()
    {       
        (entityRef as IKnockbackable).slammedDamageMod.AddMultiplierFlat(fragileMultiplier);       
    }

    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        if (type == DamageType.Shattered) { toBeRemoved = true; }
    }

    protected override void OnApplication()
    {
        base.OnApplication();

        //  NEED BODY SYSTEM entityRef.StartVibrating();
    }

    protected override void OnUpdate()
    {
        //  NEED BODY SYSTEM entityRef.Vibrate();
        //entityRef.bodySystem.Vibrate();
    }

    protected override void OnRemoval()
    {
        
        // NEED BODY SYSTEM  entityRef.StopVibrating();
        
        base.OnRemoval();
    }

}
