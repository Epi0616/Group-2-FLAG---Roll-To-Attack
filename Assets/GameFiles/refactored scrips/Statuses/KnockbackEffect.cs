using UnityEngine;
using UnityEngine.AI;

public class KnockbackEffect : BaseDisplacementEffect
{
    Vector3 origin;
    float force;
    
    public KnockbackEffect(Vector3 origin, float force)
    {
        this.origin = origin;
        this.force = force;
        type = StatusType.Knockback;
        preventsMovement = true;
        preventsAction = true;
        isDisplacing = true;
        isStackable = true;
    }

    protected override void OnApplication()
    {
        base.OnApplication();             

        if (!isActive) { return; }

        rbInterfaceAccess.rb.linearVelocity = Vector3.zero;
        Vector3 targetVector = (entityRef.transform.position - origin);

        Vector3 targetDirection = targetVector.normalized;
        targetDirection.y = 0.3f;
        if (knockbackInterfaceAccess != null && knockbackInterfaceAccess.knockbackWeightMod != null)
        {
            rbInterfaceAccess.rb.AddForce(targetDirection * ((force * knockbackInterfaceAccess.knockbackWeightMod.GetFinalValue()) * 10f), ForceMode.VelocityChange);
        }
        else
        {
            rbInterfaceAccess.rb.AddForce(targetDirection * (force * 10f), ForceMode.VelocityChange);
        }
        
    } 

    protected override void OnRemoval()
    {
        //if (entityRef is INavAgent navAgent)
        //{
        //    NavMesh.SamplePosition(entityRef.transform.position, out NavMeshHit hit, 10, 1);
        //    if (hit.position.magnitude < 1000000)
        //    {
        //        navAgent.agent.Warp(hit.position);
        //    }

        //}


        base.OnRemoval();
        //Debug.Log("KB Removed");
    }

    public override StatusEffect Clone()
    {
        return new KnockbackEffect(origin, force);
    }
}


