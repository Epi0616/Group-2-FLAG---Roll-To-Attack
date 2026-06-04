using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour, IEntity
{
    public List<Stat> statList {  get;  set; }

    protected EntityHealthSystem healthSystem;
    protected EntityStatusSystem statusSystem;
    protected EntityBodySystem bodySystem;
    protected TextDisplaySystem textDisplaySystem;

    public virtual void OnTakeDamage(int amount, Color color, DamageType damageType)
    {
        int finalDamage = statusSystem.ModifyDamage(amount, damageType);
        textDisplaySystem.DisplayText(finalDamage.ToString(), color, 20);

        healthSystem.OnTakeDamage(finalDamage);
    }
    public virtual void OnRecieveEffect(ActiveStatusEffect statusEffect)
    {
        textDisplaySystem.DisplayText(statusEffect.effect.GetEffectText(), Color.red, 20);

        statusSystem.OnRecieveEffect(statusEffect);
    }

    protected virtual void Update()
    {
        //statusSystem.UpdateConditions();
    }
}
