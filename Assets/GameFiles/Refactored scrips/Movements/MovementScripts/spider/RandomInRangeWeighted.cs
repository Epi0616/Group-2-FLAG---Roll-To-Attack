using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RandomInRangeWeighted : RandomInRangeNoDelayWeighted
{
    [SerializeField] private float delayMin, delayMax;

    private float timer = 0;
    private bool targetReached = false;

    public RandomInRangeWeighted() : base() { }
    public RandomInRangeWeighted(float rangeMin, float rangeMax, float angleVariance, float delayMin, float delayMax, float chancePercentForTargettedMovement) : base(rangeMin, rangeMax, angleVariance, chancePercentForTargettedMovement) 
    {
        this.rangeMin = rangeMin;
        this.rangeMax = rangeMax;
        this.angleVariance = angleVariance;
        this.delayMin = delayMin;
        this.delayMax = delayMax;
    }

    public override void UpdateMovement()
    {
        if (navAgent.agent.pathPending) return;
        if (navAgent.agent.remainingDistance > 0.5f) return;
        PauseOnTargetReached();
        if (navAgent.agent.remainingDistance > navAgent.agent.stoppingDistance) return;
        if (navAgent.agent.velocity.sqrMagnitude > 0) return;

        timer -= Time.deltaTime;
        if (timer > 0)
        {
            return;
        }

        animated.animationManager.PlayAnimationCrossFade(AnimationType.Waddle, 2, MixerType.main, 0.2f, 1.5f);
        PickDestination();
        SetTimer();
        targetReached = false;
    }

    private void PauseOnTargetReached()
    {
        if (targetReached) return;

        animated.animationManager.PlayAnimationCrossFade(AnimationType.Idle, 2, MixerType.main);
        targetReached = true;
    }

    private void SetTimer()
    { 
        timer = Random.Range(delayMin, delayMax);
    }

    public override BaseEntityMovement Clone()
    {
        return new RandomInRangeWeighted(rangeMin, rangeMax, angleVariance, delayMin, delayMax, chancePercentForTargettedMovement);
    }
}
