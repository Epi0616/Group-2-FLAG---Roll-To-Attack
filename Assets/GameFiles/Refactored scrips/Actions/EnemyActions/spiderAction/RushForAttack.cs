using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class RushForAttack : BaseSlamAction
{
    private INavAgent navAgent;
    private Coroutine actionRoutine;

    public RushForAttack() : base() { }
    public RushForAttack(int slamDamage, float chargeTime, float slamRange, Vector3 slamPositionOffset, Color slamColour, bool preventsMovement) : base(slamDamage, chargeTime,slamRange, slamPositionOffset, slamColour, preventsMovement)
    { 
        this.slamDamage = slamDamage;
        this.chargeTime = chargeTime;
        this.slamRange = new Stat(slamRange);
        this.slamPositionOffset = slamPositionOffset;
        this.slamColour = slamColour;
        this.preventsMovement = preventsMovement;
    }

    public override void StartAction(Entity ownerEntity)
    {
        this.ownerEntity = ownerEntity;

        //Debug.Log("starting rush attack");

        if (!(ownerEntity is INavAgent navAgent)) { Debug.LogError("owner entity is not of type INavAgent"); return; }
        this.navAgent = navAgent;
        navAgent.EnableAIAgent();

        ActiveStatusEffect speedIncreaseEffect = new(new MovementSpeedStatus(5f), new List<BaseCondition> { new TimeCondition(true, 1) }, true);
        ownerEntity.statusSystem.OnRecieveEffect(speedIncreaseEffect);

        actionRoutine = ownerEntity.StartCoroutine(Action());
    }

    private IEnumerator Action()
    {
        float timer2 = 0.75f;

        while (timer2 > 0)
        {
            timer2 -= Time.deltaTime;
            SetDestinationToTarget();

            while (navAgent.agent.pathPending)
            { 
                yield return null;
            }

            if (navAgent.agent.remainingDistance < slamRange.GetFinalValue() * 2)
            {
                yield return HandleInRange();
                yield break;
            }
            yield return null;
        }

        while (navAgent.agent.remainingDistance > slamRange.GetFinalValue()*2)
        { 
            yield return null;
        }
        yield return HandleInRange();
    }

    private IEnumerator HandleInRange()
    {
        Vector3 originalTarget = navAgent.agent.destination;
        navAgent.agent.destination = navAgent.agent.transform.position;

        yield return FaceTargetPoint(originalTarget);
        yield return Attack();

        yield return new WaitForSeconds(1);
        actionRoutine = null;
        EndAction();
    }

    private IEnumerator Attack()
    {
        SetupSlam();
        AnimateAttack();

        while (chargeUpTimer < chargeTime)
        {
            chargeUpTimer += Time.deltaTime;
            yield return null; 
        }

        chargeComplete = true;
        SpawnSlamCompleteVFX();
        ExtraSlamEffect();
        if (slamRange.GetFinalValue() > slamRange.GetBaseValue()) //potential rework if we buff range in some way??
        {
            ApplyExtraHeavyEffect();
        }
        // Debug.Log("SLAMMING");

        if (slamRange.GetFinalValue() > slamRange.GetBaseValue())
        {
            triggerPillars();
        }
        Slam();
    }

    public override void ProcessHits(Collider[] colliders, RaycastHit hit)
    {
        foreach (var collider in colliders)
        {
            if (attackInterrupted) { break; }
            if (collider == null) continue;
            if (collider.gameObject == ownerEntity.gameObject) { continue; }
            if (collider.gameObject.CompareTag("StaticEntity")) { continue; }
            Entity hitEntity = collider.gameObject.GetComponent<Entity>();
            if (hitEntity == null) { continue; }
            ApplyCustomEffectPerEntity(hitEntity);
            if (slamRange.GetFinalValue() > slamRange.GetBaseValue()) //potential rework if we buff range in some way??
            {
                ApplyHeavyEffectPerEntity(hitEntity);
            }
        }
    }

    public override void ApplyCustomEffectPerEntity(Entity hitEntity)
    {   
        base.ApplyCustomEffectPerEntity(hitEntity);

        hitEntity.OnRecieveEffect(new ActiveStatusEffect(new SlowStatus(0.75f, "PlaceHolderSlow"),
        new List<BaseCondition> { new TimeCondition(true, 5f) }, true));
    }

    public override IEnumerator slamCD(float amount) { yield break; }

    private void SetDestinationToTarget()
    {
        navAgent.agent.SetDestination(ownerEntity.target.transform.position);

        //if (NavMesh.SamplePosition(ownerEntity.target.transform.position, out NavMeshHit hit, 5, NavMesh.AllAreas))
        //{
        //    //Vector3 directionToTarget = (hit.position - navAgent.agent.transform.position).normalized;
        //    //Vector3 targetPos = hit.position - (directionToTarget * SlamRange.GetFinalValue() * 2);

        //    navAgent.agent.SetDestination(hit.position);
        //    return true;
        //}

        //return false;
    }

    private IEnumerator FaceTargetPoint(Vector3 targetPoint)
    {
        Vector3 targetDirection = targetPoint - ownerEntity.transform.position;
        targetDirection.y = 0f;
        if (targetDirection == Vector3.zero) { yield break; }

        navAgent.agent.updateRotation = false;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);

        while (ownerEntity.transform.rotation != lookRotation)
        {
            ownerEntity.transform.rotation = Quaternion.RotateTowards(ownerEntity.transform.rotation, lookRotation, navAgent.agent.angularSpeed * Time.deltaTime);
            yield return null;
        }

        navAgent.agent.updateRotation = true;
    }

    public override void UpdateAction() { }

    public override void InterruptAction()
    {
        if (actionRoutine != null)
        { 
            ownerEntity.StopCoroutine(actionRoutine);
            actionRoutine = null;
        }

        if (impactField != null)
        {
            impactField.DestroyMe();
        }

        navAgent.agent.updateRotation = true;// might be breaking things aka rotation, why is this controlling whether the navagent ccan rotate lol
        EndAction();
    }
    public override void EndAction()
    {
        isComplete = true;
    }
    public override BaseEntityAction Clone()
    {
        return new RushForAttack(slamDamage, chargeTime, slamRange.GetFinalValue(), slamPositionOffset, slamColour, preventsMovement);
    }
}
