using UnityEngine;

public interface IEnemyState
{
    public void EnterState();

    public void UpdateState();

    public void FixedUpdateState();

    public void ExitState();
}
