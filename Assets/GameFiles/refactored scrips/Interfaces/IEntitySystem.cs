using UnityEngine;

public interface IEntitySystem
{
    public Entity ownerEntity { get; set; }
    public void InitialiseSystem(Entity entity);
    public void ResetSystem();
}
