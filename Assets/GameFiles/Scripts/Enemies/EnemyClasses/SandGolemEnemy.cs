using UnityEngine;
using System.Collections;

public class SandGolemEnemy : EnemyStateController
{
    [SerializeField] private Vector3 meleeAttackHalfExtents;
    [SerializeField] private float meleeAttackRange;
    [SerializeField] private int meleeAttackDamage;

    private bool didhitDetected;
    RaycastHit hit;

    public override void Attack()
    {
        //Debug.Log("Golem Enemy Attack Started");
        RaycastHit? hitCheck = SpawnMeleeAttack(meleeAttackHalfExtents, meleeAttackRange);
        if (hitCheck != null)
        {
            //Debug.Log("Golem Hit Something");
            hit = (RaycastHit)hitCheck;
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Golem Attack Hit Player");
                playerController = playerReference.GetComponent<PlayerStateController>();
                playerController.OnTakeDamage(meleeAttackDamage);
            }

        }
        //Need to Add the knockback effect for the slam
        StartCoroutine(attackCooldown());
    }

    public override void CompleteAttack()
    {

    }

    private IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(2f);
        ChangeState(new EnemyMoveState());
    }
}
