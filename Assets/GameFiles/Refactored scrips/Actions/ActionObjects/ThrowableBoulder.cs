using System.Collections;
using UnityEngine;

public class ThrowableBoulder : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private int slamDamage;
    private Color slamColor;
    private float chargeTime;
    private float slamRange;

    private Entity ownerEntity;
    private ISlamActionRequirements slamVariablesAccess;
    private float chargeUpTimer = 0;
    private bool chargeComplete = false;
    private Vector3 slamOrigin;
    private bool attackInterrupted = false;
    private ImpactFieldVisual impactField;

    private void OnEnable()
    {
        rb.isKinematic = true;
        StartCoroutine(LifeTime(10));
    }

    public void HandlePathToTarget(Entity ownerEntity, Vector3 target, float durationOfTravel, int slamDamage, Color slamColor, float slamRange)
    {
        this.ownerEntity = ownerEntity;
        this.slamDamage = slamDamage;
        this.chargeTime = durationOfTravel;
        this.slamColor = slamColor;
        this.slamRange = slamRange;

        SetupSlam();
        StartCoroutine(PathToTarget(target, transform.position, durationOfTravel));
    }

    private IEnumerator LifeTime(float lifeTime)
    {
        while (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            yield return null;
        }

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    //Slam
    protected virtual void SetupSlam()
    {
        slamVariablesAccess = ownerEntity as ISlamActionRequirements;
        chargeUpTimer = 0;
        chargeComplete = false;
        attackInterrupted = false;

        //slamImpactField = slamVariablesAccess.SlamImpactField;
        // Debug.Log("SLAM STRTED");

        slamOrigin = ownerEntity.target.transform.position;

        StartCoroutine(PerformSlam(chargeTime));
    }

    private IEnumerator PerformSlam(float chargeTime)
    {
        impactField = ObjectPoolManager.SpawnObject(slamVariablesAccess.slamImpactField, slamOrigin, Quaternion.identity).GetComponent<ImpactFieldVisual>();
        impactField.PassInValuesColorRadiusChargeTimeFlash(slamColor, slamRange, chargeTime, true);

        while (chargeTime > 0)
        { 
            chargeTime -= Time.deltaTime;
            yield return null;
        }
        Slam();
    }

    public virtual void Slam()
    {
        //Debug.Log("Started Slam");
        RaycastHit hit;
        Ray ray = new Ray(slamOrigin, Vector3.down);
        if (Physics.Raycast(ray, out hit, 200f, slamVariablesAccess.groundLayer))
        {
            Collider[] colliders = Physics.OverlapSphere(hit.point, slamRange, ownerEntity.hostileMask);
            ProcessHits(colliders, hit);
        }
    }

    public virtual void ProcessHits(Collider[] colliders, RaycastHit hit)
    {
        //Debug.Log("Started Processing");
        if (ownerEntity is IJumpable wewa)
        {
            //Debug.Log("Impact Speed is: " + wewa.impactSpeed.GetFinalValue());
        }
        foreach (var collider in colliders)
        {
            if (attackInterrupted) { break; }
            if (collider == null) continue;
            if (collider.gameObject == ownerEntity.gameObject) { continue; }
            if (collider.gameObject.CompareTag("StaticEntity")) { continue; }
            Entity hitEntity = collider.gameObject.GetComponent<Entity>();
            if (hitEntity == null) { continue; }
            ApplyCustomEffectPerEntity(hitEntity);
        }

    }
    public virtual void ApplyCustomEffectPerEntity(Entity hitEntity)
    {
        if (slamDamage == 0) { return; }
        hitEntity.OnTakeDamage(slamDamage, slamColor, DamageType.Normal);
    }

    //Pathing
    private IEnumerator PathToTarget(Vector3 target, Vector3 initialPosition, float durationOfTravel)
    {
        float timer = durationOfTravel;
        float t = 0;

        Vector3 position; 

        float peakInArc = GetPeakInArc(target);
        Debug.Log(peakInArc);

        while (t < 1)
        { 
            timer -= Time.deltaTime;
            t = (durationOfTravel - timer) / durationOfTravel;

            position = Vector3.Lerp(initialPosition, target, t);
            position.y += ArcY(target, initialPosition, peakInArc, t);

            transform.position = position;

            yield return null;
        }
        transform.position = target;
    }

    private float ArcY(Vector3 target, Vector3 initialPosition, float peakInArc, float t)
    {
        return peakInArc * 6 * t * (1 - t);
    }

    private float GetPeakInArc(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        float distance = direction.magnitude;
        float peakInArc;

        Vector3 midPoint = transform.position + (direction / 2);

        if (distance != 0)
        {
            peakInArc = midPoint.y + (500 * (1 / distance));
        }
        else
        {
            peakInArc = midPoint.y + (500 * (1 / distance));
        }

        return peakInArc;
    }
}
