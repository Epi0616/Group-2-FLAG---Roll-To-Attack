using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    private Vector2 spawnAreaCentrePoint = Vector2.zero;
    private float spawnAreaRadius = 50f;
    Vector3 spawnPositionForDraw;

    private Vector2 playerPos;
    private GameObject playerRef;
    private float spawnTolerance = 50f;
    private IEnemyFactory[] enemyFactories;
    [SerializeField] private LayerMask propsLayer;
    [SerializeField] private float spawnPointAreaRadius = 4f;
    [SerializeField] private float enemySpawnInterval;
    [Header("This List holds all the Spawn Points placed in the scene, to use press the +")]
    [Header("then drag in a SpawnPoint Prefab.   DOES NOTHING IF EMPTY")]
    [SerializeField] private List<EnemySpawnPoint> spawnPointList;

    private void Awake()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        enemyFactories = gameObject.GetComponentsInChildren<IEnemyFactory>();
        // This will be adjusted once the Director is complete to avoid using .FindObject
        playerPos = playerRef.transform.position;
    }

    private void OnEnable()
    {
        EnemyDirector.SpawnWave += StartWaveSpawn;
    }

    private void OnDisable()
    {
        EnemyDirector.SpawnWave -= StartWaveSpawn;
    }

    private void StartWaveSpawn(List<EnemyTypes> wave)
    {
        StartCoroutine(SpawnWave(wave));
    }

    public IEnumerator SpawnWave(List<EnemyTypes> wave)  
    {
        foreach (EnemyTypes enemy in wave)
        {
            // Find the corrisponding Factory
            IEnemyFactory factory = CheckEnemyFactory(enemy);
            if (factory != null)
            {
                Vector3 spawnPos;
                if (enemy == EnemyTypes.SandGolem)
                {
                    spawnPos = PickSpawnAreaCircular();                 
                    spawnPos.y = spawnPos.y - 4f;
                }
                else
                {
                    // Spawn and place the new enemy
                    spawnPos = PickSpawnAreaPoint(spawnPointList);
                }
                   
                GameObject spawnedEnemy = factory.CreateEnemy(spawnPos);
                
                //Debug.Log(spawnPos.x + " " + spawnPos.y + " " + spawnPos.z);
                //spawnedEnemy.transform.position = spawnPos;
                EnemyStateController spawnedEnemyCont = spawnedEnemy.GetComponent<EnemyStateController>();
                spawnedEnemyCont.playerReference = playerRef;             
                yield return new WaitForSeconds(enemySpawnInterval);
            }
        } 
    }

    // This Function uses random area spawning that the Golem uses to spawn
    private Vector3 PickSpawnAreaCircular()
    {
        int iterations = 0;
        while (true && iterations < 100)
        {
            // Pick area within a circle, if too close to the player reroll that position
            Vector2 randomArea = playerPos + Random.insideUnitCircle * spawnAreaRadius;
            Vector3 spawnPos = new Vector3(randomArea.x, 0f, randomArea.y);

            float distanceFromPlayer = Vector3.Distance(spawnPos, playerPos);

            // Check if the chosen area is occupied by a prop
            bool isAreaOccupied = Physics.CheckSphere(new Vector3(spawnPos.x, 5f, spawnPos.z), 3f, propsLayer);       
            //Debug.Log(isAreaOccupied);

            // Tolerance can be adjusted as needed 
            if (distanceFromPlayer > spawnTolerance && !isAreaOccupied)
            {
                return spawnPos;
            }
            iterations++;
        }
        Debug.LogError("No Valid Spawn Point Found");
        return Vector3.zero;
    }

    // This Function picks one of the spawn points in the List to act as the position for enemy spawns
    private Vector3 PickSpawnAreaPoint(List<EnemySpawnPoint> spawnPoints)
    {
        int choice = Random.Range(0, spawnPoints.Count);
        Vector3 chosenPoint = new Vector3(spawnPoints[choice].transform.position.x, 1f, spawnPoints[choice].transform.position.z);
        Vector2 spawnCentreArea = new Vector2(chosenPoint.x, chosenPoint.z);
        Vector2 randomArea = spawnCentreArea + Random.insideUnitCircle * spawnPointAreaRadius;
        return new Vector3(randomArea.x, chosenPoint.y, randomArea.y);
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
