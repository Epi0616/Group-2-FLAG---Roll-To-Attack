using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class SlimeJump : BaseEntityAction
{
    private Coroutine actionRoutine = null;
    private ISlimeTrail slimeTrail;
    private IGrounded grounded;
    private IUsesRigidBody usesRigidBody;
    private INavAgent navAgent;
    private ISlimeSplit slimeSplit;

    public SlimeJump() { }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);

        if (!(ownerEntity is INavAgent navAgent)) return;
        this.navAgent = navAgent;

        if (!(ownerEntity is ISlimeTrail slimeTrail)) return;
        this.slimeTrail = slimeTrail;

        if (!(ownerEntity is IUsesRigidBody usesRigidBody)) return;
        this.usesRigidBody = usesRigidBody;

        if (!(ownerEntity is IGrounded grounded)) return;
        this.grounded = grounded;

        if (!(ownerEntity is ISlimeSplit slimeSplit)) return;
        this.slimeSplit = slimeSplit;

        actionRoutine = ownerEntity.StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        yield return Jump();

        yield return new WaitForSeconds(1);

        actionRoutine = null;
        EndAction();
    }

    private IEnumerator Jump()
    {
        Vector3 startSquish = ownerEntity.bodySystem.body.transform.localScale;
        Vector3 downSquish = new Vector3(startSquish.x * 1.2f, startSquish.y * 0.8f, startSquish.z * 1.2f);
        Vector3 downEndSquish = new Vector3(startSquish.x * 1.1f, startSquish.y * 0.9f, startSquish.z * 1.1f);
        Vector3 upSquish = new Vector3(startSquish.x * 0.8f, startSquish.y * 1.1f, startSquish.z * 0.8f);

        ActiveStatusEffect displacing = new ActiveStatusEffect(new SlimeDisplacement(),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true);
        ownerEntity.OnRecieveEffect(displacing);

        Rigidbody rb = usesRigidBody.rb;

        Vector3 force = new Vector3(0, 20, 0);


        yield return Squish(0.5f, startSquish, downSquish);

        Vector3 currentVelocity = rb.linearVelocity;
        currentVelocity.y = 0;
        rb.linearVelocity = currentVelocity;

        rb.AddForce(force, ForceMode.VelocityChange);

        yield return Squish(0.25f, downSquish, upSquish);

        //yield return new WaitForSeconds(0.5f);
        while (!grounded.isGrounded)
        {
            yield return null;
        }

        HandleImpact();
        yield return Squish(0.2f, upSquish, downEndSquish);
        yield return Squish(0.2f, downEndSquish, startSquish);
    }

    private IEnumerator Squish(float duration, Vector3 startSquish, Vector3 targetSquish)
    {
        Vector3 start = startSquish;
        Vector3 end = targetSquish;

        float timer = duration;
        float t = 0;
        while (t < 1)
        { 
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;

            ownerEntity.bodySystem.body.transform.localScale = Vector3.Lerp(start, end, t);
            yield return null;
        }

        ownerEntity.bodySystem.body.transform.localScale = end;
    }

    private void HandleImpact()
    {
        Collider[] colliders = Physics.OverlapSphere(ownerEntity.transform.position, 20 * slimeSplit.scale, slimeTrail.slimeableMask);

        foreach (var collider in colliders)
        {
            if (!collider.gameObject) { continue; }
            if (collider.gameObject == ownerEntity.gameObject) { continue; }
            if (collider.TryGetComponent<Entity>(out Entity entity))
            {
                ActiveStatusEffect knockBack = new ActiveStatusEffect(new SlimeKnockback(ownerEntity.transform.position, 5f * slimeSplit.scale),
                new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
                true);
                entity.OnRecieveEffect(knockBack);
            }
        }
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
        isComplete = true;
    }

    public override BaseEntityAction Clone()
    {
        return new SlimeJump();
    }
}
