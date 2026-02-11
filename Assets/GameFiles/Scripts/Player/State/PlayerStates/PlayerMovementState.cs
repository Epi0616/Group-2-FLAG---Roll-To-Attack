using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementState : PlayerBaseState
{
    private Vector3 moveDirection;
    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);
    }
    public override void UpdateState()
    {
        CheckForMoveActionPressed();
        CheckForAttackActionPressed();
    }
    public override void FixedUpdateState()
    {
        player.rb.MovePosition(player.transform.position + moveDirection * Time.deltaTime * player.moveSpeed);
    }

    private void CheckForMoveActionPressed()
    {
        if (player.move.action.IsPressed())
        {
            moveDirection = player.move.action.ReadValue<Vector3>();
            return;
        }

        moveDirection = new(0, 0, 0);
    }

    private void CheckForAttackActionPressed()
    {
        if (player.attack.action.WasPressedThisFrame())
        {
            player.SwitchState(new PlayerJumpState());
        }
    }
}
