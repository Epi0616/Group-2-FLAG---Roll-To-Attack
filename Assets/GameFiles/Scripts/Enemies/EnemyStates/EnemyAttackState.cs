using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);
        enemy.Attack();
    }

}
