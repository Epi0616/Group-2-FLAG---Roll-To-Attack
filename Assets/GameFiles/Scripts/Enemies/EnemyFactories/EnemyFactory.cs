using UnityEngine;

public class EnemyFactory : MonoBehaviour, IEnemyFactory
{
    [SerializeField] private GameObject enemyPrefab;

    public virtual GameObject CreateEnemy()
    {
        return Instantiate(enemyPrefab);
    }

    public virtual bool DesiredEnemyType(EnemyTypes enemyType)
    {
        return false;
    }
}
