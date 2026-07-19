using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShieldDestroyed : BaseEntityAction
{
    private IShieldable shieldable;
    [SerializeField] private float animationTime;

    public ShieldDestroyed() { }
    public ShieldDestroyed(bool preventsMovement, float animationTime)
    {
        this.preventsMovement = preventsMovement;
        this.animationTime = animationTime;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);
        shieldable = ownerEntity as IShieldable;

        if (ownerEntity is IAnimated animated)
        {
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Stunned, 1, MixerType.main, 0.2f, animationTime);
        }
        ownerEntity.StartCoroutine(DelayEnd());
    }

    private IEnumerator DelayEnd()
    { 
        yield return new WaitForSeconds(animationTime);
        EndAction();
    }

    public override void InterruptAction()
    {
        EndAction();
    }
    public override void EndAction()
    {
        isComplete = true;
    }
    public override BaseEntityAction Clone()
    {
        return new ShieldDestroyed(preventsMovement, animationTime);
    }
}
