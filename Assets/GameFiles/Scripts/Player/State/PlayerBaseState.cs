using UnityEngine;

public abstract class PlayerBaseState : StateInterface
{
    protected PlayerStateController player;
    public virtual void EnterState(PlayerStateController player)
    {
        this.player = player;
    }
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
}
