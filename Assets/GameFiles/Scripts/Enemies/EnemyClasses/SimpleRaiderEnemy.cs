using UnityEngine;
using System.Collections;
using System;

public class SimpleRaiderEnemy : EnemyStateController
{
    [SerializeField] private Vector3 meleeAttackHalfExtents;
    [SerializeField] private float meleeAttackRange;
    [SerializeField] private int meleeAttackDamage;

    private bool attackInterupted;
    private bool didhitDetected;
    RaycastHit hit;

    public override void Attack()
    {
        StartCoroutine(ChargeTime());
    }

    private void MeleeAttack()
    {
        attackInterupted = false;
        //Debug.Log("Melee Enemy Attack Started");
        RaycastHit? hitCheck = SpawnMeleeAttack(meleeAttackHalfExtents, meleeAttackRange);
        if (hitCheck != null)
        {
            //Debug.Log("Melee Hit Something");
            hit = (RaycastHit)hitCheck;
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Melee Attack Hit Player");
                playerController = playerReference.GetComponent<PlayerStateController>();
                playerController.OnTakeDamage(meleeAttackDamage);
            }

        }
        if (attackInterupted)
        {
            return;
        }
        StartCoroutine(TimeBeforeMovingAfterAttack());
    }


    private IEnumerator ChargeTime()
    {
        yield return new WaitForSeconds(0.5f);
        MeleeAttack();

    }

    private IEnumerator TimeBeforeMovingAfterAttack()
    {
        yield return new WaitForSeconds(1.5f);
        ChangeState(new EnemyMoveState());
    }

    public override void CompleteAttack()
    {
        attackInterupted = true;
    }


    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (didhitDetected)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * hit.distance, meleeAttackHalfExtents);
        }
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * meleeAttackRange);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * meleeAttackRange, meleeAttackHalfExtents);
        }
    }
    */
}
