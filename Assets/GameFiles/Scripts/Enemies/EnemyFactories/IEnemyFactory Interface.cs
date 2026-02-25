using UnityEngine;

public interface IEnemyFactory
{
    GameObject CreateEnemy(Vector3 spawnPos);
    bool DesiredEnemyType(EnemyTypes enemyType);
}