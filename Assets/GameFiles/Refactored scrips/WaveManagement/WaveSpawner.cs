using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerRef;
    [SerializeField] private Camera cameraRef;
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private Vector2 spawnAreaCentrePoint = new Vector2(0, 15);
    [SerializeField] private float spawnAreaRadius = 50f;
    [SerializeField] private float spawnPointAreaRadius = 4f;
    [SerializeField] private float spawnWithinPlayerBound = 10f;

    [SerializeField] private LayerMask propsLayer;
    [SerializeField] private LayerMask groundLayer;

    public void SpawnWave(Wave currentWave)
    {
        StartCoroutine(SpawnWaveRoutine(currentWave));
    }

    private IEnumerator SpawnWaveRoutine(Wave currentWave)
    {
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
                yield return null;
            }

            //spawn all entity blocks in the group at the same time
            List<EntityBlock> currentEntityBlocks = currentGroup.entityBlocks;
            for (int j = 0; j < currentEntityBlocks.Count; j++)
            {
                StartCoroutine(ActivateEntityBlock(currentEntityBlocks[j]));
            }

        }
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

    private IEnumerator ActivateEntityBlock(EntityBlock entityBlock)
    {
        for (int i = 0; i < entityBlock.count; i++)
        {
            float delay = entityBlock.spawnDelay;
            yield return new WaitForSeconds(delay);

            GameObject entity = entityBlock.entity;
            PlaceEntityInWorldSpace(entity);
        }
    }

    private void PlaceEntityInWorldSpace(GameObject obj)
    {
        Vector3 spawnPosFinal;

        if (!true)//if obj is of type spawn in the floor
        {
            //Debug.Log("Golem Spawning");
            spawnPosFinal = PickSpawnAreaCircular();
            spawnPosFinal.y = spawnPosFinal.y - 10f;
        }
        else
        {
            // Spawn and place the new enemy
            spawnPosFinal = PickSpawnAreaPoint();
        }
        if (obj == null) { Debug.LogError("obj is null"); }
        if (spawnPosFinal == null) { Debug.LogError("SpawnPos is null"); }

        GameObject spawnedEntity = ObjectPoolManager.SpawnObject(obj, spawnPosFinal, Quaternion.identity);
        if (spawnedEntity == null) { Debug.LogError("Wave Spawned Entity null"); }
        if (spawnedEntity.GetComponent<Entity>().healthSystem != null)
        {
            spawnedEntity.GetComponent<Entity>().healthSystem.ResetSystem();
        }
        spawnedEntity.GetComponent<Entity>().textDisplaySystem.targetCamera = cameraRef;
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
}
