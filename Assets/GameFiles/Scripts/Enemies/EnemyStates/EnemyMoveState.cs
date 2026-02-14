using UnityEngine;

public class EnemyMoveState : EnemyBaseState
{
    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector3 playerPosition = enemy.playerReference.transform.position;

        Vector3 targetVector = (playerPosition - enemy.transform.position);
        Vector3 targetDirection = targetVector.normalized;
        targetDirection.y = 0;
        if (targetVector.magnitude < enemy.attackRange)
        {
            enemy.rb.linearVelocity = Vector3.zero;
            return;
        }
        enemy.rb.linearVelocity = targetDirection * enemy.moveSpeed;
    }
}
