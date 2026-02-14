using UnityEngine;

public class EnemyStunnedState : EnemyBaseState
{
    private float duration;

    public EnemyStunnedState(float duration)
    {
        this.duration = duration;
    }

    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);
        enemy.rb.linearVelocity = Vector3.zero;
    }

    public override void UpdateState()
    {
        Vibrate();

        duration -= Time.deltaTime;
        if (duration < 0)
        {
            enemy.ChangeState(new EnemyMoveState());
        }
    }

    private void Vibrate()
    { 
        //make vibrate
    }

}
