using UnityEngine;

public class PoisonField : MonoBehaviour
{
    protected Material material;
    protected Material ringMaterial;
    [SerializeField] protected MeshRenderer ringMeshRenderer;
    protected Color color;
    protected Color slamColour;
    protected float lifeSpan = 10, lifeTimer = 0;
    protected float damageTickTimer = 0;//, currentTickCount = 0;
    protected float radius = 0;
    protected Entity ownerEntity;
    protected int poisonTickDMG;
    //public AudioClip[] poisonTickSound;


    protected void Awake()
    {
        material = GetComponent<MeshRenderer>().material;      
        ringMaterial = ringMeshRenderer.material;
    }

    protected virtual void Start()
    {
        DealDamage();
    }

    protected void FixedUpdate()
    {
        TickDamage();
        BecomeTransparent();
    }

    protected void BecomeTransparent()
    {
        lifeTimer += Time.fixedDeltaTime;

        if (!(lifeTimer >= lifeSpan - 1)) { return; }
        
        AdjustColours(color);

        if (color.a > 0)
        {
            color.a += Time.fixedDeltaTime * -0.5f;
            return;
        }
        color.a = 0;

        if (!(lifeTimer >= lifeSpan)) { return; }
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    protected void AdjustColours(Color color)
    {
        Color darkerColour = new Color(color.r * 0.7f, color.g * 0.7f, color.b * 0.7f, color.a);
        Color lighterColour = new Color(color.r * 1.2f, color.g * 1.2f, color.b * 1.2f, color.a);
        material.color = darkerColour;
        ringMaterial.SetColor("_RingColour", lighterColour);
        if (color.a < 0f) { color.a = 0; }
        else if (color.a > 1f) { color.a = 1f; }
        ringMaterial.SetFloat("_Opacity", color.a);
    }

    protected virtual void TickDamage()
    {
        damageTickTimer += Time.fixedDeltaTime;
        if (!(damageTickTimer >= 1)) { return; }
        DealDamage();
        damageTickTimer = 0;
    }

    protected virtual void DealDamage()
    {
        if (ownerEntity == null) return;
        //if (!(currentTickCount < 10)) { return; }
        //currentTickCount++;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, ownerEntity.hostileMask);

        foreach (var collider in colliders)
        {
            if (!collider.gameObject) { continue; }
            if (collider.gameObject == ownerEntity) { continue; }
            //if (collider.gameObject.CompareTag("EntitySpawnable")) { continue; } 
            
            //AudioManager.instance.PlayRandomSoundClip(poisonTickSound, new Vector3(0, 0, 0), 0.6f);
            collider.gameObject.GetComponent<Entity>().OnTakeDamage(poisonTickDMG, slamColour, DamageType.Normal);
            //Debug.Log("dealing damage");
            
        }
    }

    public virtual void Initialize(Entity entity, float radius, float lifespan, int tickDamage, Color colour)
    {
        ownerEntity = entity;

        this.radius = radius;
        damageTickTimer = 0;
        //currentTickCount = 0;
        poisonTickDMG = tickDamage;

        color = colour;
        slamColour = colour;
        this.lifeSpan = lifespan;

        lifeTimer = 0;
        //color.a = 0.175f;
        color.a = 0.3f;
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
