using UnityEngine;

public class EnemyMoveState : EnemyBaseState
{
    public EnemyMoveState(EnemyBaseClass enemy) : base(enemy) { }

    public override void FixedUpdateState()
    {
        enemy.MoveTowardsPlayer();
    }
}
