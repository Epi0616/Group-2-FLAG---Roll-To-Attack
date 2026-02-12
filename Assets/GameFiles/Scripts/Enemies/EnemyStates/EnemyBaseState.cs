using UnityEngine;

public class EnemyBaseState : IEnemyState
{
    protected EnemyBaseClass enemy;
    protected Transform playerTransform;

    protected EnemyBaseState(EnemyBaseClass enemy)
    {
        this.enemy = enemy;
        playerTransform = enemy.playerTransform;
    }
    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void ExitState() { } 
}
