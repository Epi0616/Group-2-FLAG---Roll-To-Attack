using System.Collections.Generic;
using UnityEngine;
using System;

public class EnhancedSlowingBubble : MonoBehaviour
{
    private HashSet<Entity> effectedEntities;

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
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
