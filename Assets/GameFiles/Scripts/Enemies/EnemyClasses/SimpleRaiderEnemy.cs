using UnityEngine;
using System.Collections;
using System;

public class SimpleRaiderEnemy : EnemyStateController
{
    [Header("Raider Attack Variables")]
    [SerializeField] private float meleeAttackRadius;
    [SerializeField] private int meleeAttackDamage;
    [SerializeField] private float meleeAttackChargeTime;
    [SerializeField] private Color impactFieldColor;

    [Header("Variables not to be Adjusted")]
    [SerializeField] private Transform attackOriginTransform;
    [SerializeField] private GameObject impactFieldPrefab;

    private GameObject impactFieldObj;
    private EnemyAttackImpactField impactField;
    private bool attackInterrupted;
    private bool attackCompleted;

    private Coroutine charge;

    public override void Attack()
    {
        attackCompleted = false;
        attackInterrupted = false;
        LookAtPlayer();
        SpawnImpactField();
        charge = StartCoroutine(ChargeTime());
    }

    private void MeleeAttack()
    {
        RaycastHit hit;
        Ray ray = new Ray(attackOriginTransform.position, Vector3.down);
        bool wewa = (Physics.Raycast(ray, out hit, 10f, environmentLayer));
        Collider[] colliders = Physics.OverlapSphere(hit.point, meleeAttackRadius, playerLayer);
        AudioManager.instance.PlayRandomSoundClip(EnemyActiveAttackSounds, default, 0.4f);
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
        attackCompleted = true;
        ChangeState(new EnemyLookAtPlayerState(attackCooldownStat.GetFinalValue()));        
    }


    private IEnumerator ChargeTime()
    {
        AudioManager.instance.PlayRandomSoundClip(EnemyAttackChargeUpSounds);
        yield return new WaitForSeconds(meleeAttackChargeTime);

        if (attackInterrupted)
        {
            yield break;
        }
      
        MeleeAttack();
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
        RaycastHit hit;
        Ray ray = new Ray(attackOriginTransform.position, Vector3.down);
        bool wewa = (Physics.Raycast(ray, out hit, 10f, environmentLayer));
        //Vector3 impactFieldPosition = new Vector3(attackOriginTransform.position.x, hit.point.y, attackOriginTransform.position.z);

        //impactFieldObj = Instantiate(impactFieldPrefab, impactFieldPosition, Quaternion.identity);
        impactFieldObj = ObjectPoolManager.SpawnObject(impactFieldPrefab, hit.point, Quaternion.identity);

        impactField = impactFieldObj.GetComponent<EnemyAttackImpactField>();
        impactField.PassInValuesColorRadiusLifeTimeChargeTime(impactFieldColor, meleeAttackRadius * 0.9f, 2.5f, meleeAttackChargeTime);
    }


    /*private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackOriginTransform.position, meleeAttackRadius);
    }*/
}
