using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveState : EnemyBaseState
{
    Vector3 playerPosition;
    Vector3 targetVector;
    float lastSetTime;
    float footStepTimer;

    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);

        //enemy.animator.SetBool("isMoving", true);

        enemy.EnableAI();

        playerPosition = enemy.playerReference.transform.position;
        MoveTowardsPlayerNavMesh();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (enemy.isAIDisabled) { return; }

        playerPosition = enemy.playerReference.transform.position;
        targetVector = (playerPosition - enemy.transform.position);

        if (targetVector.magnitude <= enemy.attackRange && enemy.canAttack)
        {
            enemy.ChangeState(new EnemyAttackState());
        }

        if (!enemy.canMove)
        {
            enemy.enemyAgent.SetDestination(enemy.transform.position);
            return;
        }

        footStepTimer += Time.deltaTime;

        if (footStepTimer > 1f)
        {
            AudioManager.instance.PlayRandomSoundClip(enemy.EnemyWalkSounds);
            footStepTimer = 0f;
        }

        MoveTowardsPlayerNavMesh();

    }

    public override void FixedUpdateState()
    {
        //MoveTowardsPlayerVector();
    }

    private void MoveTowardsPlayerNavMesh()
    {
        if (enemy.enemyAgent == null || !enemy.enemyAgent.isOnNavMesh) return;

        if (Time.time - lastSetTime > 0.15f)
        {
            enemy.enemyAgent.SetDestination(playerPosition);
            lastSetTime = Time.time;
        }
    }

    private void MoveTowardsPlayerVector()
    {
        Vector3 targetVector = (playerPosition - enemy.transform.position);
        Vector3 targetDirection = targetVector.normalized;
        targetDirection.y = 0;
        enemy.rb.linearVelocity = targetDirection * enemy.moveSpeedStat.GetFinalValue();
    }
   
}
