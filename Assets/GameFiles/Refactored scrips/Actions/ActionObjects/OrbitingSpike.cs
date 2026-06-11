using System;
using UnityEngine;

public class OrbitingSpike : BaseOrbitObject
{
    protected override void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("EntitySpawnable")) { return; }

        if ((ownerEntity.hostileMask & (1 << target.layer)) > 0)
        {
            DamageTarget(target.GetComponent<Entity>());
        }
    }

    protected override void DamageTarget(Entity entity)
    {
        //AudioManager.instance.PlayRandomSoundClip(spikeOnHitSound, new Vector3(0, 0, 0), 0.7f);
        entity.OnTakeDamage(damage, Color.silver, DamageType.Normal);
        if (age > 0.75f)
        {
            DestroyMe();
        }

    }
}
