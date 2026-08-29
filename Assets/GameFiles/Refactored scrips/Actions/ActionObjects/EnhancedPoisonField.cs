using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class EnhancedPoisonField : PoisonField
{
    public int enhancementLevel;
    private int currentTickCount;
    protected override void DealDamage()
    {
        if (ownerEntity == null) return;
        currentTickCount++;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, ownerEntity.hostileMask);
        if (currentTickCount > 2)
        {
            foreach (var collider in colliders)
            {
                if (!collider.gameObject) { continue; }
                if (collider.gameObject == ownerEntity) { continue; }
                if (collider.gameObject.CompareTag("EntitySpawnable")) { continue; }
                if (collider.TryGetComponent<Entity>(out Entity entity))
                {
                    //AudioManager.instance.PlayRandomSoundClip(poisonTickSound, new Vector3(0, 0, 0), 0.6f);
                    entity.OnTakeDamage(poisonTickDMG, slamColour, DamageType.Poison);

                    // hitEntity.OnRecieveEffect(new ActiveStatusEffect(new PoisonedStatus(1 * enhancementLevel, "Poisoned"),
                    // new List<BaseCondition> { new TimeCondition(false, 3) }, true));

                    entity.OnRecieveEffect(new ActiveStatusEffect(new PoisonedStatus(1 * enhancementLevel, "Poisoned"),
                    new List<BaseCondition> { new TimeCondition(true, 3) }, true));
                }
            }
            currentTickCount = 0;
        }
        else
        {
            foreach (var collider in colliders)
            {
                if (!collider.gameObject) { continue; }
                if (collider.gameObject == ownerEntity) { continue; }
                if (collider.gameObject.CompareTag("EntitySpawnable")) { continue; }
                if (collider.TryGetComponent<Entity>(out Entity entity))
                {
                    //AudioManager.instance.PlayRandomSoundClip(poisonTickSound, new Vector3(0, 0, 0), 0.6f);
                    entity.OnTakeDamage(poisonTickDMG, slamColour, DamageType.Poison);
                    entity.statusSystem.ResetStatusByType(StatusType.Poison);
                }
            }
        }
            
    }

    public void Initialize(Entity entity, float radius, float lifespan, int tickDamage, Color colour, int enhancementLevel)
    {
        //SetTiling();
        ownerEntity = entity;
        //Debug.Log("Base Radius is: " + radius);
        this.radius = radius + (enhancementLevel / 3f);
        //Debug.Log("Enhanced Radius is: " + this.radius);
        damageTickTimer = 0;
        currentTickCount = 0;
        poisonTickDMG = tickDamage;

        this.enhancementLevel = enhancementLevel;
        //Debug.Log("PoisonField Spawned with Level of: " + this.enhancementLevel);
        color = colour;
        slamColour = colour;
        this.lifeSpan = lifespan;

        lifeTimer = 0;
        //color.a = 0.175f;
        color.a = 0.3f;
        //AdjustColours(color, 1);
        AdjustDecalOpacity();

        Vector3 tempScale = transform.localScale;
        tempScale.x = radius * 2;
        tempScale.z = radius * 2;
        transform.localScale = tempScale;

        //foreach (DecalProjector p in VFXProjectors)
        //{
        //    p.size = new Vector3(radius * 2, radius * 2, p.size.z);
        //}
        VFXProjectors[0].size = new Vector3(radius * 2, radius * 2, VFXProjectors[0].size.z);
        VFXProjectors[1].size = new Vector3((radius * 2) + 5, (radius * 2) + 5, VFXProjectors[1].size.z);
        VFXProjectors[2].size = new Vector3(radius * 2, radius * 2, VFXProjectors[2].size.z);

        Vector3 position = transform.position;
        position.y -= 0.5f;
        transform.position = position;
    }
}
