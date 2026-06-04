using UnityEngine;
using UnityEngine.AI;

public interface INavAgent
{
    public NavMeshAgent agent { get; set; }
    public bool isAIDisabled { get; set; }

    public void EnableAIAgent();
    public void DisableAIAgent();
}
