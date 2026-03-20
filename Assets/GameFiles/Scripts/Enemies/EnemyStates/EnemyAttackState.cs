using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);
        if (enemy.animator != null)
        {
            enemy.animator.SetBool("isAttacking", true);
        }
        enemy.Attack();
    }

    public override void ExitState()
    {
        if (enemy.animator != null)
        {
            enemy.animator.SetBool("isAttacking", false);
        }
        enemy.CompleteAttack();
    }
}
