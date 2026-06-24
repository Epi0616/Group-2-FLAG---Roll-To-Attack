using Unity.VisualScripting;
using UnityEngine;

public interface IEntityBaseMovementState
{
    public void EnterState();

    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
}
