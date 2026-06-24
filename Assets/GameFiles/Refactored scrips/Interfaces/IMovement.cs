using UnityEngine;

public interface IMovement
{
    public void StartMovement(Entity ownerEntity);
    public void UpdateMovement();
    public void FixedUpdateMovement();
    public void InterruptMovement();
    public void EndMovement();
}
