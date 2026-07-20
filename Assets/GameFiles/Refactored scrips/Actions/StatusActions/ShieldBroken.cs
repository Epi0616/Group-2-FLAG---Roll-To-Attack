using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

[Serializable]
public class ShieldBroken : BaseEntityAction
{
    [SerializeField] private int shieldStacks;
    [SerializeField] private float stunnedTime;
    [SerializeField] private float applyTime;

    private IAnimated animated;
    private Coroutine applyShieldsRoutine;

    public ShieldBroken() { }
    public ShieldBroken(bool preventsMovement, int shieldStacks, float stunnedTime, float applyTime)
    {
        this.preventsMovement = preventsMovement;
        this.shieldStacks = shieldStacks;
        this.stunnedTime = stunnedTime;
        this.applyTime = applyTime;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);
        if (ownerEntity is IAnimated animated)
        {
            this.animated = animated;
        }

        ownerEntity.StartCoroutine(ShieldsBrokenRoutine());
    }

    private IEnumerator ShieldsBrokenRoutine()
    {
        yield return ownerEntity.StartCoroutine(Stunned());

        applyShieldsRoutine = ownerEntity.StartCoroutine(ApplyShields());
        yield return applyShieldsRoutine;

        EndAction();
    }

    private IEnumerator Stunned()
    {
        if (ownerEntity is IAnimated animated)
        {
            animated.animationManager.PlayAnimationCrossFade(AnimationType.OnStunned, 1, MixerType.main, 0.2f, stunnedTime);
        }
        yield return new WaitForSeconds(stunnedTime);
    }

    private IEnumerator ApplyShields()
    {
        float timeFith = applyTime / 5f;

        yield return new WaitForSeconds(3*timeFith);

        if (animated != null)
        {
            animated.animationManager.PlayAnimationCrossFade(AnimationType.StunnedOver, 1, MixerType.main, 0.2f, 2 * timeFith);
        }


        yield return new WaitForSeconds(2*timeFith);

        ActiveStatusEffect shieldEffect = new(new ShieldedStatus(shieldStacks), new List<BaseCondition>() { new AlwaysTrueCondition(true) }, false);
        ownerEntity.statusSystem.OnRecieveEffect(shieldEffect);

        if (animated != null)
        {
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Waddle, 1, MixerType.main, 1);
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Scream, 1, MixerType.complimentary, 2);
        }

        yield return new WaitForSeconds(2);
        EndAction();
    }

    public override void InterruptAction()
    {
        //if (applyShieldsRoutine != null)
        //{
        //    ownerEntity.StopCoroutine(applyShieldsRoutine);
        //}

        EndAction();
    }
    public override void EndAction()
    {
        isComplete = true;
    }
    public override BaseEntityAction Clone()
    {
        return new ShieldBroken(preventsMovement, shieldStacks, stunnedTime, applyTime);
    }
}
