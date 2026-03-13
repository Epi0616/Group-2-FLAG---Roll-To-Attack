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
        activeTimer = 0f;
        base.EnterState(enemy);
    }

    public override void UpdateState()
    {
        if((activeTimer > duration || playerDir.magnitude > enemy.attackRange * 1.25f && activeTimer > 0.5f) && (!enemy.isStunned && !enemy.isKnockedBack))
        {
            enemy.ChangeState(new EnemyMoveState());
        }
        
        playerDir = enemy.playerReference.transform.position - enemy.transform.position;
        playerDir.y = enemy.transform.position.y;
        lookRotation = Quaternion.LookRotation(playerDir);
        lookRotation.z = 0f;
        lookRotation.x = 0f;
        activeTimer += Time.deltaTime;
        float t = activeTimer / duration;
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, t);
        
    }
}
