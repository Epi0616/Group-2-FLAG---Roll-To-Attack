using System;
using UnityEngine;


[Serializable]
public class NavMeshMovement : BaseEntityMovement
{
    private INavAgent aiInterfaceAccess;
    public Vector3 targetPos;
    private float setDestinationInterval = 0.15f;
    private float intervalTimer = 0;

    public NavMeshMovement() { }

    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);
        aiInterfaceAccess = ownerEntity as INavAgent;
        aiInterfaceAccess.EnableAIAgent();
    }

    public override void UpdateMovement()
    {        
        if (aiInterfaceAccess.agent == null) { Debug.LogError("NO AGENT LOL"); }

        targetPos = ownerEntity.target.transform.position;

        intervalTimer += Time.deltaTime;
        if (intervalTimer > setDestinationInterval)
        {
            
            aiInterfaceAccess.agent.SetDestination(targetPos);
            intervalTimer = 0;
        }
    }

    public override void InterruptMovement()
    {
        
    }

    public override void EndMovement()
    {
        aiInterfaceAccess.agent.SetDestination(ownerEntity.transform.position);
    }

}
