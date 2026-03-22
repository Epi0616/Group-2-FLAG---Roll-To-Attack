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
        enemy.enemyAgent.enabled = true;
        enemy.enemyAgent.updatePosition = true;
        enemy.enemyAgent.updateRotation = true;            
        
        playerPosition = enemy.playerReference.transform.position;
        MoveTowardsPlayerNavMesh();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        footStepTimer += Time.deltaTime;

        if (footStepTimer > 1f)
        {
            AudioManager.instance.PlayRandomSoundClip(enemy.EnemyWalkSounds);
            footStepTimer = 0f;
        }

        if (enemy.isKnockedBack || enemy.isStunned )
        {
            return;
        }

        playerPosition = enemy.playerReference.transform.position;
        targetVector = (playerPosition - enemy.transform.position);

        //enemy.transform.rotation = Quaternion.LookRotation(targetVector);

        MoveTowardsPlayerNavMesh();

        if (targetVector.magnitude <= enemy.attackRange)
        {
            enemy.ChangeState(new EnemyAttackState());
        }

        //if (CheckIfAIHasStopped(enemy.enemyAgent))
        //{
        //    enemy.ChangeState(new EnemyAttackState());
        //}
    }

    public override void FixedUpdateState()
    {
        //MoveTowardsPlayerVector();
    }

    private bool CheckIfAIHasStopped(NavMeshAgent enemyAgent)
    {
        if (enemyAgent == null || !enemyAgent.isOnNavMesh) return false;
        if (enemyAgent.pathPending) return false;
        if (!enemyAgent.hasPath) return false;
        if (enemyAgent.remainingDistance > enemyAgent.stoppingDistance) return false;
        if (enemyAgent.velocity.magnitude > 0.1f) return false;

        return true;
    }

    private void MoveTowardsPlayerNavMesh()
    {
        if (enemy.enemyAgent == null || !enemy.enemyAgent.isOnNavMesh) return;

        if (Time.time - lastSetTime > 0.25f)
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

    public override void ExitState()
    {
        enemy.enemyAgent.enabled = false;

    }
}
