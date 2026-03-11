using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveState : EnemyBaseState
{
    Vector3 playerPosition;
    Vector3 targetVector;

    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);
        enemy.animator.SetBool("isMoving", true);
        enemy.enemyAgent.enabled = true;
        enemy.enemyAgent.updatePosition = true;
        enemy.enemyAgent.updateRotation = true;
        
        if (enemy.animator != null)
        {
            enemy.animator.speed = 1f;
        }
        

        MoveTowardsPlayerNavMesh();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (enemy.isKnockedBack || enemy.isStunned )
        {
            return;
        }

        playerPosition = enemy.playerReference.transform.position;
        targetVector = (playerPosition - enemy.transform.position);

        //enemy.transform.rotation = Quaternion.LookRotation(targetVector);

        MoveTowardsPlayerNavMesh();

        if (CheckIfAIHasStopped(enemy.enemyAgent))
        {
            enemy.ChangeState(new EnemyAttackState());
        }
    }

    public override void FixedUpdateState()
    {
        //MoveTowardsPlayerVector();
    }

    private bool CheckIfAIHasStopped(NavMeshAgent enemyAgent)
    {
        if (enemyAgent.pathPending) { return false; }

        if (!enemyAgent.hasPath) { return false; }

        if (enemyAgent.remainingDistance - 2f > enemyAgent.stoppingDistance) { return false; }

        if (enemyAgent.velocity.magnitude > 0.1f) { return false; }

        return true;


    }

    private void MoveTowardsPlayerNavMesh()
    {
        enemy.enemyAgent.destination = playerPosition;
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
        enemy.enemyAgent.updatePosition = false;
        enemy.enemyAgent.updateRotation = false;
        enemy.enemyAgent.enabled = false;
        enemy.animator.SetBool("isMoving", false);
        
        if (enemy.animator != null)
        {
            enemy.animator.speed = 0f;
        }
        
        //enemy.enemyAgent.ResetPath();
    }
}
