using UnityEngine;

public class EnemyStunnedState : EnemyBaseState
{
    private float duration;
    private float stunTimer;
    private float speed = 50f;
    private float intensity = 0.1f;
    private Vector3 initialPosition;

    public EnemyStunnedState(float duration)
    {
        this.duration = duration;
    }

    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);

        enemy.enemyAgent.enabled = false;
        enemy.isStunned = true;

        stunTimer = duration;      
        initialPosition = enemy.transform.position;
    }

    public override void UpdateState()
    {
        stunTimer -= Time.deltaTime;
        if (stunTimer < 0)
        {
            enemy.ChangeState(new EnemyMoveState());
        }
    }

    public override void FixedUpdateState()
    {
        Vibrate();
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
        float x = Mathf.Sin(Time.time * speed) * intensity;
        float z = Mathf.Sin(Time.time * speed) * intensity;

        enemy.transform.position = initialPosition + new Vector3(x, 0, z);
        
    }

}
