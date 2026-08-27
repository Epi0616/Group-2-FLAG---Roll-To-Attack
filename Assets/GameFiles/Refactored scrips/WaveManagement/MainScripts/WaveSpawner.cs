using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    public static event Action finishedSpawning;
    public static event Action waveInstanceFinishedSpawning;

    [SerializeField] private float healthScaleIncrement;
    public Stat enemyHealthScale { get; private set; }

    [Header("Setup")]
    [SerializeField] private GameObject playerRef;
    [SerializeField] private Camera cameraRef;
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private Vector2 spawnAreaCentrePoint = new Vector2(0, 15);
    [SerializeField] private float spawnAreaRadius = 50f;
    [SerializeField] private float spawnPointAreaRadius = 4f;
    [SerializeField] private float spawnWithinPlayerBound = 10f;

    [SerializeField] private LayerMask propsLayer;
    [SerializeField] private LayerMask groundLayer;

    private HashSet<Coroutine> activeRoutines = new HashSet<Coroutine>();

    private void OnEnable()
    {
        WaveScaling.setScaling += SetScaling;
    }

    private void OnDisable()
    {
        WaveScaling.setScaling -= SetScaling;
    }

    private void Awake()
    {
        enemyHealthScale = new Stat(1);
    }

    public void SpawnWave(Wave currentWave, bool isWaveEnemy)
    {
        StartCoroutine(ActiveSpawningRoutine(currentWave, isWaveEnemy));
    }

    private IEnumerator ActiveSpawningRoutine(Wave currentWave, bool isWaveEnemy)
    {
        Coroutine waveSpawn = StartCoroutine(SpawnWaveRoutine(currentWave, isWaveEnemy));
        activeRoutines.Add(waveSpawn);
        yield return waveSpawn;

        activeRoutines.Remove(waveSpawn);

        CheckForFinishedSpawning();
    }

    private void CheckForFinishedSpawning()
    {
        if (activeRoutines.Count <= 0)
        {
            finishedSpawning?.Invoke();
        }
    }

    private IEnumerator SpawnWaveRoutine(Wave currentWave, bool isWaveEnemy)
    {
        float longestSpawnTime = 0f;

        for (int i = 0; i < currentWave.waveGroups.Count; i++)
        { 
            bool nextGroupActive = false;
            WaveGroup currentGroup = currentWave.waveGroups[i];
            List<BaseWaveCondition> currentConditions = currentGroup.conditions;
            
            //activate conditions
            for (int j = 0; j < currentConditions.Count; j++)
            {
                currentConditions[j].Initialize(this);
            }

            //wait for group to be ready
            while (!nextGroupActive)
            {
                nextGroupActive = CheckForConditionsMet(currentConditions);
                longestSpawnTime -= Time.deltaTime;
                yield return null;
            }

            //spawn all entity blocks in the group at the same time
            List<EntityBlock> currentEntityBlocks = currentGroup.entityBlocks;
            for (int j = 0; j < currentEntityBlocks.Count; j++)
            {
                StartCoroutine(ActivateEntityBlock(currentEntityBlocks[j], isWaveEnemy));

                float currentSpawnTime = currentEntityBlocks[j].spawnDelay * currentEntityBlocks[j].count;
                if (currentSpawnTime > longestSpawnTime)
                {
                    longestSpawnTime = currentSpawnTime;
                }
            }
        }

        yield return new WaitForSeconds(longestSpawnTime);
    }

    private bool CheckForConditionsMet(List<BaseWaveCondition> currentConditions)
    {
        bool allConditionsMet = true;

        for (int i = 0; i < currentConditions.Count; i++)
        {
            currentConditions[i].UpdateCondition();
            if (!currentConditions[i].IsConditionMet())
            { 
                allConditionsMet = false;
            }
        }

        return allConditionsMet;
    }

    private IEnumerator ActivateEntityBlock(EntityBlock entityBlock, bool isWaveEnemy)
    {
        for (int i = 0; i < entityBlock.count; i++)
        {
            float delay = entityBlock.spawnDelay;
            yield return new WaitForSeconds(delay);

            GameObject entity = entityBlock.entity;
            PlaceEntityInWorldSpace(entity, isWaveEnemy);
        }
    }

    private void PlaceEntityInWorldSpace(GameObject obj, bool isWaveEnemy)
    {
        Vector3 spawnPosFinal = Vector3.zero;

        ISpawnModifier spawnModifier;
        if (obj.TryGetComponent<ISpawnModifier>(out spawnModifier))//if obj is of type spawn in the floor
        {
            switch (spawnModifier.spawnModifier)
            { 
                case SpawnModifier.spawnInGround:
                    spawnPosFinal = PickSpawnAreaCircular();
                    spawnPosFinal.y -= 10f;
                    break;

                case SpawnModifier.dragonSpawnInSky:
                    spawnPosFinal = PickSpawnAreaCircular();
                    //spawnPosFinal.y -= 10f;//currently the action sets the correct height, this is to make sure the player doesnt breifly see the dragon body before it teleports up high, while still allowing for the nav mesh agent to be initialized
                    break;

                case SpawnModifier.SlimeInSky:
                    spawnPosFinal = PickSpawnAreaCircular();
                    spawnPosFinal.y = 40f;
                    break;

                default: //duplicating this else feels bad but i currently am tired and cant think of a better way to do it
                    spawnPosFinal = PickSpawnAreaPoint();
                    break;
            }
        }
        else { Debug.LogError("BROWHERE IS THE SPAWN MOD????"); }

        if (obj == null) { Debug.LogError("obj is null"); }
        if (spawnPosFinal == null) { Debug.LogError("SpawnPos is null"); }

        GameObject spawnedEntity = ObjectPoolManager.SpawnObject(obj, spawnPosFinal, Quaternion.identity);

        EnemySetup(spawnedEntity, spawnModifier, isWaveEnemy);
    }

    private void EnemySetup(GameObject spawnedEntity, ISpawnModifier spawnModifier, bool isWaveEnemy)
    {
        if (spawnedEntity.TryGetComponent<AIDrivenEntity>(out AIDrivenEntity entity))
        {
            if (spawnModifier.spawnModifier != SpawnModifier.None)
            {
                entity.DisableAIAgent();
            }
        }

        if (spawnedEntity == null) { Debug.LogError("Wave Spawned Entity null"); }

        Entity spawnedEntityReference = spawnedEntity.GetComponent<Entity>();

        if (spawnedEntityReference is IWaveEnemy enemy)
        {
            enemy.isWaveEnemy = isWaveEnemy;
        }
        //spawnedEntityReference.Initialize();
        spawnedEntityReference.healthSystem.maxHealth.SetMultiplier(enemyHealthScale.GetFinalValue());
        spawnedEntityReference.textDisplaySystem.targetCamera = cameraRef;

        spawnedEntityReference.Reset();        
    }

    private Vector3 PickSpawnAreaCircular()
    {
        RaycastHit hit;

        for (int i = 0; i < 100; i++) //change to adjust for max iterations
        {
            // Pick area within a circle, if too close to the player reroll that position
            Vector2 randomArea = spawnAreaCentrePoint + Random.insideUnitCircle * spawnAreaRadius;
            Vector3 spawnPos = new Vector3(randomArea.x, 0f, randomArea.y);
            //Debug.Log(randomArea.x + " , " + randomArea.y);

            float distanceFromPlayer = Vector3.Distance(spawnPos, playerRef.transform.position);

            // Check if the chosen area is occupied by a prop
            bool isAreaOccupied = Physics.CheckSphere(new Vector3(spawnPos.x, 5f, spawnPos.z), 3f, propsLayer);
            //Debug.Log(isAreaOccupied);

            // Tolerance can be adjusted as needed 
            if (distanceFromPlayer > spawnWithinPlayerBound && !isAreaOccupied)
            {

                Ray ray = new Ray(new Vector3(spawnPos.x, 10f, spawnPos.z), new Vector3(0, -1, 0));
                if (Physics.Raycast(ray, out hit, 100f, groundLayer))
                {
                    //Debug.DrawLine(spawnPos, new Vector3(spawnPos.x, 100f, spawnPos.z), Color.green, 100f);
                    //Debug.DrawLine(hit.point, new Vector3(hit.point.x, 100f, hit.point.z), Color.blue, 100f);
                    //Debug.Log("Function Has Selected: " + spawnPos.x + " " + spawnPos.y + " " + spawnPos.z);
                    //Debug.Log("Hit Has Selected: " + hit.point.x + " " + hit.point.y + " " + hit.point.z);
                    return hit.point;
                }
            }
        }
           
        Debug.LogError("No Valid Spawn Point Found, Reverting to Normal Enemy Spawn Logic");
        return PickSpawnAreaPoint();
    }

    private Vector3 PickSpawnAreaPoint()
    {
        int choice = Random.Range(0, spawnPoints.Count);
        Vector3 chosenPoint = new Vector3(spawnPoints[choice].transform.position.x, 1f, spawnPoints[choice].transform.position.z);
        Vector2 spawnCentreArea = new Vector2(chosenPoint.x, chosenPoint.z);
        Vector2 randomArea = spawnCentreArea + Random.insideUnitCircle * spawnPointAreaRadius;
        return new Vector3(randomArea.x, chosenPoint.y, randomArea.y);
    }

    private void SetScaling(int iterations)
    {
        enemyHealthScale.SetMultiplier(MathF.Pow(healthScaleIncrement, iterations));
    }
}
