using UnityEngine;
using UnityEngine.VFX;
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
    [SerializeField] private Color chargeColour;
    [SerializeField] private Color activeColour;
    [Header("Currently In Testing")]
    [SerializeField] private bool hasTracking;
    [SerializeField] private float minTurnSpeedDegrees = 60f;
    [SerializeField] private float maxTurnSpeedDegrees = 1080f;

    private float activeTimer;
    private float damageTickTimer;
    private bool attackInterrupted;
    private RaycastHit hit;

    [Header("Not to be Modified")]
    [SerializeField] private Transform firingOrigin;
    /*
    [SerializeField] private Transform laserHolder;
    [SerializeField] private GameObject laserObject;
    */
    [SerializeField] private Transform laserParticleHolder;
    [SerializeField] private VisualEffect laserParticle;

    private Coroutine Laser;

    public override void Attack()
    {
        LookAtPlayer();
        
        attackInterrupted = false;
       
        Laser = StartCoroutine(FireLaserTracking());
        
        /*
        else
        {
            StartCoroutine(FireLaser());
        }
        */
    }
    /*
    private IEnumerator FireLaser()
    {
        Vector3 laserTarget = playerReference.transform.position;

        Vector3 laserDirection = playerReference.transform.position - firingOrigin.position;
        laserDirection.y = 0f;
        laserDirection = laserDirection.normalized;

        firingOrigin.forward = laserDirection;

        Ray ray = new Ray(firingOrigin.position, laserDirection);

        float distanceToEndofLaser = laserRange;

        if (Physics.Raycast(ray, out hit, laserRange, environmentLayer))
        {
            distanceToEndofLaser = hit.distance;
            Debug.DrawLine(firingOrigin.position, hit.point, Color.blue, 10f);
            
        }

        MoveLaserParticle(ray, distanceToEndofLaser, chargingWidth);
             
        laserParticle.Reinit();
       
        laserParticle.SetVector4("Beam Colour", chargeColour);

        laserParticle.enabled = true;

        laserParticle.SetFloat("Duration", chargeTime + laserDuration);

        activeTimer = 0;

        AudioManager.instance.PlayRandomSoundClip(EnemyAttackChargeUpSounds);

        animator.speed = 0.7f;

        while (activeTimer < chargeTime)
        {
            firingOrigin.forward = laserDirection;
            ray = new Ray(firingOrigin.position, laserDirection);

            if (Physics.Raycast(ray, out hit, laserRange, environmentLayer))
            {
                distanceToEndofLaser = hit.distance;
                Debug.DrawLine(firingOrigin.position, hit.point, Color.blue, 10f);

            }
            else
            {
                distanceToEndofLaser = laserRange;
            }
            
            MoveLaserParticle(ray, distanceToEndofLaser, chargingWidth);
            if (attackInterrupted)
            {
                yield break;
            }
            activeTimer += Time.deltaTime;
            yield return null;
        }

        //yield return new WaitForSeconds(chargeTime);

        activeTimer = 0;
        
        AudioManager.instance.PlayRandomSoundClip(EnemyActiveAttackSounds, default, 0.5f);
        laserParticle.enabled = false;

        laserParticle.SetVector4("Beam Colour", activeColour);

        laserParticle.enabled = true;
        animator.speed = 1.3f;
        
        
        while (activeTimer < laserDuration && !isStunned && !attackInterrupted)
        {
            

            activeTimer += Time.deltaTime;
            damageTickTimer += Time.deltaTime;

            if (attackInterrupted)
            {
                yield break;
            }

            LaserCheck(ray, hit);
            yield return null;
        }
        
        laserParticle.Stop();
        
        if (!attackInterrupted)
        {
            ChangeState(new EnemyLookAtPlayerState(attackCooldownStat.GetFinalValue()));          
        }
    }
    */
    private IEnumerator FireLaserTracking()
    {

        laserParticle.Reinit();

        laserParticle.SetVector4("Beam Colour", chargeColour);

        laserParticle.enabled = true;
        laserParticle.SetFloat("Duration", chargeTime + laserDuration);
        
        float distanceToEndofLaser;
        Vector3 playerDirection = Vector3.zero;
        Ray ray = new Ray(firingOrigin.position, playerDirection);

        AudioManager.instance.PlayRandomSoundClip(EnemyAttackChargeUpSounds);

        activeTimer = 0;
        animator.speed = 0.5f;
        Quaternion lookRotation;
        while (activeTimer < chargeTime && !isStunned && !attackInterrupted)
        {
            //LookAtPlayer();

            playerDirection = playerReference.transform.position - firingOrigin.position;         
            playerDirection.y = 0f;
            playerDirection = playerDirection.normalized;

            lookRotation = Quaternion.LookRotation(playerDirection);
            lookRotation.z = 0f;
            lookRotation.x = 0f;

            // Determine Angle Difference between current rotation and desired rotation
            float angleDifference = Quaternion.Angle(transform.rotation, lookRotation);

            // Scale to 0-1 for time
            float t = angleDifference / 180f;
            // Use Lerp to dynamically adjust the speed depending on angleDifference
            float turnSpeedDegrees = Mathf.Lerp(minTurnSpeedDegrees, maxTurnSpeedDegrees, t);
          
            float step = turnSpeedDegrees * Time.deltaTime;
            // Actually Rotate The Beholder
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, step);

            //firingOrigin.forward = playerDirection;

            ray = new Ray(firingOrigin.position, transform.forward);

            if (Physics.SphereCast(ray, 0.04f, out hit, laserRange, environmentLayer))
            {
                distanceToEndofLaser = hit.distance;
                //Debug.DrawLine(firingOrigin.position, hit.point, Color.blue, 10f);

            }
            else
            {
                distanceToEndofLaser = laserRange;
            }
            MoveLaserParticle(ray, distanceToEndofLaser, chargingWidth);
            if (attackInterrupted)
            {
                yield break;
            }
                   
            activeTimer += Time.deltaTime;
            
            yield return null;
        }
        animator.speed = 0f;

        yield return new WaitForSeconds(0.5f);

        animator.speed = 1.5f;

        laserParticle.enabled = false;
        
        laserParticle.SetVector4("Beam Colour", activeColour);
        laserParticle.enabled = true;

        AudioManager.instance.PlayRandomSoundClip(EnemyActiveAttackSounds, default, 0.5f);

        activeTimer = 0;

        while (activeTimer < laserDuration && !isStunned && !attackInterrupted)
        {
            activeTimer += Time.deltaTime;
            damageTickTimer += Time.deltaTime;

            LaserCheck(ray, hit);

            yield return null;  
        }

        laserParticle.enabled = false;

        if (!attackInterrupted) 
        {
            ChangeState(new EnemyLookAtPlayerState(attackCooldownStat.GetFinalValue()));          
        }
    }
    /*
    private void MoveLaserCylinder(Vector3 laserDir, float distance, float width)
    {
        laserHolder.transform.position = firingOrigin.position;
        laserHolder.rotation = Quaternion.LookRotation(laserDir);
        Vector3 scale = laserHolder.localScale;
        scale.x = width;
        scale.y = width;
        scale.z = distance/2;     
        laserHolder.localScale = scale;     
    }*/

    private void MoveLaserParticle(Ray ray, float distance, float width)
    {      
        Quaternion r = Quaternion.LookRotation(transform.forward);
        laserParticleHolder.rotation = r;
        Vector3 scale = laserParticleHolder.localScale;
        scale.x = width;
        scale.y = width;
        scale.z = distance * 1.5f;
        laserParticleHolder.localScale = scale;
        
    }


    private void LaserCheck(Ray ray, RaycastHit hit)
    {
        float distanceToEndofLaser = laserRange;
        if (Physics.SphereCast(ray, 0.04f, out hit, laserRange, playerLayer))
        {
            if (hit.collider.CompareTag("Player") && damageTickTimer >= damageTickRateInSeconds)
            {
                damageTickTimer = 0f;
                playerController.healthSystem.OnTakeDamage(laserDamage / 2);
            }
                     
        }
        if (Physics.SphereCast(ray, 0.04f, out hit, laserRange, environmentLayer))
        {
            distanceToEndofLaser = hit.distance;
        }
        
        
        MoveLaserParticle(ray, distanceToEndofLaser, firingWidth);

    }

    public override void CompleteAttack()
    {
        StopCoroutine(Laser);
        attackInterrupted = true;
        laserParticle.Stop();
        laserParticle.enabled = false;
        
    }

}
