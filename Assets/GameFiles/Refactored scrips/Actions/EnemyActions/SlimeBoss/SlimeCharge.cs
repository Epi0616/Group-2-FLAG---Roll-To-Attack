using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class SlimeCharge : BaseEntityAction
{
    private IUsesRigidBody usesRigidBody;
    private ISlimeTrail slimeTrail;
    private IMoveable moveable;
    private IAnimated animated;
    private Coroutine actionRoutine = null;

    public SlimeCharge() { }
    public SlimeCharge(bool preventsMovement)
    { 
        this.preventsMovement = preventsMovement;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);

        if (!(ownerEntity is IUsesRigidBody usesRigidBody)) return;
        this.usesRigidBody = usesRigidBody;

        if (!(ownerEntity is IAnimated animated)) return;
        this.animated = animated;

        if (!(ownerEntity is IMoveable moveable)) return;
        this.moveable = moveable;

        if (!(ownerEntity is ISlimeTrail slimeTrail)) return;
        this.slimeTrail = slimeTrail;

        //navAgent.DisableAIAgent();
        actionRoutine = ownerEntity.StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        Rigidbody rb = usesRigidBody.rb;

        animated.animationManager.PlayAnimationCrossFade(AnimationType.Charge, 1, MixerType.main, 0.2f, 2f);
        yield return new WaitForSeconds(1.5f);

        slimeTrail.isCharging = true;
        ChargeTowardsTarget();

        yield return new WaitForSeconds(0.5f);
        animated.animationManager.PlayAnimationCrossFade(AnimationType.Waddle, 1, MixerType.main, 0.2f, 2f);

        yield return new WaitForSeconds(1f);

        actionRoutine = null;
        EndAction();
    }

    private IEnumerator Vibrate(float duration, float intensity)
    {
        while (duration > 0)
        { 
            duration -= Time.deltaTime;
            ownerEntity.bodySystem.Vibrate(intensity);
            yield return null;
        }
    }

    private void ChargeTowardsTarget()
    {
        Vector3 force = (ownerEntity.target.transform.position - ownerEntity.transform.position).normalized;
        force.y = 0;

        force *= 7.5f * moveable.movementSpeed.GetFinalValue();
        usesRigidBody.rb.AddForce(force, ForceMode.VelocityChange);
    }

    public override void InterruptAction()
    {
        if (actionRoutine != null)
        {
            ownerEntity.StopCoroutine(actionRoutine);
            actionRoutine = null;
        }

        EndAction();
    }

    public override void EndAction()
    {
        slimeTrail.isCharging = false;
        isComplete = true;
    }

    public override BaseEntityAction Clone()
    {
        return new SlimeCharge(preventsMovement);
    }
}
