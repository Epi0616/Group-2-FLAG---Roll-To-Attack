using UnityEngine;

public class PoisonField : MonoBehaviour
{
    private Material material;
    private Color color;
    private Color slamColour;
    private float lifeSpan = 10, lifeTimer = 0;
    private float damageTickTimer = 0, currentTickCount = 0;
    private float radius = 0;
    private Entity ownerEntity;
    private int poisonTickDMG;
    //public AudioClip[] poisonTickSound;


    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;      
    }

    private void Start()
    {
        DealDamage();
    }

    private void FixedUpdate()
    {
        TickDamage();
        BecomeTransparent();
    }

    private void BecomeTransparent()
    {
        lifeTimer += Time.fixedDeltaTime;

        if (!(lifeTimer >= lifeSpan - 1)) { return; }
        material.color = color;

        if (color.a > 0)
        {
            color.a += Time.fixedDeltaTime * -0.5f;
            return;
        }
        color.a = 0;

        if (!(lifeTimer >= lifeSpan)) { return; }
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    private void TickDamage()
    {
        damageTickTimer += Time.fixedDeltaTime;
        if (!(damageTickTimer >= 1)) { return; }
        DealDamage();
        damageTickTimer = 0;
    }

    private void DealDamage()
    {
        if (ownerEntity == null) return;
        //if (!(currentTickCount < 10)) { return; }
        //currentTickCount++;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, ownerEntity.hostileMask);

        foreach (var collider in colliders)
        {
            if (!collider.gameObject) { continue; }
            if (collider.gameObject == ownerEntity) { continue; }
            if (collider.gameObject.CompareTag("EntitySpawnable")) { continue; } 
            
            //AudioManager.instance.PlayRandomSoundClip(poisonTickSound, new Vector3(0, 0, 0), 0.6f);
            collider.gameObject.GetComponent<Entity>().OnTakeDamage(poisonTickDMG, slamColour, DamageType.Normal);
            //Debug.Log("dealing damage");
            
        }
    }

    public void Initialize(Entity entity, float radius, float lifespan, int tickDamage, Color colour)
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
        material.color = color;

        Vector3 tempScale = transform.localScale;
        tempScale.x = radius * 2;
        tempScale.z = radius * 2;
        transform.localScale = tempScale;

        Vector3 position = transform.position;
        position.y -= 0.5f;
        transform.position = position;
    }
}
