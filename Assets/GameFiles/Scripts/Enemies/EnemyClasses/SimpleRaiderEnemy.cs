using UnityEngine;
using System.Collections;

public class SimpleRaiderEnemy : EnemyStateController
{
    [SerializeField] private Vector3 meleeAttackHalfExtents;
    [SerializeField] private float meleeAttackRange;
    [SerializeField] private int meleeAttackDamage;

    private bool didhitDetected;
    RaycastHit hit;

    public override void Attack()
    {
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
        ChangeState(new EnemyMoveState());
        //StartCoroutine(attackCooldown());
    }

    public override void CompleteAttack()
    {

    }

    /*private IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(2f);
        ChangeState(new EnemyMoveState());
    }*/

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
