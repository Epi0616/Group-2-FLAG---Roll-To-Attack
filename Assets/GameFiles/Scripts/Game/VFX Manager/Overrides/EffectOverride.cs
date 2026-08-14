using UnityEngine;

public abstract class EffectOverride 
{
    public abstract void ApplyOverride(ParticleSystem particleSystem);
}

public interface ILateEffectOverride
{
    void ApplyLateOverride(ParticleSystem particleSystem);
}

// Main Module Properties -----------------------------------------------------------
public class ColourEffectOverride : EffectOverride
{
    private Color colour;
    public ColourEffectOverride(Color colour) {  this.colour = colour; }
    public override void ApplyOverride(ParticleSystem particleSystem)
    {
        ParticleSystem.MainModule main = particleSystem.main;

        main.startColor = colour;
    }
}

public class ColourHueEffectOverride : EffectOverride, ILateEffectOverride
{
    private rangePair hueRange;
    public ColourHueEffectOverride(rangePair hueRange)
    {
        this.hueRange = hueRange;
    }
    public override void ApplyOverride(ParticleSystem particleSystem) { }
    public void ApplyLateOverride(ParticleSystem particleSystem)
    {
        ParticleSystem.MainModule main = particleSystem.main;
        ParticleSystemRenderer renderer = particleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[main.maxParticles];
        int size = particleSystem.GetParticles(particles);
 
        for (int i = 0; i < size; i++)
        {
            Color newColour = particles[i].startColor;
            float hue = Random.Range(hueRange.min, hueRange.max);
            particles[i].startColor = newColour * hue;
        }

        particleSystem.SetParticles(particles, size);
    }
}

public class DurationEffectOverride : EffectOverride
{
    private float duration;
    public DurationEffectOverride(float duration) { this.duration = duration; }
    public override void ApplyOverride(ParticleSystem particleSystem)
    {
        ParticleSystem.MainModule main = particleSystem.main;

        main.duration = duration;
    }
}

public class StartScaleEffectOverride : EffectOverride
{
    private ParticleSystem.MinMaxCurve scale;
    public StartScaleEffectOverride(rangePair scale)
    {
        this.scale.constantMin = scale.min;
        this.scale.constantMax = scale.max;
    }
    public override void ApplyOverride(ParticleSystem particleSystem)
    {
        ParticleSystem.MainModule main = particleSystem.main;
        main.startSize = scale;
    }
}

public class StartLifetimeEffectOverride : EffectOverride
{
    private ParticleSystem.MinMaxCurve lifetime;
    public StartLifetimeEffectOverride(rangePair lifetime)
    {
        this.lifetime.constantMin = lifetime.min;
        this.lifetime.constantMax = lifetime.max;
    }
    public override void ApplyOverride(ParticleSystem particleSystem)
    {
        ParticleSystem.MainModule main = particleSystem.main;
        main.startLifetime = lifetime;
    }
}
public class StartSpeedEffectOverride : EffectOverride
{
    private ParticleSystem.MinMaxCurve speed;
    public StartSpeedEffectOverride(rangePair speed)
    {
        this.speed.constantMin = speed.min;
        this.speed.constantMax = speed.max;
    }
    public override void ApplyOverride(ParticleSystem particleSystem)
    {
        ParticleSystem.MainModule main = particleSystem.main;
        main.startSpeed = speed;
    }
}
public class GravityModEffectOverride : EffectOverride
{
    private ParticleSystem.MinMaxCurve gravity;
    public GravityModEffectOverride(rangePair gravity)
    {
        this.gravity.constantMin = gravity.min;
        this.gravity.constantMax = gravity.max;
    }
    public override void ApplyOverride(ParticleSystem particleSystem)
    {
        ParticleSystem.MainModule main = particleSystem.main;
        main.gravityModifier = gravity;
    }
}
// Emission Module Properties ---------------------------------------

public class BurstCountEffectOverride : EffectOverride
{
    private ParticleSystem.MinMaxCurve burstCount;
    public BurstCountEffectOverride(rangePair burstCount)
    {
        this.burstCount.constantMin = burstCount.min;
        this.burstCount.constantMax = burstCount.max;
    }
    public override void ApplyOverride(ParticleSystem particleSystem)
    {
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        ParticleSystem.Burst burst = new ParticleSystem.Burst();
        burst.count = burstCount;
        emission.SetBurst(0, burst);
    }
}
// Shape Module Properties ------------------------------------------

public class ShapeArcSpreadEffectOverride : EffectOverride
{
    private float arcSpread;
    public ShapeArcSpreadEffectOverride(float arcSpread) { this.arcSpread = arcSpread; }
    public override void ApplyOverride(ParticleSystem particleSystem)
    {
        ParticleSystem.ShapeModule shape = particleSystem.shape;
        shape.arcSpread = arcSpread;
    }
}

public class ShapeRadiusEffectOverride : EffectOverride
{
    private float radius;
    public ShapeRadiusEffectOverride(float radius) { this.radius = radius; }
    public override void ApplyOverride(ParticleSystem particleSystem)
    {
        ParticleSystem.ShapeModule shape = particleSystem.shape;
        shape.radius = radius;
    }
}
// Velocity Module Properties -------------------------------------------

public class InitialVelocityEffectOverride : EffectOverride
{
    private ParticleSystem.MinMaxCurve initialVelocityX;
    private ParticleSystem.MinMaxCurve initialVelocityY;
    private ParticleSystem.MinMaxCurve initialVelocityZ;
    public InitialVelocityEffectOverride(rangePair x, rangePair y, rangePair z)
    {
        initialVelocityX.constantMin = x.min; initialVelocityX.constantMax = x.max;
        initialVelocityY.constantMin = y.min; initialVelocityY.constantMax = y.max;
        initialVelocityZ.constantMin = z.min; initialVelocityZ.constantMax = z.max;
    }
    public override void ApplyOverride(ParticleSystem particleSystem)
    {
        ParticleSystem.VelocityOverLifetimeModule velocity = particleSystem.velocityOverLifetime;
        velocity.x = initialVelocityX; velocity.y = initialVelocityY; velocity.z = initialVelocityZ;
    }
}

public class VelocitySpeedModEffectOverride : EffectOverride
{
    private ParticleSystem.MinMaxCurve velocitySpeed;
    public VelocitySpeedModEffectOverride(rangePair speedMod)
    {
        velocitySpeed.constantMin = speedMod.min;
        velocitySpeed.constantMax = speedMod.max;
    }
    public override void ApplyOverride(ParticleSystem particleSystem)
    {
        ParticleSystem.VelocityOverLifetimeModule velocity = particleSystem.velocityOverLifetime;
        velocity.speedModifier = velocitySpeed;
    }
} 

// VelocityLimit Module Properties --------------------------------------------

public class VelocityDampeningEffectOverride : EffectOverride
{
    private float velocityDampening;
    public VelocityDampeningEffectOverride(float dampen)
    {
        velocityDampening = dampen;
    }
    public override void ApplyOverride(ParticleSystem particleSystem)
    {
        ParticleSystem.LimitVelocityOverLifetimeModule limit = particleSystem.limitVelocityOverLifetime;
        limit.dampen = velocityDampening;
    }
}
