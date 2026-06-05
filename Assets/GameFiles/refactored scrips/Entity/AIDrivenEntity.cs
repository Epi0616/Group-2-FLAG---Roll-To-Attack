using UnityEngine;
using UnityEngine.AI;

public class AIDrivenEntity : Entity , INavAgent
{
    // INavAgent Interface Properties
    public NavMeshAgent agent {  get; set; }
    public bool isAIDisabled { get; set; }

    public virtual void EnableAIAgent()
    {
        if (!isAIDisabled) { return; }

        isAIDisabled = false;

        //agent.enabled = true;      
        agent.updatePosition = true;
        agent.updateRotation = true;

        agent.Warp(transform.position);
        agent.ResetPath();
        
    }

    public virtual void DisableAIAgent()
    {
        if (isAIDisabled) { return; }

        //agent.enabled = false;
        agent.updatePosition = false;
        agent.updateRotation = false;

        isAIDisabled = true;

    }
}
