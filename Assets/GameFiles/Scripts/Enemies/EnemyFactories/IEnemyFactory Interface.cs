using UnityEngine;

public interface IEnemyFactory
{
    GameObject CreateEnemy();
    bool DesiredEnemyType(EnemyTypes enemyType);
}