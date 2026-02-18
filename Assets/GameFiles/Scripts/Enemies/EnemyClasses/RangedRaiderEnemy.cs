using UnityEngine;
using System.Collections;

public class RangedRaiderEnemy : EnemyStateController
{
    [Header("Laser Specific Variables to Be Edited")]
    [SerializeField] private float chargeTime;
    [SerializeField] private float laserDuration;
    [SerializeField] private float laserRange;
    [SerializeField] private int laserDamage;
    [SerializeField] private float damageTickRateInSeconds;
    [SerializeField] private float chargingWidth;
    [SerializeField] private float firingWidth;

    private float activeTimer;
    private float damageTickTimer;

    [Header("Not to be Modified")]
    [SerializeField] private Transform firingOrigin;
    [SerializeField] private Transform laserHolder;
    [SerializeField] private GameObject laserObject;
   


    public override void Attack()
    {
        StartCoroutine(FireLaser());
    }

    private IEnumerator FireLaser()
    {
        laserHolder.transform.position = firingOrigin.position;

        Vector3 laserTarget = playerReference.transform.position;
        Vector3 laserDirection = (playerReference.transform.position - firingOrigin.position).normalized;

        Ray ray = new Ray(firingOrigin.position, playerReference.transform.position - firingOrigin.position);
        RaycastHit hit;

        float distanceToEndofLaser = laserRange;

        if (Physics.Raycast(ray, out hit, laserRange, environmentLayer))
        {
            distanceToEndofLaser = hit.distance;
        }
       
        laserObject.SetActive(true);
        MoveLaserCylinder(laserDirection ,distanceToEndofLaser, chargingWidth);
        yield return new WaitForSeconds(chargeTime);

        activeTimer = 0;
        while (activeTimer < laserDuration && !isStunned)
        {
            activeTimer += Time.deltaTime;
            damageTickTimer += Time.deltaTime;

            LaserCheck(laserTarget, laserDirection, ray, hit);
            yield return null;  
        }

        laserObject.SetActive(false);

        ChangeState(new EnemyMoveState());
    }

    private void MoveLaserCylinder(Vector3 laserDir, float distance, float width)
    {
        Vector3 laserDirection = new Vector3(laserDir.x, laserHolder.position.y, laserDir.z);
        laserHolder.rotation = Quaternion.LookRotation(laserDir);
        Vector3 scale = laserHolder.localScale;
        scale.x = width;
        scale.y = width;
        scale.z = distance/2;
        laserHolder.localScale = scale;
    }

    private void LaserCheck(Vector3 laserTarget, Vector3 laserDir, Ray ray, RaycastHit hit)
    {
        float distanceToEndofLaser = laserRange;
        if (Physics.Raycast(ray, out hit, laserRange, playerLayer)){
            
            if (hit.collider.CompareTag("Player") && damageTickTimer >= damageTickRateInSeconds)
            {
                damageTickTimer = 0f;      
                playerController = playerReference.GetComponent<PlayerStateController>();
                playerController.OnTakeDamage(laserDamage/2);           
            }                     
        }
        if (Physics.Raycast(ray, out hit, laserRange, environmentLayer))
        {
            distanceToEndofLaser = hit.distance;
        }

        MoveLaserCylinder(laserDir, distanceToEndofLaser, firingWidth);

    }

    public override void CompleteAttack()
    {
        laserObject.SetActive(false);
        activeTimer = laserDuration;
    }

}
