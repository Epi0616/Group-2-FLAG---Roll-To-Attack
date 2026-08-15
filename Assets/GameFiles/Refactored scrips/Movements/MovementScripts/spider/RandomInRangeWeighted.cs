using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RandomInRangeWeighted : RandomInRangeNoDelayWeighted
{
    [SerializeField] private float delayMin, delayMax;

    private float timer = 0;

    public RandomInRangeWeighted() : base() { }
    public RandomInRangeWeighted(float rangeMin, float rangeMax, float angleVariance, float delayMin, float delayMax) : base(rangeMin, rangeMax, angleVariance) 
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
        if (navAgent.agent.remainingDistance > navAgent.agent.stoppingDistance) return;
        if (navAgent.agent.velocity.sqrMagnitude > 0) return;

        timer -= Time.deltaTime;
        if (timer > 0)
        {
            return;
        }

        PickDestination();
        SetTimer();
    }

    private void SetTimer()
    { 
        timer = Random.Range(delayMin, delayMax);
    }

    public override BaseEntityMovement Clone()
    {
        return new RandomInRangeWeighted(rangeMin, rangeMax, angleVariance, delayMin, delayMax);
    }
}
