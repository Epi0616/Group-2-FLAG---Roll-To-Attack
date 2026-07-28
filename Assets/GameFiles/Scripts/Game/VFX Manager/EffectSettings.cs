using System;
using UnityEngine;

public class EffectSettings
{
    public Color? overrideColour;
    public rangePair? overrideScale;
    public rangePair? overrideLifetime;
    public rangePair? overrideSpeed;
    public rangePair? overrideGravity;
    
    public EffectSettings(Color? overrideColour = null, rangePair? overrideScale = null, rangePair? overrideLifetime = null, rangePair? overrideSpeed = null, rangePair? overrideGravity = null)
    {
        this.overrideColour = overrideColour;
        this.overrideScale = overrideScale;      
        this.overrideLifetime = overrideLifetime;
        this.overrideSpeed = overrideSpeed;
        this.overrideGravity = overrideGravity;
    }
    
    public void ApplySettings(ParticleSystem particleSystem)
    {
        ParticleSystem.MainModule main = particleSystem.main;
        if (overrideColour is Color OverrideColour)
        {
            Debug.Log("Colour Override");
            main.startColor = OverrideColour;
        }
        if (overrideScale is rangePair OverrideScale)
        {
            Debug.Log("Scale Override");
            main.startSize = new ParticleSystem.MinMaxCurve(OverrideScale.min, OverrideScale.max);
        }
        if (overrideLifetime is rangePair OverrideLifetime)
        {
            Debug.Log("LifeTime Override");
            main.startLifetime = new ParticleSystem.MinMaxCurve(OverrideLifetime.min, OverrideLifetime.max);
        }
        if (overrideSpeed is rangePair OverrideSpeed)
        {
            Debug.Log("Speed Override");
            main.startSpeed = new ParticleSystem.MinMaxCurve(OverrideSpeed.min, OverrideSpeed.max);
        }
        if (overrideGravity is rangePair OverrideGravity)
        {
            main.gravityModifier = new ParticleSystem.MinMaxCurve(OverrideGravity.min, OverrideGravity.max);
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
