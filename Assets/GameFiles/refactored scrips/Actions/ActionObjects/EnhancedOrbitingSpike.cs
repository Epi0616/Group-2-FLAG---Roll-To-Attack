using System.Collections.Generic;
using UnityEngine;

public class EnhancedOrbitingSpike : BaseOrbitObject
{
    private int enhancementLevel = 1;
    [SerializeField] private GameObject SpikeEntity;
    private bool hasSpawnedNewSpike = false;
    public void Initialize(Entity ownerEntity, GameObject anchorObj, float radius, float orbitSpeed, int objDamage, float lifetime, int enhancementLevel)
    {
        isDestroyed = false;
        age = 0;
        this.radius = radius;
        speed = orbitSpeed;
        this.ownerEntity = ownerEntity;
        lifeSpan = lifetime;
        this.anchorObj = anchorObj;
        tempY = anchorObj.transform.position.y + 30f;
        damage = objDamage;
        this.enhancementLevel = enhancementLevel;
        hasSpawnedNewSpike = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("StaticEntity") || target.CompareTag("PhysicsEntity")) { return; }
        if ((ownerEntity.hostileMask & (1 << target.layer)) > 0)
        {
            DamageTarget(target.GetComponent<Entity>(), other);
        }
    }

    protected void DamageTarget(Entity entity, Collider other)
    {
        //AudioManager.instance.PlayRandomSoundClip(spikeOnHitSound, new Vector3(0, 0, 0), 0.7f);
        entity.OnTakeDamage(damage, Color.silver, DamageType.Spell);
        if (age > 0.75f)
        {
            if (hasSpawnedNewSpike) { return; }
            hasSpawnedNewSpike = true;

            GameObject newSpike = ObjectPoolManager.SpawnObject(SpikeEntity, transform.position, Quaternion.identity);
            newSpike.GetComponent<EnhancedSpikeEntity>().Initialize(ownerEntity, entity, other, enhancementLevel);

            DestroyMe();
        }

    }
    protected override void CheckForExpiration()
    {
        age += Time.deltaTime;
        if (!(age >= lifeSpan) && ownerEntity != null) { return; }

        DropOff();

        
    }

    public void DropOff()
    {
        if (hasSpawnedNewSpike) { return; }
        hasSpawnedNewSpike = true;
        GameObject newSpike = ObjectPoolManager.SpawnObject(SpikeEntity, transform.position, Quaternion.identity);
        EnhancedSpikeEntity spikeEntity = newSpike.GetComponent<EnhancedSpikeEntity>();
        spikeEntity.Initialize(ownerEntity, enhancementLevel);
        spikeEntity.OnRecieveEffect(new ActiveStatusEffect(new KnockbackEffect(ownerEntity.transform.position, 1.75f),
            new List<BaseCondition> { new TimeCondition(true, 1f) },
            true),
            Color.red);
        DestroyMe();
    }
}
