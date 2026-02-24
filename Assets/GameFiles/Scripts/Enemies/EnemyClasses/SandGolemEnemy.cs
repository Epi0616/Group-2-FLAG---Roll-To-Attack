using UnityEngine;
using System.Collections;
using System;

public class SandGolemEnemy : EnemyStateController
{
    //[SerializeField] private Vector3 meleeAttackHalfExtents;
    [Header("Golem Attack Variables")]
    [SerializeField] private float meleeAttackRange;
    [SerializeField] private int meleeAttackDamage;
    [SerializeField] private float golemKnockBackForce;
    [SerializeField] private Color impactFieldColor;

    [Header("Variables not to be Adjusted")]
    [SerializeField] private Transform attackOriginTransform;
    [SerializeField] private LayerMask canBeKnockedBackByGolem;
    [SerializeField] private GameObject impactFieldPrefab;

    private bool didhitDetected;
    RaycastHit hit;

    public override void Attack()
    {
        SpawnImpactField();
        StartCoroutine(ChargeTime());
    }

    private void GolemSlam()
    {
        Collider []
        colliders = Physics.OverlapSphere(attackOriginTransform.position, meleeAttackRange, canBeKnockedBackByGolem);
        foreach (var collider in colliders)
        {
            if (collider.gameObject == gameObject) { continue; }

            if (collider.gameObject.CompareTag("Player"))
            {
                //Debug.Log("Golem Attack Hit Player");
                playerController.OnTakeDamage(meleeAttackDamage);
                continue;
            }
            else if (collider.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Golem KnockBack");
                EnemyStateController enemyRef = collider.gameObject.GetComponent<EnemyStateController>();
                if (enemyRef == null)
                {
                    Debug.LogError("EnemyRef is NULL");
                }
                enemyRef.OnTakeKnockback(attackOriginTransform.position, golemKnockBackForce);
            }
        }

        StartCoroutine(TimeBeforeMovingAfterAttack());
    }

    private IEnumerator ChargeTime()
    {
        yield return new WaitForSeconds(0.5f);
        GolemSlam();

    }

    private IEnumerator TimeBeforeMovingAfterAttack()
    {
        yield return new WaitForSeconds(1.5f);
        ChangeState(new EnemyMoveState());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackOriginTransform.position, meleeAttackRange);
    }

    public override void CompleteAttack()
    {

    }

    private void SpawnImpactField()
    {
        Vector3 impactFieldPosition = new Vector3(attackOriginTransform.position.x, attackOriginTransform.position.y - 1f, attackOriginTransform.position.z);
        GameObject impactFieldObj = Instantiate(impactFieldPrefab, impactFieldPosition, Quaternion.identity);
        EnemyAttackImpactField impactField = impactFieldObj.GetComponent<EnemyAttackImpactField>();
        impactField.PassInValuesColorRadiusLifeTimeChargeTime(impactFieldColor, meleeAttackRange * 0.9f, 2.5f, 0.5f);
    }

}
