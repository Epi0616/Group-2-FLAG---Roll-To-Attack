using UnityEngine;

public class SeekingRocket : MonoBehaviour 
{
    [SerializeField] protected GameObject impactFieldPrefab;
    //[SerializeField] AudioClip[] rocketOnHitSounds;
    protected GameObject target;
    protected bool searchingForTarget = false;
    protected bool flyingTowardsTarget = false;
    //protected bool targetAssigned = false;
    protected bool isDestroyed = false;
    protected float startHeight;
    private int rocketDamage;
    protected Entity ownerEntity;

    protected virtual void Start()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.up);
    }

    //void Update()
    //{
    //    if (target == null) { DestroyMe(); return; }
    //    if (!target.activeInHierarchy)
    //    {
    //        if (targetAssigned)
    //        {
    //            DestroyMe();
    //        }
    //        return;
    //    }

    //    if (!searchingForTarget)
    //    {
    //        FlyUp();
    //        return;
    //    }

    //    if (!flyingTowardsTarget)
    //    {
    //        SearchForTarget();
    //        return;
    //    }

    //    FlyTowardsTarget();

    //}

    private void Update()
    {
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

    public virtual void Initialize(Entity ownerEntity, GameObject target, float startHeight, int rocketDamage)
    {
        isDestroyed = false;
        this.ownerEntity = ownerEntity;
        this.target = target;
        this.startHeight = startHeight;
        transform.rotation = Quaternion.LookRotation(Vector3.up);
        //targetAssigned = true;
        searchingForTarget = false;
        flyingTowardsTarget = false;
        this.rocketDamage = rocketDamage;
    }

    protected void SearchForTarget()
    {
        if (target == null) { SelectNewTarget(); }
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7.5f * Time.deltaTime);

        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        if (angle < 5f)
        {
            flyingTowardsTarget = true;
        }
    }

    protected void FlyTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = targetRotation;

        transform.position += transform.forward * 100f * Time.deltaTime;
    }

    protected virtual void SelectNewTarget()
    {
        Collider[] hitColliders = new Collider[10];
        int numHit = Physics.OverlapSphereNonAlloc(transform.position, 100f, hitColliders, ownerEntity.hostileMask);
       
        GameObject newTarget = null;
        if (numHit > 0)
        {
            for (int i = 0; i < numHit; i++)
            {
                if (hitColliders[i].gameObject == null) { continue; }
                if (hitColliders[i].gameObject.CompareTag("VacuumMine"))
                {
                    continue;
                }
                newTarget = hitColliders[i].gameObject;
            }
            
        }
        if (newTarget == null)
        {
            //Debug.LogWarning("No New Rocket Target Located: Destroying");
            DestroyMe();
            return;
        }
        target = newTarget;
    }

    protected virtual void FlyUp()
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x, startHeight + 30, target.transform.position.z);
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = targetRotation;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 2f * Time.deltaTime);
        //transform.position += transform.forward * 65f * Time.deltaTime;
        if (transform.position.y >= startHeight + 25)
        {
            searchingForTarget = true;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target == this.gameObject) return;
        if (target == ownerEntity.gameObject) return;

        if ((ownerEntity.hostileMask & (1 << target.layer)) > 0)
        {
           // Debug.Log("Target Hit");
            DamageTarget(target.GetComponent<Entity>());
        }
    }

    protected virtual void DamageTarget(Entity entity)
    {
        Vector3 groundedPosition = new(transform.position.x, entity.transform.position.y, transform.position.z); // needs adjusting if enemies can ever reach an elevated position.

        //Instantiate(impactFieldPrefab, groundedPosition, Quaternion.identity).GetComponent<TemporaryImpactField>().adjustObject(1f, 1f, 0.5f, 1f);
        ObjectPoolManager.SpawnObject(impactFieldPrefab, groundedPosition, Quaternion.identity).GetComponent<TemporaryImpactField>().adjustObject(1f, 1f, 0.5f, 1f);

        entity.OnTakeDamage(rocketDamage, Color.orange, DamageType.Explosive);
        //AudioManager.instance.PlayRandomSoundClip(rocketOnHitSounds, transform.position, 0.6f);
        DestroyMe();
    }

    protected void DestroyMe()
    {
        if (isDestroyed) return;
        isDestroyed = true;
        //targetAssigned = false;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
