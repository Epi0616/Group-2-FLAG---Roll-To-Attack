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
        isStackable = true;
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
        if (entityRef is IAnimated temp)
        {
            temp.animationManager.EndCurrentAnimation(MixerType.main);
        }
    }

    protected override void OnFirstStackApplication()
    {
        entityRef.bodySystem.ApplyFreezeShader(effectColour);
    }

    protected override void OnUpdate()
    {
        entityRef.bodySystem.Vibrate();
    }

    protected override void OnRemoval()
    {
        base.OnRemoval();
    }

    protected override void OnLastStackRemoval()
    {
        //Debug.Log("Final Freeze Stack For: " + entityRef.gameObject.name);
        entityRef.bodySystem.RemoveFreezeShader();
        //entityRef.bodySystem.RemoveFreezeShader();
    }

    public override StatusEffect Clone()
    {
        return new FreezeStatus(fragileMultiplier, effectText, effectColour);
    }

}
