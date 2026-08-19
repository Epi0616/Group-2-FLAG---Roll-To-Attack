using System.Collections;
using UnityEngine;

public class FireField : MonoBehaviour
{
    [SerializeField] protected MeshRenderer ringMeshRenderer;

    protected Material material;
    protected Material ringMaterial;

    protected Entity ownerEntity;
    protected Color color;
    protected float radius = 0;
    protected int initialDamage;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        ringMaterial = ringMeshRenderer.material;
    }

    public virtual void Initialize(Entity ownerEntity, Color color, float radius, int initialDamage, int tickDamage, float lifespan, float tickRate)
    {
        this.ownerEntity = ownerEntity;
        this.radius = radius;
        this.color = color;
        this.initialDamage = initialDamage;

        this.color.a = 0.3f;
        AdjustColors();
        AdjustScale(radius);

        StartCoroutine(TickDamage(lifespan, tickRate, tickDamage));
    }

    private IEnumerator TickDamage(float lifespan, float tickRate, int tickDamage)
    {
        float tickTimer = tickRate;

        while (lifespan > 0)
        { 
            lifespan -= Time.deltaTime;
            tickTimer -= Time.deltaTime;

            if (tickTimer <= 0)
            {
                OnTickDamage(tickDamage);
                tickTimer = tickRate;
            }

            yield return null;
        }

        yield return FadeAway();
        OnEnd();
    }

    private IEnumerator FadeAway()
    {
        while (color.a > 0)
        {
            color.a -= Time.deltaTime * 0.5f;
            AdjustColors();
            yield return null;
        }

        color.a = 0;
    }

    private void OnEnd()
    { 
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    private void OnTickDamage(int tickDamage)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, ownerEntity.hostileMask);

        foreach (var collider in colliders)
        {
            if (!collider.gameObject) { continue; }
            if (collider.gameObject == ownerEntity) { continue; }
            if (collider.TryGetComponent<Entity>(out Entity entity))
            {
                entity.OnTakeDamage(tickDamage, color, DamageType.Spell);
            }
        }
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (ownerEntity == null) return;
        if (!(ownerEntity is IFireballAction fireballAction)) return;

        if ((fireballAction.targetableLayers.value & (1 << hit.gameObject.layer)) != 0)
        {
            if (!(hit.gameObject.TryGetComponent<Entity>(out Entity hitEntity))) return;
            OnInitialHit(hitEntity);
        }
    }

    private void OnInitialHit(Entity hitEntity)
    {
        hitEntity.OnTakeDamage(initialDamage, color, DamageType.Spell);
    }

    private void AdjustColors()
    {
        Mathf.Clamp01(color.a);

        Color darkerColour = new Color(color.r * 0.7f, color.g * 0.7f, color.b * 0.7f, color.a);
        Color lighterColour = new Color(color.r * 1.2f, color.g * 1.2f, color.b * 1.2f, color.a);

        material.color = darkerColour;
        ringMaterial.SetColor("_RingColour", lighterColour);
        ringMaterial.SetFloat("_Opacity", color.a);
    }

    private void AdjustScale(float radius)
    {
        Vector3 tempScale = transform.localScale;
        tempScale.x = radius * 2;
        tempScale.z = radius * 2;
        transform.localScale = tempScale;
    }
}
