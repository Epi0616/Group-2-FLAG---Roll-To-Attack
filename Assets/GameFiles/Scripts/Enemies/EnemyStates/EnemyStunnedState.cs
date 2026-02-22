using UnityEngine;

public class EnemyStunnedState : EnemyBaseState
{
    private float duration;
    private float speed = 5f;
    private float intensity = 0.1f;

    public EnemyStunnedState(float duration)
    {
        this.duration = duration;
    }

    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);
        enemy.enemyAgent.enabled = false;
        enemy.isStunned = true;
        enemy.rb.linearVelocity = Vector3.zero;
    }

    public override void UpdateState()
    {
        Vibrate();

        duration -= Time.deltaTime;
        if (duration < 0)
        {
            //enemy.isStunned = false;
            enemy.ChangeState(new EnemyMoveState());
        }
    }

    public override void ExitState()
    {
        enemy.enemyAgent.enabled = true;
        enemy.enemyAgent.Warp(enemy.transform.position);
        enemy.isStunned = false;
    }

    private void Vibrate()
    {
        //make vibrate
        
    }

}
