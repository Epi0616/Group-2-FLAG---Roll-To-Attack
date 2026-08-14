using Unity.Burst;
using UnityEngine;

public class EffectStateHolder 
{
    private Color startColour;
    private float duration;
    private ParticleSystem.MinMaxCurve startScale;
    private ParticleSystem.MinMaxCurve startLifetime;
    private ParticleSystem.MinMaxCurve startSpeed;
    private ParticleSystem.MinMaxCurve gravityMod;

    private ParticleSystem.MinMaxCurve burstCount;

    private float shapeArc;
    private float shapeRadius;

    private ParticleSystem.MinMaxCurve initialVelocityX;
    private ParticleSystem.MinMaxCurve initialVelocityY;
    private ParticleSystem.MinMaxCurve initialVelocityZ;

    private ParticleSystem.MinMaxCurve velocitySpeedMod;
    private float velocityDampeningMod;

  

    public static EffectStateHolder FetchCurrentState(ParticleSystem particleSystem)
    {
        ParticleSystem.MainModule main = particleSystem.main;
        ParticleSystem.VelocityOverLifetimeModule velocity = particleSystem.velocityOverLifetime;
        ParticleSystem.LimitVelocityOverLifetimeModule limit = particleSystem.limitVelocityOverLifetime;
        ParticleSystem.ShapeModule shape = particleSystem.shape;
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        

        return new EffectStateHolder
        {
            startColour = main.startColor.color,
            duration = main.duration,
            startScale = main.startSize,
            startLifetime = main.startLifetime,
            startSpeed = main.startSpeed,
            gravityMod = main.gravityModifier,
            burstCount = emission.GetBurst(0).count,
            shapeArc = shape.arcSpread,
            shapeRadius = shape.radius,
            initialVelocityX = velocity.x,
            initialVelocityY = velocity.y,
            initialVelocityZ = velocity.z,
            velocitySpeedMod = velocity.speedModifier,
            velocityDampeningMod = limit.dampen,

        };
    }

    public void ApplyStateToParticleSystem(ParticleSystem particleSystem)
    {
        particleSystem.Stop();
        ParticleSystem.MainModule main = particleSystem.main;
        ParticleSystem.VelocityOverLifetimeModule velocity = particleSystem.velocityOverLifetime;
        ParticleSystem.LimitVelocityOverLifetimeModule limit = particleSystem.limitVelocityOverLifetime;
        ParticleSystem.ShapeModule shape = particleSystem.shape;
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        ParticleSystem.Burst burst = new ParticleSystem.Burst();
        main.startColor = startColour;
        main.duration = duration;
        main.startSize = startScale;
        main.startLifetime = startLifetime;
        main.startSpeed = startSpeed;
        main.gravityModifier = gravityMod;
        burst.count = burstCount;
        shape.arcSpread = shapeArc;
        shape.radius = shapeRadius;
        velocity.x = initialVelocityX;
        velocity.y = initialVelocityY;
        velocity.z = initialVelocityZ;
        velocity.speedModifier = velocitySpeedMod;
        limit.dampen = velocityDampeningMod;

        emission.SetBurst(0, burst);

    }
}
