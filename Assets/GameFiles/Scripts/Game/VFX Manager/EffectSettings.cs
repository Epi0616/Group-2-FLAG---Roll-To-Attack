using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;

public class EffectSettings
{
    private List<EffectOverride> effectOverrides = new List<EffectOverride>();
    private List<ILateEffectOverride> lateEffectOverrides = new List<ILateEffectOverride>();

    public Color? overrideColour;
    public rangePair? overrideScale;
    public rangePair? overrideLifetime;
    public rangePair? overrideSpeed;
    public rangePair? overrideGravity;
    public Vector3? overrideInitialVelocity;
    public float? overrideVelocitySpeedMult;
    public Material? overrideMaterial;
    public float? overrideVelocityDampening;
    public float? overrideShapeArc;
    public float? overrideShapeRadius;
    public rangePair? overrideBurstCount;
    public rangePair? overrideColourHues;


    public EffectSettings(Color? overrideColour = null, rangePair? overrideScale = null, rangePair? overrideLifetime = null, rangePair? overrideSpeed = null,
        rangePair? overrideGravity = null, Vector3? overrideInitialVelocity = null, float? overrideVelocitySpeedMult = null, Material? overrideMaterial = null,
        float? overrideVelocityDampening = null, float? overrideShapeArc = null, float? overrideShapeRadius = null, rangePair? overrideBurstCount = null, rangePair? overrideColourHues = null)
    {
        this.overrideColour = overrideColour;
        this.overrideScale = overrideScale;
        this.overrideLifetime = overrideLifetime;
        this.overrideSpeed = overrideSpeed;
        this.overrideGravity = overrideGravity;
        this.overrideInitialVelocity = overrideInitialVelocity;
        this.overrideVelocitySpeedMult = overrideVelocitySpeedMult;
        this.overrideMaterial = overrideMaterial;
        this.overrideVelocityDampening = overrideVelocityDampening;
        this.overrideShapeArc = overrideShapeArc;
        this.overrideShapeRadius = overrideShapeRadius;
        this.overrideBurstCount = overrideBurstCount;
        this.overrideColourHues = overrideColourHues;
    }

    public EffectSettings(List<EffectOverride> overrides)
    {
        effectOverrides = overrides;
    }

    public void ApplyEffectOverrides(ParticleSystem particleSystem)
    { 
        foreach (EffectOverride effectOverride in effectOverrides)
        {
            effectOverride.ApplyOverride(particleSystem);
            if (effectOverride is ILateEffectOverride temp)
            {
                lateEffectOverrides.Add(temp);
            }
        }
    }

    public void ApplyLateEffectOverrides(ParticleSystem particleSystem)
    {
        foreach (ILateEffectOverride lateEffectOverride in lateEffectOverrides)
        {
            lateEffectOverride.ApplyLateOverride(particleSystem);
        }
        lateEffectOverrides.Clear();
    }

    public void AddOverride(EffectOverride effectOverride)
    {
        effectOverrides.Add(effectOverride);
    }


    public void ApplySettings(ParticleSystem particleSystem, ParticleSystemRenderer particleRenderer)
    {
        ParticleSystem.MainModule main = particleSystem.main;
        ParticleSystem.VelocityOverLifetimeModule velocity = particleSystem.velocityOverLifetime;
        ParticleSystem.LimitVelocityOverLifetimeModule limit = particleSystem.limitVelocityOverLifetime;
        ParticleSystem.ShapeModule shape = particleSystem.shape;
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        ParticleSystem.Burst burst = new ParticleSystem.Burst();
        if (overrideColour is Color OverrideColour)
        {
            //Debug.Log("Colour Override");
            main.startColor = OverrideColour;
            particleRenderer.material.SetColor("_BaseColour", OverrideColour);
        }
        if (overrideScale is rangePair OverrideScale)
        {
            //Debug.Log("Scale Override");
            main.startSize = new ParticleSystem.MinMaxCurve(OverrideScale.min, OverrideScale.max);
        }
        if (overrideLifetime is rangePair OverrideLifetime)
        {
            //Debug.Log("LifeTime Override");
            main.startLifetime = new ParticleSystem.MinMaxCurve(OverrideLifetime.min, OverrideLifetime.max);
        }
        if (overrideSpeed is rangePair OverrideSpeed)
        {
            //Debug.Log("Speed Override");
            main.startSpeed = new ParticleSystem.MinMaxCurve(OverrideSpeed.min, OverrideSpeed.max);
        }
        if (overrideGravity is rangePair OverrideGravity)
        {
            main.gravityModifier = new ParticleSystem.MinMaxCurve(OverrideGravity.min, OverrideGravity.max);
        }
        if (overrideInitialVelocity is Vector3 OverrideVelocity)
        {
            velocity.x = OverrideVelocity.x;
            velocity.y = OverrideVelocity.y;
            velocity.z = OverrideVelocity.z;
        }
        if (overrideVelocitySpeedMult is float OverrideVelocitySpeedMult)
        {
            velocity.speedModifier = OverrideVelocitySpeedMult;
        }
        if (overrideVelocityDampening is float OverrideVelocityDampening)
        {
            limit.dampen = OverrideVelocityDampening;
        }
        if (overrideShapeArc is float OverrideShapeArc)
        {
            shape.arcSpread = OverrideShapeArc;
        }
        if (overrideShapeRadius is float OverrideShapeRadius)
        {
            shape.radius = OverrideShapeRadius;
        }
        if (overrideBurstCount is rangePair OverrideBurstCount)
        {
            burst.count = new ParticleSystem.MinMaxCurve(OverrideBurstCount.min, OverrideBurstCount.max);
            emission.SetBurst(0, burst);
        }

    }

    public void ApplyLateSettings(ParticleSystem particleSystem, ParticleSystemRenderer particleRenderer)
    {
        ParticleSystem.MainModule main = particleSystem.main;
        //ParticleSystem.VelocityOverLifetimeModule velocity = particleSystem.velocityOverLifetime;
        //ParticleSystem.LimitVelocityOverLifetimeModule limit = particleSystem.limitVelocityOverLifetime;
        //ParticleSystem.ShapeModule shape = particleSystem.shape;
        //ParticleSystem.EmissionModule emission = particleSystem.emission;
        //ParticleSystem.Burst burst = new ParticleSystem.Burst();

        if (overrideColourHues is rangePair OverrideColourHues)
        {
            //Debug.Log("Overriding Hue");
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[main.maxParticles];
            int size = particleSystem.GetParticles(particles);

            //Debug.Log("Size is: " + size);
            for (int i = 0; i < size; i++)
            {
                Color newColour = particles[i].startColor;
                float hue = UnityEngine.Random.Range(OverrideColourHues.min, OverrideColourHues.max);
                particles[i].startColor = newColour * hue;
                particleRenderer.material.SetColor("_BaseColour", newColour * hue);

            }

            particleSystem.SetParticles(particles, size);
        }
    }

    // amount per burst?, shape, lifetime, scale
}

[Serializable]
public struct rangePair
{
    public float min;
    public float max;
    public rangePair(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
