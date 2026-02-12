using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    private Vector2 spawnAreaCentrePoint = Vector2.zero;
    private float spawnAreaRadius = 50;
    private Vector2 playerPos;
    private float spawnTolerance = 30f;
    private IEnemyFactory[] enemyFactories;
    [Header("This List holds all the Spawn Points placed in the scene, to use press the +")]
    [Header("then drag in a SpawnPoint Prefab.   DOES NOTHING IF EMPTY")]
    [SerializeField] private List<EnemySpawnPoint> spawnPointList;

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
                Vector3 spawnPos = PickSpawnAreaPoint(spawnPointList);
                spawnedEnemy.transform.position = spawnPos;
            }
        }
    }

    // This Function uses the previous random area spawning that I may use for the golem later in development
    private Vector3 PickSpawnAreaCircular()
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

    // This Function picks one of the spawn points in the List to act as the position for enemy spawns
    private Vector3 PickSpawnAreaPoint(List<EnemySpawnPoint> spawnPoints)
    {
        int choice = Random.Range(0, spawnPoints.Count);
        return new Vector3(spawnPoints[choice].transform.position.x, 1f, spawnPoints[choice].transform.position.z);
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
