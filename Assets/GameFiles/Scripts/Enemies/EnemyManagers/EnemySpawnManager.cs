using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    private Vector2 spawnAreaCentrePoint = Vector2.zero;
    [SerializeField] private float spawnAreaRadius;
    private Vector2 playerPos;
    private float spawnTolerance = 30f;
    private IEnemyFactory[] enemyFactories;

    private void Start()
    {
        enemyFactories = gameObject.GetComponentsInChildren<IEnemyFactory>();
        // This will be adjusted once the Director is complete to avoid using .FindObject
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void OnEnable()
    {
        EnemyDirector.SpawnWave += SpawnWave;
    }

    private void OnDisable()
    {
        EnemyDirector.SpawnWave -= SpawnWave;
    }

    public void SpawnWave(List<EnemyTypes> wave)  
    {
        foreach (EnemyTypes enemy in wave)
        {
            // Find the corrisponding Factory
            IEnemyFactory factory = CheckEnemyFactory(enemy);
            if (factory != null)
            {
                // Spawn and place the new enemy
                GameObject spawnedEnemy = factory.CreateEnemy();
                Vector3 spawnPos = PickSpawnArea();
                spawnedEnemy.transform.position = spawnPos;
            }
        }
    }

    private Vector3 PickSpawnArea()
    {
        while (true)
        {
            // Pick area within a circle, if too close to the player reroll that position
            Vector2 randomArea = spawnAreaCentrePoint + Random.insideUnitCircle * spawnAreaRadius;
            Vector3 spawnPos = new Vector3(randomArea.x, 1f, randomArea.y);
            float distanceFromPlayer = Vector3.Distance(spawnPos, playerPos);
            // Tolerance can be adjusted as needed 
            if (distanceFromPlayer > spawnTolerance)
            {
                return spawnPos;
            }

        }

    }

    private IEnemyFactory CheckEnemyFactory(EnemyTypes enemy)
    {
        foreach (IEnemyFactory factory in enemyFactories)
        {
            if (factory.DesiredEnemyType(enemy))
            {
                return factory;
            }
        }
        Debug.LogError("Enemy Factory not found");
        return null;
    }
}
