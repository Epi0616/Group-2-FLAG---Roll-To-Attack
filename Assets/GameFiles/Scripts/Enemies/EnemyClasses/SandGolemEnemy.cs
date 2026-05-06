using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    private bool attackCompleted;
    private GameObject impactFieldObj;
    private EnemyAttackImpactField impactField;

    private Coroutine charge;

    public override void Attack()
    {
        attackCompleted = false;
        attackInterrupted = false;
        LookAtPlayer();
        SpawnImpactField();
        charge = StartCoroutine(ChargeTime());
    }

    private void GolemSlam()
    {
        Collider [] colliders = Physics.OverlapSphere(attackOriginTransform.position, meleeAttackRadius, canBeKnockedBackByGolem);
        AudioManager.instance.PlayRandomSoundClip(EnemyActiveAttackSounds, default, 0.5f);
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
                //enemyRef.OnTakeGolemKnockback(attackOriginTransform.position, golemKnockBackForce);
                enemyRef.OnRecieveEffect(new ActiveStatusEffect(new GolemKnockBackEffect(attackOriginTransform.position, golemKnockBackForce),
                new List<BaseCondition> { new GroundedCondition(true, enemyRef), new DurationCondition(true, 0.75f), new NavMeshReturnCondition(false, enemyRef) }));
            }
        }

        if (attackInterrupted)
        {
            return;
        }
        attackCompleted = true;
        ChangeState(new EnemyLookAtPlayerState(attackCooldownStat.GetFinalValue()));      
    }

    private IEnumerator ChargeTime()
    {
        AudioManager.instance.PlayRandomSoundClip(EnemyAttackChargeUpSounds);
        yield return new WaitForSeconds(meleeAttackChargeTime);
        if (attackInterrupted)
        {
            impactField.DestroyMe();
            yield break;
        }
        GolemSlam();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackOriginTransform.position, meleeAttackRadius);
    }

    public override void CompleteAttack()
    {
        StopCoroutine(charge);
        attackInterrupted = true;
        if (!attackCompleted)
        {
            impactField.DestroyMe();
        }
        
    }

    private void SpawnImpactField()
    {
        Vector3 impactFieldPosition = new Vector3(attackOriginTransform.position.x, attackOriginTransform.position.y - 1f, attackOriginTransform.position.z);

        //impactFieldObj = Instantiate(impactFieldPrefab, impactFieldPosition, Quaternion.identity);
        impactFieldObj = ObjectPoolManager.SpawnObject(impactFieldPrefab, impactFieldPosition, Quaternion.identity);

        impactField = impactFieldObj.GetComponent<EnemyAttackImpactField>();
        impactField.PassInValuesColorRadiusLifeTimeChargeTime(impactFieldColor, meleeAttackRadius * 0.9f, 2.5f, meleeAttackChargeTime);
    }   

}
