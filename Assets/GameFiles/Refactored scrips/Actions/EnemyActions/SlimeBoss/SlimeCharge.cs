using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class SlimeCharge : BaseEntityAction
{
    private IUsesRigidBody usesRigidBody;
    private ISlimeTrail slimeTrail;
    private IGrounded grounded;
    private ISlimeSplit slimeSplit;
    private Coroutine actionRoutine = null;

    public SlimeCharge() { }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);

        if (!(ownerEntity is IUsesRigidBody usesRigidBody)) return;
        this.usesRigidBody = usesRigidBody;

        if (!(ownerEntity is ISlimeSplit slimeSplit)) return;
        this.slimeSplit = slimeSplit;

        if (!(ownerEntity is IGrounded grounded)) return;
        this.grounded = grounded;

        if (!(ownerEntity is ISlimeTrail slimeTrail)) return;
        this.slimeTrail = slimeTrail;

        //navAgent.DisableAIAgent();
        actionRoutine = ownerEntity.StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        Rigidbody rb = usesRigidBody.rb;

        yield return Vibrate(1f, 0.3f);

        slimeTrail.isCharging = true;
        ChargeTowardsTarget();

        yield return new WaitForSeconds(1.5f);

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

        force *= 100;
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
        return new SlimeCharge();
    }
}
