using UnityEngine;

public class EnemyStunnedState : EnemyBaseState
{
    private float duration;

    public EnemyStunnedState(EnemyBaseClass enemy, float duration) : base(enemy)
    {
        this.duration = duration;
    }

    public override void EnterState()
    {
        enemy.canMove = false;
    }

    public override void UpdateState()
    {
        enemy.rb.linearVelocity = Vector3.zero;
        duration -= Time.deltaTime;
        if (duration < 0)
        {
            enemy.RequestStateChange(new EnemyMoveState(enemy));
        }
    }

    public override void ExitState()
    {
        enemy.canMove = true;
    }
}
