using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ShieldCondition : BaseCondition
{
    private bool activatingShield = false;
    private IShieldable shieldable;
    private bool isConditionMet = false;
    private Entity ownerEntity;

    [SerializeField] private float shieldDowntime;

    public ShieldCondition() { }

    public ShieldCondition(float shieldDowntime)
    { 
        this.shieldDowntime = shieldDowntime;
    }
    public override void Initialize(Entity entity)
    {
        ownerEntity = entity;
        shieldable = entity as IShieldable;
    }
    public override void ConditionUpdate()
    {
        if (activatingShield) return;

        if (!shieldable.shielded)
        { 
            activatingShield = true;
            ownerEntity.StartCoroutine(DelayedConditionMet(shieldDowntime));
        }
    }

    private IEnumerator DelayedConditionMet(float timer)
    {
        while (timer > 0)
        { 
            timer -= Time.deltaTime;
            yield return null;
        }

        isConditionMet = true;
    }

    public override void ResetCondition()
    {
        isConditionMet = false;
        activatingShield = false;
    }
    public override bool IsConditionMet()
    {
        return isConditionMet;
    }
    public override BaseCondition Clone()
    {
        return new ShieldCondition(shieldDowntime);
    }
}
