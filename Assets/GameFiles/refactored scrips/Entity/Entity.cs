using UnityEngine;
using System.Collections.Generic;

public class Entity : MonoBehaviour, IEntity, IResetable
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
        Initialize();
    }

    public virtual void Initialize()
    {
        bodySystem.InitialiseSystem(this);
        statusSystem.InitialiseSystem(this);
        healthSystem.InitialiseSystem(this);
        textDisplaySystem.InitialiseSystem(this);
    }

    public virtual void Reset()
    {
        healthSystem.ResetSystem();
        statusSystem.ResetSystem();
        bodySystem.ResetSystem();
        statusSystem.ResetSystem();
    }

    public virtual void OnTakeDamage(int amount, Color color, DamageType damageType)
    {
        int finalDamage = statusSystem.ModifyDamage(amount, damageType);
        float size = Mathf.Clamp(10 + (finalDamage * 1.1f), 48f, 240f);
        textDisplaySystem.DisplayText(finalDamage.ToString(), color,(int) 55);
        //Debug.Log("DAMAGE TAKEN: " + amount);
        healthSystem.OnTakeDamage(finalDamage, damageType);
    }
    public virtual void OnRecieveHeal(int amount, Color color)
    {
        float size = Mathf.Clamp(10 + (amount * 1.1f), 48f, 240f);
        textDisplaySystem.DisplayText(amount.ToString(), color, (int)55);
        healthSystem.OnHeal(amount);
    }
    
    public virtual void OnRecieveEffect(ActiveStatusEffect statusEffect, Color effectColour)
    {
        textDisplaySystem.DisplayHigherText(statusEffect.effect.GetEffectText(), effectColour, 52);
        statusSystem.OnRecieveEffect(statusEffect);
    }
    public virtual void OnRecieveEffect(ActiveStatusEffect statusEffect)
    {
        statusSystem.OnRecieveEffect(statusEffect);
    }

    protected virtual void Update()
    {
        statusSystem.UpdateConditions();
    }

    protected virtual void FixedUpdate()
    {
    }

    public virtual void UpdateTarget()
    {
        Collider[] hitColliders = new Collider[10];
        int numHit = Physics.OverlapSphereNonAlloc(transform.position, 100f, hitColliders, hostileMask);

        float minDist = float.MaxValue;
        GameObject newTarget = null;
        foreach (Collider collider in hitColliders)
        {
            if ((collider.transform.position - transform.position).magnitude < minDist)
            {
                minDist = (collider.transform.position - transform.position).magnitude;
                newTarget = collider.gameObject;
            }
        }
        if (newTarget == null)
        {
            Debug.LogError("No New Target Located");
            return;
        }
        target = newTarget;
    }

}
