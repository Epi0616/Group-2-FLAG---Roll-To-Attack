using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewEVacuumMine : NewVacuumMine
{
    private int enhancementLevel = 1;
    private List<ActiveStatusEffect> heldEffects;
    private float heldDamage;
    protected override void Start()
    {
        base.Start();
        heldEffects = new List<ActiveStatusEffect>();
    }
    public void Initialize(Entity ownerEntity, float range, float chargeTime, Color colour, int enhancementLevel)
    {
        detonated = false;
        this.ownerEntity = ownerEntity;
        this.enhancementLevel = enhancementLevel;
        this.range = range + this.enhancementLevel;
        fieldColour = colour;
        // Potentially scale the duration of the mine
        timer = chargeTime;
        //this.gameObject.layer = 14;        
        heldDamage = 0;
        healthSystem.isDead = false;
        ShowRange();
        StartCoroutine(CountDown());
    }

    public override void OnTakeDamage(int amount, Color color, DamageType damageType)
    {
        float storeAmount = (amount / 2) / Mathf.Clamp((1 / enhancementLevel), 1, 999);
        if (storeAmount <= 0) { storeAmount = 1; }
        heldDamage += storeAmount;
        float size = Mathf.Clamp(10 + (storeAmount * 1.1f), 48f, 240f);
        textDisplaySystem.DisplayText(storeAmount.ToString(), fieldColour, (int)size);
    }

    public override void OnRecieveEffect(ActiveStatusEffect statusEffect, Color effectColour)
    {
       // Debug.Log("Hit by Effect");
        if (statusEffect.effect.type == StatusType.Knockback)
        {
            statusSystem.OnRecieveEffect(statusEffect);
        }
        else
        {
            //Debug.Log("StatusAbsorbed");
            heldEffects.Add(statusEffect);
        }
    }

    public override void OnRecieveEffect(ActiveStatusEffect statusEffect)
    {
        //Debug.Log("Hit by Effect");
        if (statusEffect.effect.type == StatusType.Knockback)
        {
            statusSystem.OnRecieveEffect(statusEffect);
        }
        else
        {
           // Debug.Log("StatusAbsorbed");
            heldEffects.Add(statusEffect);
        }
    }

    protected override void OnVacuum()
    {
        List<Entity> hitEntities = GetEntitiesInRange();

        foreach (Entity entity in hitEntities)
        {
            if (entity != null)
            {
                entity.OnRecieveEffect(new ActiveStatusEffect(new VacuumDisplacementEffect(transform.position, 10f),
                new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) }, true), fieldColour);
                entity.OnTakeDamage((int)heldDamage + 20, fieldColour, DamageType.Normal);
                foreach (ActiveStatusEffect effect in heldEffects)
                {
                    entity.OnRecieveEffect(new ActiveStatusEffect(effect.effect.Clone(), effect.conditions.Select(c => c.Clone()).ToList(), effect.allConditionsRequired));                  
                }
            }
        }

        DestroyMe();
    }

    protected override void DestroyMe()
    {
        rb.linearVelocity = Vector3.zero;
        if (impactfield != null)
        {
            impactfield.DestroyMe();
        }
        bodySystem.RemoveAllShaders();
        //Debug.Log("Effects Cleared");
        heldEffects.Clear();
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
