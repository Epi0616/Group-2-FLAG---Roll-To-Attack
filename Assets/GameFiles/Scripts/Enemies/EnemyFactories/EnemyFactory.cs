using UnityEngine;

public class EnemyFactory : MonoBehaviour, IEnemyFactory
{
    [SerializeField] private GameObject enemyPrefab;

    public virtual GameObject CreateEnemy(Vector3 spawnPos)
    {
        return Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    public virtual bool DesiredEnemyType(EnemyTypes enemyType)
    {
        return false;
    }
}
