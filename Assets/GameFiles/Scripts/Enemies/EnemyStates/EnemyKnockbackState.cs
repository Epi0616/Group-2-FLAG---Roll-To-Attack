using UnityEngine;

public class EnemyKnockbackState : EnemyBaseState
{
    private float force;

    public EnemyKnockbackState(EnemyBaseClass enemy, float force) : base(enemy)
    {
        this.force = force;
    }

    public override void EnterState()
    {
        enemy.canMove = false;
        enemy.OnTakeKnockback(force);
    }

    public override void UpdateState()
    {
        enemy.rb.linearVelocity = Vector3.Lerp(enemy.rb.linearVelocity, Vector3.zero, 1.1f * Time.deltaTime);
        if (enemy.rb.linearVelocity.magnitude <= 2f)
        {
            enemy.canMove = true;
            enemy.RequestStateChange(new EnemyMoveState(enemy));
        }
    }

    public override void ExitState()
    {
        enemy.canMove = true;
    }
}
