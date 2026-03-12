using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    private Vector2 spawnAreaCentrePoint = new Vector2(0f, 15f);
    private float spawnAreaRadius = 50f;
    Vector3 spawnPositionForDraw;
    
    
    private Vector2 playerPos;
    private GameObject playerRef;
    private float spawnTolerance = 50f;
    private IEnemyFactory[] enemyFactories;
    [SerializeField] private LayerMask propsLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float spawnPointAreaRadius = 4f;
    [SerializeField] private float enemySpawnInterval;
    [SerializeField] private float enemyScalingHealthMultiplier;
    [SerializeField] private float healthScalingIncreasePerWave;
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
        Vector3 spawnPosFinal;
        foreach (EnemyTypes enemy in wave)
        {
            // Find the corrisponding Factory
            IEnemyFactory factory = CheckEnemyFactory(enemy);
            if (factory != null)
            {
                
                if (enemy == EnemyTypes.SandGolem)
                {
                    //Debug.Log("Golem Spawning");
                    spawnPosFinal = PickSpawnAreaCircular();                 
                    spawnPosFinal.y = spawnPosFinal.y - 10f;
                }
                else
                {
                    // Spawn and place the new enemy
                    spawnPosFinal = PickSpawnAreaPoint(spawnPointList);
                }
                //Debug.Log("Spawner Has Selected: " + spawnPosFinal.x + " " + spawnPosFinal.y + " " + spawnPosFinal.z);
                GameObject spawnedEnemy = factory.CreateEnemy(spawnPosFinal);
                
                //Debug.Log(spawnPos.x + " " + spawnPos.y + " " + spawnPos.z);
                //spawnedEnemy.transform.position = spawnPosFinal;
                EnemyStateController spawnedEnemyCont = spawnedEnemy.GetComponent<EnemyStateController>();
                spawnedEnemyCont.AdjustScaledHealth(enemyScalingHealthMultiplier);
                spawnedEnemyCont.playerReference = playerRef;             
                yield return new WaitForSeconds(enemySpawnInterval);
                
                
            }
        }
        if (enemySpawnInterval > 0.2)
        {
            enemySpawnInterval -= 0.1f;
        }
        enemyScalingHealthMultiplier += healthScalingIncreasePerWave;
    }

    // This Function uses random area spawning that the Golem uses to spawn
    private Vector3 PickSpawnAreaCircular()
    {
        RaycastHit hit;
        int iterations = 0;
        while (true && iterations < 100)
        {
            // Pick area within a circle, if too close to the player reroll that position
            Vector2 randomArea = spawnAreaCentrePoint + Random.insideUnitCircle * spawnAreaRadius;
            Vector3 spawnPos = new Vector3(randomArea.x, 0f, randomArea.y);
            //Debug.Log(randomArea.x + " , " + randomArea.y);
            
            float distanceFromPlayer = Vector3.Distance(spawnPos, playerPos);

            // Check if the chosen area is occupied by a prop
            bool isAreaOccupied = Physics.CheckSphere(new Vector3(spawnPos.x, 5f, spawnPos.z), 3f, propsLayer);       
            //Debug.Log(isAreaOccupied);

            // Tolerance can be adjusted as needed 
            if (distanceFromPlayer > spawnTolerance && !isAreaOccupied)
            {
                
                Ray ray = new Ray(new Vector3(spawnPos.x, 10f, spawnPos.z), new Vector3(0, -1, 0));
                if(Physics.Raycast(ray, out hit, 100f, groundLayer))
                {
                    //Debug.DrawLine(spawnPos, new Vector3(spawnPos.x, 100f, spawnPos.z), Color.green, 100f);
                    //Debug.DrawLine(hit.point, new Vector3(hit.point.x, 100f, hit.point.z), Color.blue, 100f);
                    //Debug.Log("Function Has Selected: " + spawnPos.x + " " + spawnPos.y + " " + spawnPos.z);
                    //Debug.Log("Hit Has Selected: " + hit.point.x + " " + hit.point.y + " " + hit.point.z);
                    return hit.point;
                }
            }
            Debug.DrawLine(spawnPos, new Vector3(spawnPos.x, 100f, spawnPos.z), Color.red, 100f);
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
