using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject centerPos, areaBlocker;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float range = 10f;

    private float pushInterval = 1f;
    private float pushTimer = 0;
    private Vector3 areaBlockerStartPosition;

    private void OnEnable()
    {
        WaveManager.WaveCountStart += RaiseAreaBlocker;
        WaveSpawner.waveFinishedSpawning += HandleCloseSpawnArea;
    }

    private void OnDisable()
    {
        WaveManager.WaveCountStart -= RaiseAreaBlocker;
        WaveSpawner.waveFinishedSpawning -= HandleCloseSpawnArea;
    }

    private void Start()
    {
        areaBlockerStartPosition = areaBlocker.transform.position;
    }

    //private void Update()
    //{
    //    pushTimer += Time.deltaTime;
    //    if (pushTimer > pushInterval)
    //    {
    //        ApplyKnockBackToEnemiesInSpawn(5f);
    //        pushTimer = 0;
    //    }
    //}

    private void HandleCloseSpawnArea()
    {
        StartCoroutine(CloseSpawnArea());
    }

    private IEnumerator CloseSpawnArea()
    {
        yield return new WaitForSeconds(0.75f);
        ApplyKnockBackToEnemiesInSpawn(10f);     
        yield return new WaitForSeconds(0.3f);
        LowerAreaBlocker();
    }

    private void RaiseAreaBlocker(float timeDelay)
    {
        areaBlocker.transform.position = areaBlockerStartPosition + new Vector3(0, 25f, 0);
    }

    private void LowerAreaBlocker()
    { 
        areaBlocker.transform.position = areaBlockerStartPosition;
    }

    private bool ApplyKnockBackToEnemiesInSpawn(float force)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, enemyMask);

        foreach (Collider collider in colliders)
        {
            Entity hitEntity = collider.gameObject.GetComponent<Entity>();

            Vector3 targetPos = centerPos.transform.position;
            targetPos.y += 10f;
            hitEntity.OnRecieveEffect(
                new ActiveStatusEffect(new SafeKBEffect(targetPos, -force),
                new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
                true));
        }
        return true;
    }
}
