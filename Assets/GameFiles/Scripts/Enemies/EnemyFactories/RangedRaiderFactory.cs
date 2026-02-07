using UnityEngine;

public class RangedRaiderFactory : EnemyFactory
{
    public override GameObject CreateEnemy()
    {
        GameObject enemyObj = base.CreateEnemy();
        return enemyObj;
    }

    public override bool DesiredEnemyType(EnemyTypes enemyType)
    {
        return enemyType is EnemyTypes.RangedRaider;
    }
}
