using UnityEngine;
using System.Collections;

public class RangedRaiderEnemy : EnemyStateController
{
    [Header("Laser Specific Variables to Be Edited")]
    [SerializeField] private float chargeTime;
    [SerializeField] private float laserDuration;
    [SerializeField] private float laserRange;
    [SerializeField] private int laserDamage;
    private float activeTimer;
    [SerializeField] private Transform firingOrigin;
    [SerializeField] private LineRenderer lr;

    public override void Attack()
    {
        StartCoroutine(FireLaser());
    }

    private IEnumerator FireLaser()
    {
        Vector3 laserTarget = playerReference.transform.position;
        //laserTarget.y = laserTarget.y + 1f;
        Vector3 endPoint = firingOrigin.position + (laserTarget - firingOrigin.position).normalized * laserRange;

        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        
        lr.SetPosition(0, firingOrigin.position);
        lr.SetPosition(1, endPoint);

        lr.enabled = true;
        yield return new WaitForSeconds(chargeTime);

        activeTimer = 0;
        while (activeTimer < laserDuration && !isStunned)
        {
            activeTimer += Time.deltaTime;

            LaserCheck(laserTarget, endPoint);
            yield return null;  
        }

        lr.enabled = false;
        ChangeState(new EnemyMoveState());
    }
    private void LaserCheck(Vector3 laserTarget, Vector3 endPoint)
    {
        Ray ray = new Ray(firingOrigin.position, laserTarget - firingOrigin.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, laserRange, playerLayer)){
            
            endPoint = hit.point;
            playerController = playerReference.GetComponent<PlayerStateController>();
            playerController.OnTakeDamage(laserDamage);
            activeTimer = laserDuration;
        }
        lr.startWidth = 0.5f;
        lr.endWidth = 1f;

        lr.SetPosition(0, firingOrigin.position);
        lr.SetPosition(1, endPoint);
    }

    
}
