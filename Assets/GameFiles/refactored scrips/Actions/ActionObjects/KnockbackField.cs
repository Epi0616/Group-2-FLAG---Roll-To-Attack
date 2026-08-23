using System.Collections.Generic;
using UnityEngine;

public class KnockbackField : MonoBehaviour
{
    private Material material;
    private Material ringMaterial;
    [SerializeField] private MeshRenderer ringRenderer;
    private Color color;
    private Color slamColour;
    private float lifeSpan = 10, lifeTimer = 0;
    private float CrumblingDamageMod;
    private Entity ownerEntity;
    private int enhancementLevel;
    private HashSet<Entity> alreadyHit;
    private float hitTimer = 0;
    private bool isDestroyed;

    protected void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        ringMaterial = ringRenderer.material;
    }

    protected virtual void Start()
    {
        alreadyHit = new HashSet<Entity>();
    }

    protected void Update()
    {
        hitTimer += Time.deltaTime;
        if (hitTimer > 0.15)
        {
            alreadyHit.Clear();
            hitTimer = 0;
        }
    }

    protected void FixedUpdate()
    {
        BecomeTransparent();
        if (ownerEntity != null)
        {
            transform.position = ownerEntity.transform.position;
        }
        
    }

    protected void BecomeTransparent()
    {
        lifeTimer += Time.fixedDeltaTime;

        if (!(lifeTimer >= lifeSpan - 1)) { return; }
        AdjustColours(color);

        if (color.a > 0)
        {
            color.a += Time.fixedDeltaTime * -1f;
            return;
        }
        color.a = 0;

        if (!(lifeTimer >= lifeSpan)) { return; }
        DestroyMe();
    }

    public void DestroyMe()
    {
        if (isDestroyed) return;
        isDestroyed = true;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    protected void AdjustColours(Color color)
    {
        Color darkerColour = new Color(color.r * 0.7f, color.g * 0.7f, color.b * 0.7f, color.a);
        Color lighterColour = new Color(color.r * 1.2f, color.g * 1.2f, color.b * 1.2f, color.a);
        material.color = darkerColour;
        ringMaterial.SetColor("_RingColour", color);
        if (color.a < 0f) { color.a = 0; }
        else if (color.a > 1f) { color.a = 1f; }
        ringMaterial.SetFloat("_Opacity", color.a);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.CompareTag("StaticEntity")) { return; }

        if ((ownerEntity.hostileMask & (1 << hit.gameObject.layer)) > 0)
        {
            Entity hitEntity = hit.GetComponent<Entity>();
            if (hitEntity == ownerEntity || alreadyHit.Contains(hitEntity)) { return; }
            //Debug.Log("Something Correct Hit Collision");
            ApplyKnockback(hitEntity);
            alreadyHit.Add(hitEntity);
        }
    }

    protected virtual void ApplyKnockback(Entity hitEntity)
    {
        if (hitEntity == null) return;
        hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new KnockbackEffect(transform.position, 7f + enhancementLevel),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true));
        hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new EnhancedCrumblingStatus(CrumblingDamageMod, slamColour, ownerEntity, enhancementLevel),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true));
        hitEntity.OnRecieveEffect(
            new ActiveStatusEffect(new HordeCollisionStatus(ownerEntity),
            new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) },
            true));
    }

    public virtual void Initialize(Entity entity, float crumblingDamageMod, float radius, float lifespan, Color colour, int enhancementLevel)
    {
        ownerEntity = entity;
        isDestroyed = false;
        //this.gameObject.layer = ownerEntity.gameObject.layer;
        CrumblingDamageMod = crumblingDamageMod;
        color = colour;
        slamColour = colour;
        this.lifeSpan = lifespan;
        this.enhancementLevel = enhancementLevel;
        if (alreadyHit == null)
        {
            alreadyHit = new HashSet<Entity>();
        }
        alreadyHit.Clear();

        lifeTimer = 0;
        color.a = 0.5f;
        AdjustColours(color);

        Vector3 tempScale = transform.localScale;
        tempScale.x = radius * 2;
        tempScale.z = radius * 2;
        transform.localScale = tempScale;

        Vector3 position = transform.position;
        position.y -= 0.5f;
        transform.position = position;
    }
}
