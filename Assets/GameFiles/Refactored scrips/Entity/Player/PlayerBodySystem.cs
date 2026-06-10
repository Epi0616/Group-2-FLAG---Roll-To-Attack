using UnityEngine;

public class PlayerBodySystem : EntityBodySystem
{
    public ParticleSystem chargeCompleteEffect;
    public ParticleSystem chargingEffect;

    public override void InitialiseSystem(Entity entity)
    {
        base.InitialiseSystem(entity);
        chargeCompleteEffect.Stop();
        chargingEffect.Stop();
    }

    public override void ResetSystem()
    {
        base.ResetSystem();
    }

    public void DisplayChargingEffect()
    {
        if (chargingEffect.isPlaying) { return; }
        chargingEffect.Play();
    }

    public void DisplayChargeCompleteEffect()
    {
        if (chargeCompleteEffect.isPlaying) { return; }
        chargeCompleteEffect.Play();
    }

    public void ResetChargingEffects()
    {
        if (chargingEffect == null || chargeCompleteEffect == null) { return; }
        if (chargingEffect.isPlaying)
        {
            chargingEffect.Stop();
        }
        if (chargeCompleteEffect.isPlaying)
        {
            chargeCompleteEffect.Stop();
        }
    }
}
