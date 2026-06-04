using UnityEngine;

public interface IMovement
{
    public void StartMovement(Entity ownerEntity);
    public void UpdateMovement();
    public void InterruptMovement();
    public void EndMovement();
}
