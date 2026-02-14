using UnityEngine;

public interface IEnemyState
{
    public void EnterState(EnemyStateController enemy);

    public void UpdateState();

    public void FixedUpdateState();

    public void ExitState();
}
