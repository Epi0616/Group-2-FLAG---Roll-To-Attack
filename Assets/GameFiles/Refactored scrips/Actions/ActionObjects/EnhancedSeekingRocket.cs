using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnhancedSeekingRocket : SeekingRocket
{
    private int enhancementLevel = 1;
    private int numBouncesTotal;
    private int numBouncesLeft;
    private float IFrameTimer = 0;
    private float BaseAoE = 1;
    private float CurrentAoE;
    private bool isBouncing = false;
    private HashSet<Entity> alreadyHitEntities;
    protected float hitCount;
    protected float expectedHits;
    protected Vector3 offset;
    [SerializeField] Color colour;

    protected override void Start()
    {
        alreadyHitEntities = new HashSet<Entity>();
        transform.rotation = Quaternion.LookRotation(Vector3.up);
        offset.x = Random.Range(-7f, 7f);
        //offset.y = Random.Range(-2f, 2f);
        offset.z = Random.Range(-7f, 7f);
    }

    public void Initialize(Entity ownerEntity, GameObject target, float startHeight, int rocketDamage, int enhancementLevel)
    {
        isDestroyed = false;
        this.ownerEntity = ownerEntity;
        this.target = target;
        this.startHeight = startHeight;
        this.enhancementLevel = enhancementLevel;
        transform.rotation = Quaternion.LookRotation(Vector3.up);
        //targetAssigned = true;
        searchingForTarget = false;
        flyingTowardsTarget = false;
        numBouncesTotal = enhancementLevel;
        numBouncesLeft = numBouncesTotal;
        CurrentAoE = BaseAoE + (enhancementLevel / 5);
        alreadyHitEntities = new HashSet<Entity>();
        alreadyHitEntities.Clear();
        isBouncing = false;
        expectedHits = enhancementLevel + 1;
        hitCount = 0;
        
    }

    protected override void FlyUp()
    {
        
        Vector3 targetPosition = new Vector3(target.transform.position.x, startHeight + 30, target.transform.position.z);
        targetPosition += offset;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = targetRotation;
        if (isBouncing)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Mathf.Clamp((1f * Time.deltaTime * enhancementLevel), 1f * Time.deltaTime, (1f * Time.deltaTime * 7)));
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Mathf.Clamp((1f * Time.deltaTime * enhancementLevel), 1f * Time.deltaTime, (1f * Time.deltaTime * 7)));
        }

        //transform.position += transform.forward * 65f * Time.deltaTime;
        if (transform.position.y >= startHeight + 25 && !isBouncing)
        {
            searchingForTarget = true;
        }
        else if (transform.position.y >= startHeight + 20 && isBouncing)
        {
            searchingForTarget = true;
        }
    }

    private void Update()
    {
        IFrameTimer += Time.deltaTime;
        if (target == null) { SelectNewTarget(); return; }
        if (!target.activeInHierarchy)
        {
            SelectNewTarget();
            return;
        }

        if (!searchingForTarget)
        {
            FlyUp();
            return;
        }

        if (!flyingTowardsTarget)
        {
            SearchForTarget();
            return;
        }

        FlyTowardsTarget();

    }

    protected override void SelectNewTarget()
    {
        Collider[] hitColliders = new Collider[40];
        int numHit = Physics.OverlapSphereNonAlloc(transform.position, 100f, hitColliders, ownerEntity.hostileMask);

        Entity closestNotHit = null;
        float closestNotHitDist = float.MaxValue;
        Entity closestEntity = null;
        float closestEntityDist = float.MaxValue;


        for (int i = 0; i < numHit; i++)
        {
            Collider collider = hitColliders[i];

            if (collider == null) { continue; }
            if (collider.gameObject.CompareTag("StaticEntity") || collider.gameObject.CompareTag("PhysicsEntity")) { continue; }

            Entity newEntity = collider.GetComponent<Entity>();

            if (newEntity == null) { continue; }   
            if (newEntity.healthSystem.isDead) { continue; }
            float dist = (newEntity.transform.position - transform.position).magnitude;

            if (dist < closestEntityDist)
            {
                closestEntity = newEntity;
                closestEntityDist = dist;
            }
            if (!alreadyHitEntities.Contains(newEntity) && dist < closestNotHitDist)
            {
                closestNotHit = newEntity;
                closestNotHitDist = dist;
            }
        }

        Entity newTarget = null;

        if (closestNotHit != null)
        {
            newTarget = closestNotHit;
        }
        else
        {
            newTarget = closestEntity;
        }

        if (newTarget == null)
        {
            //Debug.LogWarning("No New Rocket Target Located: Destroying");
            DestroyMe();
            return;
        }
        target = newTarget.gameObject;
    }

    protected virtual void SelectNewTargetAlt()
    {
        Collider[] hitColliders = new Collider[40];
        int numHit = Physics.OverlapSphereNonAlloc(transform.position, 100f, hitColliders, ownerEntity.hostileMask);
        List<Entity> possibleTargets = new List<Entity>();

        foreach (Collider collider in hitColliders) 
        {
                if (collider == null) { continue; }
                if (collider.gameObject.CompareTag("StaticEntity") || collider.gameObject.CompareTag("PhysicsEntity")) { continue; }

                Entity newEntity = collider.GetComponent<Entity>();

                if (newEntity == null) { continue; }
                if (newEntity.healthSystem.isDead) { continue; }
                possibleTargets.Add(newEntity);
        }
        if (possibleTargets.Count <= 0)
        {
                DestroyMe();
                return;
        }
        int selection = Random.Range(0, possibleTargets.Count);
        target = possibleTargets[selection].gameObject;

    }


    protected override void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target == this.gameObject) return;
        if (target == ownerEntity.gameObject) return;
        Vector3 hitPos = other.ClosestPoint(transform.position);
        if ((ownerEntity.hostileMask & (1 << target.layer)) > 0)
        {
            //Debug.Log("IFrame Prevented");
            if (IFrameTimer < 0.2f) {  return; }
            // Debug.Log("Target Hit");
            //DamageTarget(target.GetComponent<Entity>());
            Collider[] collidersArray = Physics.OverlapSphere(transform.position, CurrentAoE, ownerEntity.hostileMask);
            List<Collider> colliders = collidersArray.ToList();
            if (!colliders.Contains(other)) { colliders.Add(other); }
            //Debug.Log(colliders.Length);
            
            foreach (var collider in colliders)
            {
                
                if (!collider.gameObject) { continue; }
                if (collider.gameObject == ownerEntity) { continue; }
                if (collider.gameObject.CompareTag("EntitySpawnable")) { continue; }
                if (collider.TryGetComponent<Entity>(out Entity entity))
                {
                    //AudioManager.instance.PlayRandomSoundClip(poisonTickSound, new Vector3(0, 0, 0), 0.6f);
                    
                    DamageTarget(entity);
                    SpawnHitVFX(hitPos, entity);
                    //Debug.Log("dealing damage");
                }
            }
            IFrameTimer = 0;
            
            if (numBouncesLeft <= 0) { DestroyMe(); }
            else
            {
                //Debug.Log("Bouncing Up");
                transform.rotation = Quaternion.LookRotation(Vector3.up);
                SelectNewTarget();
                offset.x = Random.Range(-7f, 7f);
                //offset.y = Random.Range(-3f, 3f);
                offset.z = Random.Range(-7f, 7f);
                searchingForTarget = false;
                numBouncesLeft--;
            }
        }
        
    }

    protected override void DamageTarget(Entity entity)
    {
        Vector3 groundedPosition = new(transform.position.x, entity.transform.position.y, transform.position.z); ;
        if (entity.bodySystem.baseplateTransform != null)
        {
            groundedPosition = new(transform.position.x, entity.bodySystem.baseplateTransform.transform.position.y, transform.position.z);
        }
        // needs adjusting if enemies can ever reach an elevated position.
        entity.OnTakeDamage(10 + (enhancementLevel * 5), Color.lightGray, DamageType.Explosive);
        //Instantiate(impactFieldPrefab, groundedPosition, Quaternion.identity).GetComponent<TemporaryImpactField>().adjustObject(1f, 1f, 0.5f, 1f);
        //ObjectPoolManager.SpawnObject(impactFieldPrefab, groundedPosition, Quaternion.identity).GetComponent<TemporaryImpactField>().adjustObject(CurrentAoE, 0.1f, 0.5f, 1f);
        ImpactFieldVisual field = (ObjectPoolManager.SpawnObject(impactFieldPrefab, groundedPosition, Quaternion.identity)).GetComponent<ImpactFieldVisual>();
        Color fieldColour = colour;
        fieldColour.a = 0.1f;
        field.PassInValuesColorRadiusChargeTimeFlash(fieldColour, CurrentAoE, 0, false);
        hitCount++;




        AudioManager.instance.PlaySound(rocketDamageSound);
        alreadyHitEntities.Add(entity);
        isBouncing = true;
    }

    protected override void SpawnHitVFX(Vector3 hitPos, Entity hitEntity)
    {
        float hueMult = Random.Range(1, 1.5f);
        Vector3 Pos = hitPos;
        if (hitEntity.bodySystem.headTransform != null) { Pos = hitEntity.bodySystem.headTransform.position; }
        ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.Impact01), Pos, Quaternion.Euler(90, 0, 0)).
       GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> { new ColourEffectOverride(Color.grey) }));
        //ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.Sparks01), Pos, Quaternion.Euler(90, 0, 0)).
        //       GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> { new ColourEffectOverride(Color.grey), new StartSpeedEffectOverride(new rangePair(4, 5)) }));
        ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.ShardImpact01), Pos, Quaternion.Euler(90, 0, 0)).
               GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> { new ColourEffectOverride(Color.black * hueMult) }));
        ObjectPoolManager.SpawnObject(ParticleEffectDatabase.Instance.ReturnParticlePrefab(ParticleType.ShardImpact02), Pos, Quaternion.Euler(90, 0, 0)).
               GetComponent<ParticleEffectInstance>().PlayParticleEffect(new EffectSettings(new List<EffectOverride> { new ColourEffectOverride(Color.black / hueMult) }));
    }

}
