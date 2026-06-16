using UnityEngine;
using UnityEngine.AI;
using System;

[Serializable]
public class OnNavMeshCondition : BaseCondition
{
    private Entity ownerEntity;  
    private INavAgent aiInterfaceAccess;
    private IUsesRigidBody rbInterfaceAccess;

    private bool isActive = false;

    public OnNavMeshCondition() { }

    public OnNavMeshCondition(bool inverse, bool active)
    {
        this.inverse = inverse;
        isActive = active;
    }
    public override void Initialize(Entity entity)
    {
        ownerEntity = entity;
        aiInterfaceAccess = ownerEntity as INavAgent;
        rbInterfaceAccess = ownerEntity as IUsesRigidBody;
    }
    public override void ConditionUpdate()
    {
        if (!isActive) { return; }

        if (rbInterfaceAccess.rb.linearVelocity.magnitude <= 2f)
        {
            NavMeshHit hit;
            bool validNavMeshNode = NavMesh.SamplePosition(ownerEntity.transform.position, out hit, 5f, NavMesh.AllAreas);
            if (validNavMeshNode)
            {
                Vector3 destinationPos = new Vector3(hit.position.x, hit.position.y, hit.position.z);
                Vector3 returntoNavMeshDirection = (destinationPos - ownerEntity.transform.position).normalized;
                //returntoNavMeshDirection.y = enemy.transform.position.y;
                rbInterfaceAccess.rb.MovePosition(ownerEntity.transform.position + returntoNavMeshDirection * 10f * Time.deltaTime);
            
            }
        }
    }

    public override void ResetCondition() { }

    public override bool IsConditionMet()
    {
        if (aiInterfaceAccess == null)
        {
            Debug.LogWarning("Nav Agent Missing");
            return false;
        }
        //Debug.Log(aiInterfaceAccess.agent.isOnNavMesh);
        if (inverse) { return !aiInterfaceAccess.agent.isOnNavMesh; }
        return aiInterfaceAccess.agent.isOnNavMesh;
    }

    public override BaseCondition Clone()
    {
        return new OnNavMeshCondition(inverse, isActive);
    }
}
