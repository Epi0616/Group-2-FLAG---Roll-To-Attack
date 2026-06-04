using UnityEngine;
using UnityEngine.AI;

public class OnNavMeshCondition : BaseCondition
{
    private Entity ownerEntity;
    private float distance = 100.0f;
    private INavAgent aiInterfaceAccess;
    private IUsesRigidBody rbInterfaceAccess;

    private bool isActive = false;

    public OnNavMeshCondition() { }

    public OnNavMeshCondition(bool required, Entity entity, bool active)
    {
        ownerEntity = entity;
        isRequired = required;
        name = "NavReturnCondition";
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

                distance = Vector3.Distance(ownerEntity.transform.position, destinationPos);

            }
        }
    }

    public override void ResetCondition() { }

    public override bool IsConditionMet()
    {
        if (aiInterfaceAccess == null) return false;

        NavMeshHit hit;
        bool validNavMeshNode = NavMesh.SamplePosition(ownerEntity.transform.position, out hit, 5f, NavMesh.AllAreas);
        if (validNavMeshNode)
        {
            distance = Vector3.Distance(ownerEntity.transform.position, hit.position);
        }

        return (distance < 0.2f);
    }
}
