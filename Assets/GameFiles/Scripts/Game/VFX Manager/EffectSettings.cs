using System;
using UnityEngine;

public class EffectSettings
{
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


    public EffectSettings(Color? overrideColour = null, rangePair? overrideScale = null, rangePair? overrideLifetime = null, rangePair? overrideSpeed = null,
        rangePair? overrideGravity = null, Vector3? overrideInitialVelocity = null, float? overrideVelocitySpeedMult = null, Material? overrideMaterial = null,
        float? overrideVelocityDampening = null, float? overrideShapeArc = null, float? overrideShapeRadius = null)
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
    }
    
    public void ApplySettings(ParticleSystem particleSystem, ParticleSystemRenderer particleRenderer)
    {
        ParticleSystem.MainModule main = particleSystem.main;
        ParticleSystem.VelocityOverLifetimeModule velocity = particleSystem.velocityOverLifetime;
        ParticleSystem.LimitVelocityOverLifetimeModule limit = particleSystem.limitVelocityOverLifetime;
        ParticleSystem.ShapeModule shape = particleSystem.shape;
        if (overrideColour is Color OverrideColour)
        {
            //Debug.Log("Colour Override");
            main.startColor = OverrideColour;
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
            shape.arc = OverrideShapeArc;
        }
        if (overrideShapeRadius is float OverrideShapeRadius)
        {
            shape.radius = OverrideShapeRadius;
        }
    }

    // amount per burst?, shape, lifetime, scale
}

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
