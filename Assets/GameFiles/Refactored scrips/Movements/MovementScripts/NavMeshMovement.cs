using System;
using UnityEngine;

[Serializable]
public class NavMeshMovement : BaseEntityMovement
{
    [SerializeField] private float movementAnimationWindow = 0;

    private INavAgent aiInterfaceAccess;
    private IAnimated animated;
    private EnemyBodySystem enemyBodySystem;
    private float setDestinationInterval = 0.15f;
    private float intervalTimer = 0;
    public NavMeshMovement() { }

    public NavMeshMovement(float movementAnimationSpeed)
    {
        this.movementAnimationWindow = movementAnimationSpeed;
    }

    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);
        aiInterfaceAccess = ownerEntity as INavAgent;
        enemyBodySystem = ownerEntity.bodySystem as EnemyBodySystem;
        aiInterfaceAccess.EnableAIAgent();
        aiInterfaceAccess.agent.updateRotation = false;

        if (ownerEntity is not IAnimated animated) { Debug.LogError("owner entity is not of type IAnimated"); return; }
        this.animated = animated;

        animated.animationManager.PlayAnimationCrossFade(AnimationType.Waddle, 2, MixerType.main, 0.2f, movementAnimationWindow);
    }

    public override void UpdateMovement()
    {        
        if (ownerEntity == null) return;
        if (aiInterfaceAccess.agent == null) { Debug.LogError("NO AGENT LOL"); }

        if (moveable.canMove == false) { EndMovement(); return; }

        intervalTimer += Time.deltaTime;
        if (intervalTimer > setDestinationInterval)
        {
            aiInterfaceAccess.agent.SetDestination(ownerEntity.target.transform.position);
            intervalTimer = 0;
        }
    }

    public override void InterruptMovement()
    {
        EndMovement();
    }

    public override void EndMovement()
    {
       // Debug.Log("Movement Ended");
       aiInterfaceAccess.agent.SetDestination(ownerEntity.transform.position);
    }

    public override BaseEntityMovement Clone()
    {
        return new NavMeshMovement(movementAnimationWindow);
    }
}
