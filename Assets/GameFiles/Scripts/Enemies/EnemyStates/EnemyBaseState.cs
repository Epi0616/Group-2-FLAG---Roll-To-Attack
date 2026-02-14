using UnityEngine;
using TMPro;
// moved constructor code here as i didnt wana delete but dont think its neccecary rn (i may very well be wrong)
//protected EnemyBaseState(EnemyBaseClass enemy)
//{
//    this.enemy = enemy;
//    playerTransform = enemy.playerTransform;
//}

public class EnemyBaseState : IEnemyState
{
    protected EnemyStateController enemy;

    public virtual void EnterState(EnemyStateController enemy) 
    {
        this.enemy = enemy;
    }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void ExitState() { }
}
