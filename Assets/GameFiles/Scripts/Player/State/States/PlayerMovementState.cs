using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementState : PlayerBaseState
{
    protected Vector3 moveDirection;
    protected float holdTime = 0;

    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);
    }
    public override void UpdateState()
    {
        CheckForMoveActionPressed();
    }
    public override void FixedUpdateState()
    {
        //Vector3 targetPosition = player.rb.position + moveDirection * Time.fixedDeltaTime * player.moveSpeed;
        //targetPosition.y = player.rb.position.y;
        //player.rb.MovePosition(targetPosition);

        Vector3 targetVelocity = moveDirection * player.moveSpeed.GetFinalValue();
        targetVelocity.y = player.rb.linearVelocity.y;
        player.rb.linearVelocity = targetVelocity;
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
}
