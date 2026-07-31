using UnityEngine;

public class FreezeStatus : BaseStunEffect
{
    protected float fragileMultiplier; 

    public FreezeStatus(float fragileMult, string effectText, Color colour)
    {
        type = StatusType.Freeze;
        fragileMultiplier = fragileMult;
        this.effectText = effectText;
        effectColour = colour;
        //Debug.Log("Freeze Applied, it is: " + isActive);
    }

    protected override void ApplyStatModifier()
    {       
        (entityRef as IKnockbackable).slammedDamageMod.AddMultiplierFlat(fragileMultiplier);       
    }

    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        if (type == DamageType.Shattered) { toBeRemoved = true; return; }
    }

    protected override void OnApplication()
    {
        base.OnApplication();
        entityRef.bodySystem.ApplyFreezeShader(effectColour);
        (entityRef as IAnimated).animationManager.EndCurrentAnimation(MixerType.main);
        //  NEED BODY SYSTEM entityRef.StartVibrating();
    }

    protected override void OnUpdate()
    {
        //  NEED BODY SYSTEM entityRef.Vibrate();
        entityRef.bodySystem.Vibrate();
    }

    protected override void OnRemoval()
    {

        // NEED BODY SYSTEM  entityRef.StopVibrating();
        //Debug.Log("Freeze Over");
        entityRef.bodySystem.RemoveFreezeShader();
        base.OnRemoval();
    }

}
