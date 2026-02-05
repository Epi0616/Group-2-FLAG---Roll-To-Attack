using UnityEngine;

public interface StateInterface
{
    public void EnterState(PlayerStateController player);
    public void UpdateState();
    public void FixedUpdateState();
}
