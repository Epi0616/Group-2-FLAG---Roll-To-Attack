using UnityEngine;
using System.Collections;
using System;

public class SandGolemEnemy : EnemyStateController
{
    //[SerializeField] private Vector3 meleeAttackHalfExtents;
    [Header("Golem Attack Variables")]
    [SerializeField] private float meleeAttackRadius;
    [SerializeField] private int meleeAttackDamage;
    [SerializeField] private float meleeAttackChargeTime;
    [SerializeField] private float golemKnockBackForce;
    [SerializeField] private Color impactFieldColor;

    [Header("Variables not to be Adjusted")]
    [SerializeField] private Transform attackOriginTransform;
    [SerializeField] private LayerMask canBeKnockedBackByGolem;
    [SerializeField] private GameObject impactFieldPrefab;

    private bool attackInterrupted;
    private GameObject impactFieldObj;

    public override void Attack()
    {
        attackInterrupted = false;
        SpawnImpactField();
        StartCoroutine(ChargeTime());
    }

    private void GolemSlam()
    {
        Collider [] colliders = Physics.OverlapSphere(attackOriginTransform.position, meleeAttackRadius, canBeKnockedBackByGolem);
        foreach (var collider in colliders)
        {
            if (collider.gameObject == gameObject) { continue; }
            if (attackInterrupted) { break; }

            if (collider.gameObject.CompareTag("Player"))
            {
                //Debug.Log("Golem Attack Hit Player");
                playerController.healthSystem.OnTakeDamage(meleeAttackDamage);
                continue;
            }
            else if (collider.gameObject.CompareTag("Enemy"))
            {
                //Debug.Log("Golem KnockBack");
                EnemyStateController enemyRef = collider.gameObject.GetComponent<EnemyStateController>();
                if (enemyRef == null)
                {
                    Debug.LogError("EnemyRef is NULL");
                }
                enemyRef.OnTakeGolemKnockback(attackOriginTransform.position, golemKnockBackForce);
            }
        }

        if (attackInterrupted)
        {
            return;
        }

        
        StartCoroutine(ContinueLookAtPlayer(attackCooldownStat.GetFinalValue()));
    }

    private IEnumerator ChargeTime()
    {
        yield return new WaitForSeconds(meleeAttackChargeTime);
        GolemSlam();

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackOriginTransform.position, meleeAttackRadius);
    }

    public override void CompleteAttack()
    {
        attackInterrupted = true; 
        Destroy(impactFieldObj);
    }

    private void SpawnImpactField()
    {
        Vector3 impactFieldPosition = new Vector3(attackOriginTransform.position.x, attackOriginTransform.position.y - 1f, attackOriginTransform.position.z);
        impactFieldObj = Instantiate(impactFieldPrefab, impactFieldPosition, Quaternion.identity);
        EnemyAttackImpactField impactField = impactFieldObj.GetComponent<EnemyAttackImpactField>();
        impactField.PassInValuesColorRadiusLifeTimeChargeTime(impactFieldColor, meleeAttackRadius * 0.9f, 2.5f, meleeAttackChargeTime);
    }

}
