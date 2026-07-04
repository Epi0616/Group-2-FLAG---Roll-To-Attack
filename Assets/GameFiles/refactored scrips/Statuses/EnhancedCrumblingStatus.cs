using System.Collections.Generic;
using UnityEngine;

public class EnhancedCrumblingStatus : CrumblingStatus , IEnhancedStatusEffect
{
    public int enhancementLevel { get; set; }
    private Entity applierEntity;
    private Rigidbody rb;
    private HashSet<Entity> alreadyHit;

    public EnhancedCrumblingStatus(float crumbleMult, Color effectColour, Entity applierEntity, int enhancementLevel) : base(crumbleMult)
    {
        this.enhancementLevel = enhancementLevel;
        this.applierEntity = applierEntity;
        this.effectColour = effectColour;
        isStackable = false;
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

    protected override void OnUpdate()
    {
        base.OnUpdate();     
    }

    protected override void OnFixedUpdate()
    {
        Collider[] hitColliders = new Collider[100];
        int numHit = Physics.OverlapSphereNonAlloc(entityRef.transform.position, 2, hitColliders, applierEntity.hostileMask);
        for (int i = 0; i < numHit; i++)
        {
            Collider collider = hitColliders[i];
            if (collider == null) { return; }
            if (collider.CompareTag("StaticEntity")) { continue; }
            Entity hitEntity = collider.gameObject.GetComponent<Entity>();
            if (hitEntity == entityRef || alreadyHit.Contains(hitEntity)) { continue; }
            if (hitEntity == null) { continue; }
            //Debug.Log("Knockback on Entity");
            hitEntity.OnTakeDamage((int)(rb.linearVelocity.magnitude / 3), Color.white, DamageType.Explosive);
            hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new KnockbackEffect(entityRef.transform.position, rb.linearVelocity.magnitude / 20 ),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true));
            alreadyHit.Add(hitEntity);
        }
    }

    protected override void ApplyOnDamageEffects(ref Stat damage, DamageType type)
    {
        if (type == DamageType.Slammed)
        {
            Collider[] hitColliders = new Collider[100];
            int numHit = Physics.OverlapSphereNonAlloc(entityRef.transform.position, 10 + (enhancementLevel * 2), hitColliders, applierEntity.hostileMask);
            if (applierEntity is ISlamActionRequirements temp)
            {
                ImpactFieldVisual field = (ObjectPoolManager.SpawnObject(temp.slamImpactField, entityRef.transform.position, Quaternion.identity)).GetComponent<ImpactFieldVisual>();
                field.PassInValuesColorRadiusChargeTimeFlash(effectColour, 10 + (enhancementLevel * 2), 0, false);
            }
            for (int i = 0; i < numHit; i++)
            {
                Collider collider = hitColliders[i];
                if (collider == null) { return; }
                if (collider.CompareTag("StaticEntity") || collider.CompareTag("PhysicsEntity")) { continue; }
                Entity hitEntity = collider.gameObject.GetComponent<Entity>();
                if (hitEntity == entityRef) { continue; }
                if (hitEntity == null) { continue; }
                //Debug.Log("Weaken Burst");
                hitEntity.OnTakeDamage(10, Color.red, DamageType.Explosive);
            }

            toBeRemoved = true;
            return;
        }
    }
}
