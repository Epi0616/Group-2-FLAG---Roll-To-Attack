using System.Collections.Generic;
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

    protected override void Start()
    {
        alreadyHitEntities = new HashSet<Entity>();
        transform.rotation = Quaternion.LookRotation(Vector3.up);
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
    }

    protected override void FlyUp()
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x, startHeight + 30, target.transform.position.z);
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = targetRotation;
        if (isBouncing)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 1f * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 2f * Time.deltaTime);
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

    //protected override void SelectNewTarget()
    //{
    //    Collider[] hitColliders = new Collider[10];
    //    int numHit = Physics.OverlapSphereNonAlloc(transform.position, 100f, hitColliders, ownerEntity.hostileMask);

    //    GameObject newTarget = null;
    //    if (numHit > 0)
    //    {
    //        for (int i = 0; i < numHit; i++)
    //        {
    //            if (hitColliders[i].gameObject == null) { continue; }
    //            if (hitColliders[i].gameObject.CompareTag("VacuumMine") )
    //            {
    //                continue;
    //            }
    //            foreach (Entity entity in alreadyHitEntities)
    //            {
    //                if (entity.gameObject == hitColliders[i].gameObject)
    //                {
    //                    continue;
    //                }
    //            }
    //            newTarget = hitColliders[i].gameObject;
    //        }

    //    }
    //    if (newTarget == null)
    //    {
    //        foreach (Entity entity in alreadyHitEntities)
    //        {
    //            if (entity == null) { continue; }
    //            newTarget = entity.gameObject; break;
    //        }


    //    }
    //    if (newTarget == null)
    //    {
    //        //Debug.LogWarning("No New Rocket Target Located: Destroying");
    //        DestroyMe();
    //        return;
    //    }
    //    target = newTarget;
    //}

    //protected override void SelectNewTarget()
    //{
    //    Collider[] hitColliders = new Collider[10];
    //    int numHit = Physics.OverlapSphereNonAlloc(transform.position, 100f, hitColliders, ownerEntity.hostileMask);
    //    GameObject newTarget = null;
    //    for (int i = 0; i < numHit; i++)
    //    {
    //        GameObject newObj = hitColliders[i].gameObject;

    //        if (newObj == null) { continue; }
    //        if (newObj.CompareTag("VacuumMine")) { continue; }

    //        bool hasBeenHit = false;
    //        foreach (Entity entity in alreadyHitEntities)
    //        {
    //            if (entity != null && entity.gameObject == newObj)
    //            {
    //                hasBeenHit = true;
    //                break;
    //            }
    //        }

    //        if (!hasBeenHit)
    //        {
    //            newTarget = newObj;
    //            break;
    //        }


    //    }

    //    if (newTarget == null)
    //    {
    //        for (int i = 0; i < numHit; i++)
    //        {
    //            GameObject newObj = hitColliders[i].gameObject;

    //            if (newObj == null) { continue; }
    //            if (newObj.CompareTag("VacuumMine")) { continue; }

    //            newTarget = newObj;
    //            break;
    //        }

    //    }

    //    if (newTarget == null)
    //    {
    //        //Debug.LogWarning("No New Rocket Target Located: Destroying");
    //        DestroyMe();
    //        return;
    //    }
    //    target = newTarget;
    //}

    protected override void SelectNewTarget()
    {
        Collider[] hitColliders = new Collider[10];
        int numHit = Physics.OverlapSphereNonAlloc(transform.position, 100f, hitColliders, ownerEntity.hostileMask);

        Entity closestNotHit = null;
        float closestNotHitDist = float.MaxValue;
        Entity closestEntity = null;
        float closestEntityDist = float.MaxValue;


        for (int i = 0; i < numHit; i++)
        {
            Collider collider = hitColliders[i];

            if (collider == null) { continue; }
            if (target.CompareTag("StaticEntity") || target.CompareTag("PhysicsEntity")) { continue; }

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


    protected override void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target == this.gameObject) return;
        if (target == ownerEntity.gameObject) return;

        if ((ownerEntity.hostileMask & (1 << target.layer)) > 0)
        {
            //Debug.Log("IFrame Prevented");
            if (IFrameTimer < 0.05f) {  return; }
            // Debug.Log("Target Hit");
            DamageTarget(target.GetComponent<Entity>());
            Collider[] colliders = Physics.OverlapSphere(transform.position, CurrentAoE, ownerEntity.hostileMask);
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
                searchingForTarget = false;
                numBouncesLeft--;
            }
        }
        
    }

    protected override void DamageTarget(Entity entity)
    {
        Vector3 groundedPosition = new(transform.position.x, entity.transform.position.y, transform.position.z); // needs adjusting if enemies can ever reach an elevated position.

        //Instantiate(impactFieldPrefab, groundedPosition, Quaternion.identity).GetComponent<TemporaryImpactField>().adjustObject(1f, 1f, 0.5f, 1f);
        ObjectPoolManager.SpawnObject(impactFieldPrefab, groundedPosition, Quaternion.identity).GetComponent<TemporaryImpactField>().adjustObject(CurrentAoE, 1f, 0.5f, 1f);

        entity.OnTakeDamage(10 + enhancementLevel, Color.orange, DamageType.Explosive);
        //AudioManager.instance.PlayRandomSoundClip(rocketOnHitSounds, transform.position, 0.6f);
        alreadyHitEntities.Add(entity);
        isBouncing = true;
    }
}
