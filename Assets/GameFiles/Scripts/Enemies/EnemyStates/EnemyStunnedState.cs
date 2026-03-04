using UnityEngine;
using System.Collections;
using System;

public class EnemyStunnedState : EnemyBaseState
{
    private float duration;
    private float stunTimer;

    public EnemyStunnedState(float duration)
    {
        this.duration = duration;
    }

    public override void EnterState(EnemyStateController enemy)
    {
        base.EnterState(enemy);

        enemy.enemyAgent.enabled = false;
        enemy.isStunned = true;
        enemy.StartVibrating(duration);
    
        stunTimer = duration;      
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
       
    }

    public override void ExitState()
    {
        // can be uncommented if we want diminishing returns on stuns
        //enemy.OnRecieveEffect(new StunResistanceEffect(2f, 0.1f));
       
        enemy.OnRecieveEffect(new FragileEffect(2f, 1.5f));                

        enemy.isStunned = false;
        enemy.StopVibrating();
        enemy.enemyAgent.enabled = true;
        enemy.enemyAgent.Warp(enemy.transform.position);

        
    }

    

}
