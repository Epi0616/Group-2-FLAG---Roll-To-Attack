using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public class PanicEscapeMovement : BaseEntityMovement
{
    private INavAgent aiInterfaceAccess;
    private float initialSpeed = 0f;

    public PanicEscapeMovement() { }

    public override void StartMovement(Entity ownerEntity)
    {
        Debug.Log("starting panic escape movement");
        base.StartMovement(ownerEntity);
        aiInterfaceAccess = ownerEntity as INavAgent;
        aiInterfaceAccess.EnableAIAgent();
        aiInterfaceAccess.agent.updateRotation = false;

        if (ownerEntity is IAnimated animated)
        {
            animated.animationManager.PlayAnimationCrossFade(AnimationType.Waddle, 2, MixerType.main);
        }

        ActiveStatusEffect speedIncreaseEffect = new(new MovementSpeedStatus(0.5f), new List<BaseCondition> { new DistanceCondition(true, 30) }, true);
        ownerEntity.statusSystem.OnRecieveEffect(speedIncreaseEffect);

        aiInterfaceAccess.agent.destination = FindDestinationAwayFromTarget(50);
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

    public override void UpdateMovement()
    {
    }

    public override void InterruptMovement()
    {
        EndMovement();
    }

    public override void EndMovement()
    {
        // Debug.Log("Movement Ended");
        aiInterfaceAccess.agent.speed = initialSpeed;
        aiInterfaceAccess.agent.SetDestination(ownerEntity.transform.position);
    }
    public override BaseEntityMovement Clone()
    {
        return new PanicEscapeMovement();
    }
}
