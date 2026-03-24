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
    [Header("Currently In Testing")]
    [SerializeField] private bool hasTracking;

    private float activeTimer;
    private float damageTickTimer;
    private bool attackInterrupted;
    private RaycastHit hit;

    [Header("Not to be Modified")]
    [SerializeField] private Transform firingOrigin;
    [SerializeField] private Transform laserHolder;
    [SerializeField] private GameObject laserObject;
    [SerializeField] private Transform laserParticleHolder;
    [SerializeField] private VisualEffect laserParticle;

   


    public override void Attack()
    {
        LookAtPlayer();
        
        attackInterrupted = false;
        if (hasTracking)
        {
            StartCoroutine(FireLaserTracking());
        }
        else
        {
            StartCoroutine(FireLaser());
        }
    }

    private IEnumerator FireLaser()
    {

        //laserHolder.transform.position = firingOrigin.position;

        Vector3 laserTarget = playerReference.transform.position;

        Vector3 laserDirection = playerReference.transform.position - firingOrigin.position;
        laserDirection.y = 0f;

        Ray ray = new Ray(firingOrigin.position, laserDirection);

        float distanceToEndofLaser = laserRange;

        if (Physics.Raycast(ray, out hit, laserRange, environmentLayer))
        {
            distanceToEndofLaser = hit.distance;
            laserParticle.SetVector3("EndPos", hit.point);
            //Debug.DrawLine(firingOrigin.position, hit.point, Color.green, 100f);
        }

        //laserHolder.transform.position = firingOrigin.position;
        MoveLaserCylinderParticle(laserDirection, distanceToEndofLaser, chargingWidth);

        //laserObject.SetActive(true);
        activeTimer = 0;

        AudioManager.instance.PlayRandomSoundClip(EnemyAttackChargeUpSounds);

        while (activeTimer < chargeTime)
        {
            //laserHolder.transform.position = firingOrigin.position;
            if (attackInterrupted)
            {
                yield break;
            }
            activeTimer += Time.deltaTime;
            yield return null;
        }

        //yield return new WaitForSeconds(chargeTime);

        activeTimer = 0;
        laserParticle.SetVector3("StartPos", firingOrigin.position);
        AudioManager.instance.PlayRandomSoundClip(EnemyActiveAttackSounds, default, 0.5f);
        laserParticle.enabled = true;
        laserParticle.SetFloat("Duration", laserDuration);
        laserParticle.SetVector3("StartPos", firingOrigin.position);
        laserParticle.Play();
        while (activeTimer < laserDuration && !isStunned && !attackInterrupted)
        {
            //laserHolder.transform.position = firingOrigin.position;

            activeTimer += Time.deltaTime;
            damageTickTimer += Time.deltaTime;

            if (attackInterrupted)
            {
                yield break;
            }

            LaserCheck(laserTarget, laserDirection, ray, hit);
            yield return null;
        }

        laserParticle.enabled = false;
        //laserObject.SetActive(false);
        if (!attackInterrupted)
        {
            ChangeState(new EnemyLookAtPlayerState(attackCooldownStat.GetFinalValue()));          
        }
    }

    private IEnumerator FireLaserTracking()
    {
        
        laserHolder.transform.position = firingOrigin.position;

        Vector3 laserTarget = playerReference.transform.position;
        
        Vector3 laserDirection = playerReference.transform.position - firingOrigin.position;
        laserDirection.y = 0;     

        Ray ray = new Ray(firingOrigin.position, laserDirection);
        

        float distanceToEndofLaser = laserRange;
             
        laserObject.SetActive(true);
        
        activeTimer = 0;
        while (activeTimer < chargeTime && !isStunned && !attackInterrupted)
        {
            LookAtPlayer();
            

            laserTarget = playerReference.transform.position;

            laserDirection = playerReference.transform.position - firingOrigin.position;         
            laserDirection.y = 0f;
            

            ray = new Ray(firingOrigin.position, laserDirection);
            

            distanceToEndofLaser = laserRange;

            if (Physics.Raycast(ray, out hit, laserRange, environmentLayer))
            {
                distanceToEndofLaser = hit.distance;
            }

            MoveLaserCylinder(laserDirection, distanceToEndofLaser, chargingWidth);
            activeTimer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        AudioManager.instance.PlayRandomSoundClip(EnemyActiveAttackSounds, default, 0.5f);

        activeTimer = 0;
        while (activeTimer < laserDuration && !isStunned && !attackInterrupted)
        {
            activeTimer += Time.deltaTime;
            damageTickTimer += Time.deltaTime;

            LaserCheck(laserTarget, laserDirection, ray, hit);
            yield return null;  
        }

        laserObject.SetActive(false);
        if (!attackInterrupted) 
        {
            ChangeState(new EnemyLookAtPlayerState(attackCooldownStat.GetFinalValue()));          
        }
    }

    private void MoveLaserCylinder(Vector3 laserDir, float distance, float width)
    {
        laserHolder.transform.position = firingOrigin.position;
        laserHolder.rotation = Quaternion.LookRotation(laserDir);
        Vector3 scale = laserHolder.localScale;
        scale.x = width;
        scale.y = width;
        scale.z = distance/2;     
        laserHolder.localScale = scale;     
    }

    private void MoveLaserCylinderParticle(Vector3 laserDir, float distance, float width)
    {
        //laserParticleHolder.transform.position = firingOrigin.position;
        Quaternion r = Quaternion.LookRotation(laserDir);
        laserParticleHolder.rotation = r;
        Vector3 scale = laserParticleHolder.localScale;
        scale.x = width;
        scale.y = width;
        scale.z = distance / 20;
        laserParticleHolder.localScale = scale;
    }


    private void LaserCheck(Vector3 laserTarget, Vector3 laserDir, Ray ray, RaycastHit hit)
    {
        float distanceToEndofLaser = laserRange;
        if (Physics.SphereCast(ray, 1f, out hit, laserRange, playerLayer))
        {
            
            if (hit.collider.CompareTag("Player") && damageTickTimer >= damageTickRateInSeconds)
            {
                damageTickTimer = 0f;      
                //playerController = playerReference.GetComponent<PlayerStateController>();
                playerController.healthSystem.OnTakeDamage(laserDamage/2);           
            }                     
        }
        if (Physics.Raycast(ray, out hit, laserRange, environmentLayer))
        {
            distanceToEndofLaser = hit.distance;
            Vector3 w = new Vector3(hit.point.x, hit.point.y + 200, hit.point.z);
            //Debug.DrawLine(hit.point, w, Color.red, 100f);
            //Debug.DrawLine(firingOrigin.position, hit.point, Color.green, 100f);
            laserParticle.SetVector3("EndPos", hit.point);
            //Debug.Log(hit.point);
        }

        MoveLaserCylinder(laserDir, distanceToEndofLaser, firingWidth);

    }

    public override void CompleteAttack()
    {
        StopCoroutine("FireLaser");
        attackInterrupted = true;
        laserObject.SetActive(false);
    }

}
