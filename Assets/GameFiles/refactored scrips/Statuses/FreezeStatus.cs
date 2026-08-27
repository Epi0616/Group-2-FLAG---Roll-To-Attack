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
        if (stunable == null) return;
        if (!stunable.canBeStunned) return;
        (entityRef as IKnockbackable).slammedDamageMod.AddMultiplierFlat(fragileMultiplier);
    }

    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        if (stunable == null) return;
        if (!stunable.canBeStunned) return;
        if (type == DamageType.Shattered) { toBeRemoved = true; return; }
    }

    protected override void OnApplication()
    {
        base.OnApplication();
        if (entityRef is IAnimated temp)
        {
            temp.animationManager.PauseCurrentAnimation(MixerType.main);
            temp.animationManager.PauseCurrentAnimation(MixerType.complimentary);
        }
    }

    protected override void OnFirstStackApplication()
    {
        if (stunable == null) return;
        if (!stunable.canBeStunned) return;
        entityRef.bodySystem.ApplyShader(effectColour, 0.25f, ShaderType.Frozen);
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
        //entityRef.bodySystem.RemoveFreezeShader();
        //entityRef.bodySystem.RemoveFreezeShader();
        entityRef.bodySystem.RemoveShader(0.2f, ShaderType.Frozen);
        if (entityRef is IAnimated temp)
        {
            temp.animationManager.ResumeCurrentAnimation(MixerType.main);
            temp.animationManager.ResumeCurrentAnimation(MixerType.complimentary);
        }
    }

    public override StatusEffect Clone()
    {
        return new FreezeStatus(fragileMultiplier, effectText, effectColour);
    }

}
