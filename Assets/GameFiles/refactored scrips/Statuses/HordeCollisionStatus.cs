using System.Collections.Generic;
using UnityEngine;

public class HordeCollisionStatus : StatusEffect
{
    private LayerMask collidingLayer;
    private Rigidbody rb;
    private HashSet<Entity> alreadyHit;
    private bool EntityApplied;
    public HordeCollisionStatus(LayerMask layer)
    {
        collidingLayer = layer;   
        isStackable = false;
        EntityApplied = false;
    }
    public HordeCollisionStatus(Entity applierEntity)
    {
        collidingLayer = applierEntity.hostileMask;
        isStackable = false;
        EntityApplied = true;
    }

    protected override void OnApplication()
    {
        base.OnApplication();
        rb = (entityRef as IUsesRigidBody).rb;
        if (alreadyHit == null)
        {
            alreadyHit = new HashSet<Entity>();
        }
        alreadyHit.Clear();
    }
    protected override void OnFixedUpdate()
    {
        Collider[] hitColliders = new Collider[100];
        int numHit = Physics.OverlapSphereNonAlloc(entityRef.transform.position, 2, hitColliders, collidingLayer);
        for (int i = 0; i < numHit; i++)
        {
            Collider collider = hitColliders[i];
            if (collider == null) { return; }
            if (collider.CompareTag("StaticEntity")) { continue; }
            Entity hitEntity = collider.gameObject.GetComponent<Entity>();
            if (hitEntity == entityRef || alreadyHit.Contains(hitEntity)) { continue; }
            if (hitEntity == null) { continue; }
            //Debug.Log("Knockback on Entity");
            if (EntityApplied)
            {
                hitEntity.OnTakeDamage((int)(rb.linearVelocity.magnitude / 3), Color.white, DamageType.Normal);
            }
            
            hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new SafeKBEffect(entityRef.transform.position, rb.linearVelocity.magnitude / 15),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true));
            alreadyHit.Add(hitEntity);
        }
    }
}
