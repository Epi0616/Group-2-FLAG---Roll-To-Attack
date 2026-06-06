using UnityEngine;
using UnityEngine.AI;

public class AIDrivenEntity : Entity , INavAgent , IUsesRigidBody
{
    // INavAgent Interface Properties
    public NavMeshAgent agent {  get; set; }
    public bool isAIDisabled { get; set; }

    // IUsesRigidBody Interface Properties
    public Rigidbody rb { get; set; }

    public virtual void EnableAIAgent()
    {
        //if (!isAIDisabled) { return; }

        rb.isKinematic = false;
        rb.linearDamping = 0f;

        rb.useGravity = false;
        rb.isKinematic = true;

        isAIDisabled = false;

        //agent.enabled = true;      
        agent.updatePosition = true;
        agent.updateRotation = true;

        agent.Warp(transform.position);
        agent.ResetPath();
        
    }

    public virtual void DisableAIAgent()
    {
        //if (isAIDisabled) { return; }

        //agent.enabled = false;
        agent.updatePosition = false;
        agent.updateRotation = false;

        isAIDisabled = true;

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.linearDamping = 3f;

    }
}
