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
        
    }

    public virtual void OnTakeDamage(int amount, Color color, DamageType damageType)
    {
        //int finalDamage = statusSystem.ModifyDamage(amount, damageType);
        //textDisplaySystem.DisplayText(finalDamage.ToString(), color, 20);
        Debug.Log("DAMAGE TAKEN");
        //healthSystem.OnTakeDamage(finalDamage);
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

    protected virtual void FixedUpdate()
    {

    }
}
