using UnityEngine;

public class ConditionalMovementDescriptor : ScriptableObject
{
    public ConditionalMovement conditionalMovement;

    public ConditionalMovement Create()
    {
        return conditionalMovement.Clone();
    }
}
