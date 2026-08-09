using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public class PanicEscapeAction : BaseEntityAction
{
    private INavAgent aiInterfaceAccess;
    private IAnimated animated;
    private float initialSpeed = 0f;

    private float interval = 0.15f;
    float timer = 0f;
    private Vector3 destination;

    public PanicEscapeAction() { }
    public PanicEscapeAction(bool preventsMovement)
    { 
        this.preventsMovement = preventsMovement;
    }

    public override void StartAction(Entity ownerEntity)
    {
        Debug.Log("starting panic escape action");
        base.StartAction(ownerEntity);
        aiInterfaceAccess = ownerEntity as INavAgent;
        aiInterfaceAccess.EnableAIAgent();
        aiInterfaceAccess.agent.updateRotation = false;

        if (ownerEntity is IAnimated animated)
        {
            this.animated = animated;
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Waddle, 2, MixerType.main);
        }

        ActiveStatusEffect speedIncreaseEffect = new(new MovementSpeedStatus(1.5f), new List<BaseCondition> { new DistanceCondition(true, 30) }, true);
        ownerEntity.statusSystem.OnRecieveEffect(speedIncreaseEffect);

        destination = FindDestinationAwayFromTarget(50);
        aiInterfaceAccess.agent.destination = destination;
    }

    private Vector3 FindDestinationAwayFromTarget(float radius)
    {
        float x = Random.Range(-1, 1);
        float y = Random.Range(-1, 1);
        float z = Random.Range(-1, 1);

        x = (x < 0) ? Mathf.Clamp(x, -1, -0.2f) : Mathf.Clamp(x, 0.2f, 1);
        y = (y < 0) ? Mathf.Clamp(y, -1, -0.2f) : Mathf.Clamp(y, 0.2f, 1);
        x = (z < 0) ? Mathf.Clamp(z, -1, -0.2f) : Mathf.Clamp(z, 0.2f, 1);

        Vector3 randomPointAwayFromOwner = new Vector3(x, y, z) * radius + ownerEntity.transform.position;

        Vector3 targetPos = Vector3.zero;
        if (NavMesh.SamplePosition(randomPointAwayFromOwner, out NavMeshHit hit, radius, 0))
        {
            targetPos = hit.position;
        }

        return targetPos;
    }

    public override void UpdateAction()
    {
        timer += Time.deltaTime;

        if ((ownerEntity.transform.position - aiInterfaceAccess.agent.destination).magnitude < 2)
        {
            EndAction();
        }

        if (timer >= interval)
        {
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Waddle, 2, MixerType.main);
            aiInterfaceAccess.agent.destination = destination;
            timer = 0;
        }
    }

    public override void InterruptAction()
    {
        EndAction();
    }

    public override void EndAction()
    {
        // Debug.Log("Movement Ended");
        aiInterfaceAccess.agent.speed = initialSpeed;
        aiInterfaceAccess.agent.SetDestination(ownerEntity.transform.position);
    }
    public override BaseEntityAction Clone()
    {
        return new PanicEscapeAction(preventsMovement);
    }
}
