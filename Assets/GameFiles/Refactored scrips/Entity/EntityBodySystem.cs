using UnityEngine;

public class EntityBodySystem : MonoBehaviour, IEntitySystem
{
    public Entity OwnerEntity { get; set; }
    public GameObject body;

    public void InitialiseSystem(Entity entity)
    {
        OwnerEntity = entity;
    }

    public void Vibrate()
    { 
    
    }

    public void ResetSystem()
    {
        // Reset body system state if needed
    }
}
