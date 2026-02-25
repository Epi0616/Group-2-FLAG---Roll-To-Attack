using UnityEngine;

public class GolemEnemyFactory : EnemyFactory
{
    public override GameObject CreateEnemy(Vector3 spawnPos)
    {
        GameObject enemyObj = base.CreateEnemy(spawnPos);
        return enemyObj;
    }

    public override bool DesiredEnemyType(EnemyTypes enemyType)
    {
        return enemyType is EnemyTypes.SandGolem;
    }
}
