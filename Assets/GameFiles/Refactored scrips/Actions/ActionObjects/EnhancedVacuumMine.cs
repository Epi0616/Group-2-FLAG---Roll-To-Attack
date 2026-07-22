using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancedVacuumMine : VacuumMine
{
    private int enhancementLevel = 1;
    private List<ActiveStatusEffect> heldEffects;
    private float heldDamage;
    protected override void Start()
    {
        //this entity doesnt have a health system attatched????????? idk what u wana do about it, ima just leave it for now lol :3 - matt
        base.Start();
        //rb = GetComponent<Rigidbody>();
        heldEffects = new List<ActiveStatusEffect>();
    }
    public void Initialize(Entity ownerEntity, float range, float chargeTime, int enhancementLevel)
    {
        detonated = false;
        this.ownerEntity = ownerEntity;
        this.enhancementLevel = enhancementLevel;
        this.range = range + this.enhancementLevel;
        // Potentially scale the duration of the mine
        timer = chargeTime;
        //this.gameObject.layer = 14;
        healthSystem.isDead = false;
        ShowRange();
        StartCoroutine(CountDown());
    }

    public override void OnTakeDamage(int amount, Color color, DamageType damageType)
    {
        float storeAmount = (amount / 5) / Mathf.Clamp((1 / enhancementLevel), 1, 999);
        if (storeAmount <= 0) { storeAmount = 1; } 
        heldDamage += storeAmount;
        float size = Mathf.Clamp(10 + (storeAmount * 1.1f), 48f, 240f);
        textDisplaySystem.DisplayText(storeAmount.ToString(), color, (int)size);
    }

    public override void OnRecieveEffect(ActiveStatusEffect statusEffect, Color effectColour)
    {
        if (statusEffect.effect.type == StatusType.Knockback)
        {
            statusSystem.OnRecieveEffect(statusEffect);
        }
        else
        {
            heldEffects.Add(statusEffect);
        }
    }

    protected override IEnumerator CountDown()
    {
        bool hasPlayedSFX = false;
        while (timer > 0 && !detonated)
        {
            timer -= Time.deltaTime;
            if (timer < 0.1f && !hasPlayedSFX)
            {
                healthSystem.isDead = true;
                //AudioManager.instance.PlayRandomSoundClip(mineDetonated, new Vector3(0, 0, 0), 1f);
                hasPlayedSFX = true;
            }

            yield return null;
        }

        OnVacuum();
        detonated = true;
    }

    protected override void OnVacuum()
    {
        List<Entity> hitEntities = GetEntitiesInRange();

        foreach (Entity entity in hitEntities)
        {
            if (entity != null)
            {
                entity.OnRecieveEffect(new ActiveStatusEffect(new VacuumDisplacementEffect(transform.position, -17f),
                new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) }, true), Color.blue);
                entity.OnTakeDamage((int)heldDamage + 20, Color.blue, DamageType.Normal);
                foreach (ActiveStatusEffect effect in heldEffects)
                {
                    if (effect.effect.GetEffectColour() != null)
                    {
                        entity.OnRecieveEffect(effect, effect.effect.GetEffectColour());
                    }
                    else
                    {
                        entity.OnRecieveEffect(effect);
                    }
                    
                }
            }
        }

        DestroyMe();
    }

    protected override void DestroyMe()
    {
        healthSystem.isDead = true;
        heldDamage = 0;
        heldEffects.Clear();
        rb.linearVelocity = Vector3.zero;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
