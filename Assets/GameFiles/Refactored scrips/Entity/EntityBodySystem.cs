using UnityEngine;

public class EntityBodySystem : MonoBehaviour, IEntitySystem
{
    public Entity OwnerEntity { get; set; }
    public GameObject body;
    public Quaternion originalRotation;

    public virtual void InitialiseSystem(Entity entity)
    {
        OwnerEntity = entity;
        originalRotation = body.transform.rotation;
    }

    public virtual void Vibrate()
    { 
    
    }

    public virtual void Wobble(float magnitude)
    {
        float x = Mathf.Sin(Time.time * 50f) * magnitude;
        float y = Mathf.Sin(Time.time * 50f) * magnitude;
        float z = Mathf.Sin(Time.time * 50f) * magnitude;
        body.transform.rotation = originalRotation * Quaternion.Euler(x, y, z);
    }

    public virtual void ResetSystem()
    {
        // Reset body system state if needed
    }
}
