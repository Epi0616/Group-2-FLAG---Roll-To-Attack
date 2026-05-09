using UnityEngine;

public class EnemyLookAtPlayerState : EnemyBaseState
{
    private float duration;
    private float activeTimer;
    private Vector3 playerDir;
    private Quaternion lookRotation;

    public EnemyLookAtPlayerState(float duration)
    {
        this.duration = duration;
    }

    public override void EnterState(EnemyStateController enemy)
    {
        if(enemy.animator != null)
        {
            enemy.animator.speed = 0f;
        }


        activeTimer = 0f;
        base.EnterState(enemy);
    }

    public override void UpdateState()
    {
        if(activeTimer > duration || playerDir.magnitude > enemy.attackRange * 1.25f && activeTimer > 0.5f)
        {
            enemy.ChangeState(new EnemyMoveState());
        }

        activeTimer += Time.deltaTime;

        if (!enemy.canMove) { return; }

        playerDir = enemy.playerReference.transform.position - enemy.transform.position;
        playerDir.y = enemy.transform.position.y;
        lookRotation = Quaternion.LookRotation(playerDir);
        lookRotation.z = 0f;
        lookRotation.x = 0f; 
        float t = activeTimer / duration;
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, t);
        
    }

    public override void ExitState()
    {
        if (enemy.animator != null && enemy.canMove)
        {
            enemy.animator.speed = 1f;
        }
    }
}
