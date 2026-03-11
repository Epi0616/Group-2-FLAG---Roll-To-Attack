using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);
        enemy.animator.SetBool("isAttacking",true);
        enemy.Attack();
    }

    public override void ExitState()
    {
        enemy.animator.SetBool("isAttacking", false);
        enemy.CompleteAttack();
    }
}
