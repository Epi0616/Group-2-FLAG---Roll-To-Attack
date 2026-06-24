using UnityEngine;

public interface IEntitySystem
{
    public Entity OwnerEntity { get; set; }
    public void InitialiseSystem(Entity entity);
    public void ResetSystem();
}
