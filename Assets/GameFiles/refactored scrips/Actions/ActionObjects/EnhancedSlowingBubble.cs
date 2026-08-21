using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EnhancedSlowingBubble : MonoBehaviour
{
    private HashSet<Entity> effectedEntities;
    private HashSet<RadialProjectile> effectedProjectiles;

    private Material material;
    private Color color;
    private Color slamColour;
    private float lifeSpan = 10, lifeTimer = 0;
    private float slowMult;
    private Entity ownerEntity;
    private int enhancementLevel;
    private Vector3 originalScale;
    private bool isDestroyed = false;

    protected void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    protected virtual void Start()
    {
        effectedEntities = new HashSet<Entity>();
        effectedProjectiles = new HashSet<RadialProjectile>();
        originalScale = transform.localScale;
    }

    protected void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifeSpan)
        {
            lifeTimer = 0;
            DestroyMe();
            
        }
    }

    protected void FixedUpdate()
    {
        foreach (Entity entity in effectedEntities)
        {
            entity.statusSystem.ResetStatusByType(StatusType.Slow);
        }
        if (!isDestroyed)
        {
            foreach (RadialProjectile projectile in effectedProjectiles.ToList())
            {
                if (projectile.gameObject.activeInHierarchy)
                {
                    projectile.speed.SetMultiplier(0.25f);
                }
                else
                {
                    effectedProjectiles.Remove(projectile);
                }
                
            }
        }
        
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.CompareTag("StaticEntity") || hit.CompareTag("PhysicsEntity")) { return; }

        if ((ownerEntity.hostileMask & (1 << hit.gameObject.layer)) > 0)
        {
            Entity hitEntity = hit.GetComponent<Entity>();
            if (hitEntity == ownerEntity) { return; }
            effectedEntities.Add(hitEntity);
            hitEntity.OnRecieveEffect(new ActiveStatusEffect(new SlowStatus(slowMult, "PlaceHolderSlow"),
                new List<BaseCondition> { new TimeCondition(true, 0.5f) }, true));
        }
        else if (hit.TryGetComponent<RadialProjectile>(out RadialProjectile projectile))
        {
            effectedProjectiles.Add(projectile);
        }   
        
    }


    public void OnTriggerExit(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.CompareTag("StaticEntity") || hit.CompareTag("PhysicsEntity")) { return; }

        if ((ownerEntity.hostileMask & (1 << hit.gameObject.layer)) > 0)
        {
            Entity hitEntity = hit.GetComponent<Entity>();
            if (hitEntity == ownerEntity) { return; }
            if (effectedEntities.Contains(hitEntity))
            {
                effectedEntities.Remove(hitEntity);
            }
        }
        else if (hit.TryGetComponent<RadialProjectile>(out RadialProjectile projectile))
        {
            effectedProjectiles.Remove(projectile);
            projectile.speed.ResetModifiers();
        }
    }

    public virtual void Initialize(Entity entity, float slowMult, float radius, float lifespan, Color colour, int enhancementLevel)
    {
        isDestroyed = false;
        ownerEntity = entity;
        this.slowMult = slowMult;
        color = colour;
        slamColour = colour;
        this.lifeSpan = lifespan + enhancementLevel;
        this.enhancementLevel = enhancementLevel;
        if (effectedEntities == null)
        {
            effectedEntities = new HashSet<Entity>();
        }
        effectedEntities.Clear();
        if (effectedProjectiles == null)
        {
            effectedProjectiles = new HashSet<RadialProjectile>();
        }
        effectedProjectiles.Clear();

        lifeTimer = 0;
        color.a = 0.5f;
        material.color = color;

        Vector3 tempScale = originalScale;
        tempScale.x = radius;
        tempScale.y = radius * 0.6f;
        tempScale.z = radius;
        transform.localScale = tempScale;

        Vector3 position = transform.position;
        position.y -= 0.2f;
        transform.position = position;
    }

    public virtual void DestroyMe()
    {
        if (isDestroyed) return;
        isDestroyed = true;
        while (color.a > 0)
        {
            color.a += Time.fixedDeltaTime * -0.2f;
        }
        color.a = 0f;
        foreach (RadialProjectile projectile in effectedProjectiles)
        {
            projectile.speed.ResetModifiers();
        }
        effectedProjectiles.Clear();
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
