using UnityEngine;

public class EnhancedSeekingRocket : SeekingRocket
{
    private int enhancementLevel = 1;
    private int numBouncesTotal;
    private int numBouncesLeft;
    private float IFrameTimer = 0;
    private float BaseAoE = 1;
    private float CurrentAoE;

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

                //AudioManager.instance.PlayRandomSoundClip(poisonTickSound, new Vector3(0, 0, 0), 0.6f);
                DamageTarget(collider.GetComponent<Entity>());
                //Debug.Log("dealing damage");

            }
            IFrameTimer = 0;
            
            if (numBouncesLeft <= 0) { DestroyMe(); }
            else
            {
                //Debug.Log("Bouncing Up");
                transform.rotation = Quaternion.LookRotation(Vector3.up);
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

        entity.OnTakeDamage(10 * enhancementLevel, Color.orange, DamageType.Explosive);
        //AudioManager.instance.PlayRandomSoundClip(rocketOnHitSounds, transform.position, 0.6f);
        
    }
}
