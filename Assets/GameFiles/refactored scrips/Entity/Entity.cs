using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour, IEntity
{
    public List<Stat> VisibleStatList = new List<Stat> ();
    public List<Stat> statList {  get => VisibleStatList;  set => VisibleStatList = value; }

    public GameObject target;
    public LayerMask hostileMask;

    public EntityHealthSystem healthSystem;
    public EntityStatusSystem statusSystem;
    public EntityBodySystem bodySystem;
    public TextDisplaySystem textDisplaySystem;

    protected virtual void Start()
    {
        //statList = new List<Stat>();       
        bodySystem.InitialiseSystem(this);
        statusSystem.InitialiseSystem(this);
        healthSystem.InitialiseSystem(this);
        textDisplaySystem.InitialiseSystem(this);
    }

    public virtual void OnTakeDamage(int amount, Color color, DamageType damageType)
    {
        int finalDamage = statusSystem.ModifyDamage(amount, damageType);
        float size = Mathf.Clamp(10 + (finalDamage * 1.1f), 48f, 240f);
        textDisplaySystem.DisplayText(finalDamage.ToString(), color,(int) size);
        //Debug.Log("DAMAGE TAKEN: " + amount);
        healthSystem.OnTakeDamage(finalDamage);
    }
    public virtual void OnRecieveEffect(ActiveStatusEffect statusEffect, Color effectColour)
    {

        textDisplaySystem.DisplayHigherText(statusEffect.effect.GetEffectText(), effectColour, 52);

        statusSystem.OnRecieveEffect(statusEffect);
    }

    protected virtual void Update()
    {
        statusSystem.UpdateConditions();
    }

    protected virtual void FixedUpdate()
    {

    }
}
