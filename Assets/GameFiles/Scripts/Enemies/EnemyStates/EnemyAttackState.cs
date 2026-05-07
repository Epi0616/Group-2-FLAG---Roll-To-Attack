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

        enemy.enemyAgent.SetDestination(enemy.transform.position);
        enemy.enemyAgent.isStopped = true;
        enemy.Attack();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (!enemy.canAttack)
        {
            enemy.ChangeState(new EnemyMoveState());
        }
    }

    public override void ExitState()
    {
        if (enemy.animator != null)
        {
            enemy.animator.SetBool("isAttacking", false);
        }
        enemy.CompleteAttack();
        enemy.enemyAgent.isStopped = false;
    }
}
