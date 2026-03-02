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
                
                playerController.OnTakeDamage(meleeAttackDamage);
                
            }

        }
        
        if (attackInterrupted)
        {
            return;
        }
        StartCoroutine(ContinueLookAtPlayer());
    }


    private IEnumerator ChargeTime()
    {
        yield return new WaitForSeconds(meleeAttackChargeTime);
        MeleeAttack();

    }

    private IEnumerator ContinueLookAtPlayer()
    {
        Vector3 playerDir = playerReference.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(playerDir);
        float movementTimer = 0f;
        while (movementTimer < attackCooldown && playerDir.magnitude < attackRange || movementTimer < 0.5f)
        {
            playerDir = playerReference.transform.position - transform.position;
            playerDir.y = transform.position.y;
            lookRotation = Quaternion.LookRotation(playerDir);
            movementTimer += Time.deltaTime;
            float t = movementTimer / attackCooldown;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, t);
            yield return null;
        }
        ChangeState(new EnemyMoveState());
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
