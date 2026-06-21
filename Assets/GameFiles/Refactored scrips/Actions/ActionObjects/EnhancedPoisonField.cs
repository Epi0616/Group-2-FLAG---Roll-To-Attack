using System.Collections.Generic;
using UnityEngine;

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

                //AudioManager.instance.PlayRandomSoundClip(poisonTickSound, new Vector3(0, 0, 0), 0.6f);
                Entity hitEntity = collider.gameObject.GetComponent<Entity>();
                hitEntity.OnTakeDamage(poisonTickDMG, slamColour, DamageType.Poison);

               // hitEntity.OnRecieveEffect(new ActiveStatusEffect(new PoisonedStatus(1 * enhancementLevel, "Poisoned"),
               // new List<BaseCondition> { new TimeCondition(false, 3) }, true));
                
                hitEntity.OnRecieveEffect(new ActiveStatusEffect(new PoisonedStatus(1 * enhancementLevel, "Poisoned"),
                new List<BaseCondition> { new TimeCondition(true, 3) }, true), slamColour);

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

                //AudioManager.instance.PlayRandomSoundClip(poisonTickSound, new Vector3(0, 0, 0), 0.6f);
                Entity hitEntity = collider.gameObject.GetComponent<Entity>();
                hitEntity.OnTakeDamage(poisonTickDMG, slamColour, DamageType.Poison);
                hitEntity.statusSystem.ResetStatusByType(StatusType.Poison);
            }
        }
            
    }

    public void Initialize(Entity entity, float radius, float lifespan, int tickDamage, Color colour, int enhancementLevel)
    {
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
        material.color = color;

        Vector3 tempScale = transform.localScale;
        tempScale.x = radius * 2;
        tempScale.z = radius * 2;
        transform.localScale = tempScale;

        Vector3 position = transform.position;
        position.y -= 0.5f;
        transform.position = position;
    }
}
