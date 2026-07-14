using UnityEngine;
using UnityEngine.AI;

public class AIDrivenEntity : Entity , INavAgent , IUsesRigidBody
{
    // INavAgent Interface Properties
    [SerializeField] private NavMeshAgent Agent;
    public NavMeshAgent agent {  get => Agent; set => Agent = value; }
    public bool isAIDisabled { get; set; }

    // IUsesRigidBody Interface Properties
    [SerializeField] private Rigidbody Rb;
    public Rigidbody rb { get => Rb; set => Rb = value; }

    public virtual void EnableAIAgent()
    {
        if (!isAIDisabled) { return; }


        //Debug.Log("endabling agent");
        rb.isKinematic = false;
        rb.linearDamping = 0f;

        rb.useGravity = false;
        rb.isKinematic = true;

        isAIDisabled = false;

        //agent.enabled = true;      
        agent.updatePosition = true;
        agent.updateRotation = true;

        //agent.Warp(transform.position);
        agent.ResetPath();
        
    }

    public virtual void DisableAIAgent()
    {
        if (isAIDisabled) { return; }

        //Debug.Log("disabling agent");
        //agent.enabled = false;
        agent.updatePosition = false;
        agent.updateRotation = false;

        isAIDisabled = true;

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.linearDamping = 3f;
    }
}
