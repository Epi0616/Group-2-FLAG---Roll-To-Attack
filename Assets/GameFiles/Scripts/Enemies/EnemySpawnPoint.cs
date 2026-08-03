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
    //private Vector3 areaBlockerStartPosition;

    [SerializeField] private Vector3 blockerStartPos, blockerEndPos;


    private void OnEnable()
    {
        WaveManager.WaveCountStart += HandleRemoveAreaBlocker;
        WaveSpawner.waveFinishedSpawning += HandleCloseSpawnArea;
    }

    private void OnDisable()
    {
        WaveManager.WaveCountStart -= HandleRemoveAreaBlocker;
        WaveSpawner.waveFinishedSpawning -= HandleCloseSpawnArea;
    }

    private void Start()
    {
        //areaBlockerStartPosition = areaBlocker.transform.position;
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
        ApplyKnockBackToEnemiesInSpawn(15f);     
        yield return new WaitForSeconds(0.3f);
        HandleApplyAreaBlocker();
    }

    private void HandleApplyAreaBlocker()
    {
        StartCoroutine(MoveAreaBlocker(blockerEndPos, blockerStartPos, 2f));
    }

    private void HandleRemoveAreaBlocker(float timeDelay)
    {
        StartCoroutine(MoveAreaBlocker(blockerStartPos, blockerEndPos, timeDelay));
    }

    private IEnumerator MoveAreaBlocker(Vector3 start, Vector3 end, float duration)
    {
        float timer = duration;
        float t = 0;

        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;
            areaBlocker.transform.localPosition = Vector3.Lerp(start, end, t);
            yield return null;
        }

        areaBlocker.transform.localPosition = end;
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
