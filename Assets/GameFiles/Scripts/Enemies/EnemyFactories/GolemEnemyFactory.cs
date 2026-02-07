using UnityEngine;

public class GolemEnemyFactory : EnemyFactory
{
    public override GameObject CreateEnemy()
    {
        GameObject enemyObj = base.CreateEnemy();
        return enemyObj;
    }

    public override bool DesiredEnemyType(EnemyTypes enemyType)
    {
        return enemyType is EnemyTypes.SandGolem;
    }
}
