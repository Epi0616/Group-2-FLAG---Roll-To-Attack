using UnityEngine;

public class EnemySpawnState : EnemyBaseState
{
    

    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);

        enemy.isSpawning = true;

        if (enemy.hasSpawnVibration)
        {
            if (enemy.animator != null)
            {
                enemy.animator.speed = 0f;
            }
            enemy.StartSpawnVibration();
        }
        else
        {
            enemy.isSpawning = false;
            enemy.ChangeState(new EnemyMoveState());
        }
    }

    public override void ExitState()
    {
        enemy.isSpawning = false;
        if (enemy.animator != null)
        {
            enemy.animator.speed = 1f;
        }
    }

}


