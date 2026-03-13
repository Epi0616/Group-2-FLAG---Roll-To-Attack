using UnityEngine;
using System.Collections;
using System;

public class SimpleRaiderEnemy : EnemyStateController
{
    
    [SerializeField] private float meleeAttackRadius;
    [SerializeField] private int meleeAttackDamage;
    [SerializeField] private float meleeAttackChargeTime;
    [SerializeField] private Color impactFieldColor;

    [SerializeField] private Transform attackOriginTransform;
    [SerializeField] private GameObject impactFieldPrefab;

    private GameObject impactFieldObj;
    private bool attackInterrupted;

    public override void Attack()
    {
        attackInterrupted = false;
        SpawnImpactField();
        StartCoroutine(ChargeTime());
    }

    private void MeleeAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(attackOriginTransform.position, meleeAttackRadius, playerLayer);
        foreach (var collider in colliders)
        {
            if (collider.gameObject == gameObject) { continue; }
            if (attackInterrupted) { break; }

            if (collider.gameObject.CompareTag("Player"))
            {
                
                playerController.healthSystem.OnTakeDamage(meleeAttackDamage);
                
            }

        }
        
        if (attackInterrupted)
        {
            return;
        }
        ChangeState(new EnemyLookAtPlayerState(attackCooldownStat.GetFinalValue()));
        //currentLookCoroutine = StartCoroutine(ContinueLookAtPlayer(attackCooldownStat.GetFinalValue()));
    }


    private IEnumerator ChargeTime()
    {
        yield return new WaitForSeconds(meleeAttackChargeTime);
        MeleeAttack();

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


    /*private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackOriginTransform.position, meleeAttackRadius);
    }*/
}
