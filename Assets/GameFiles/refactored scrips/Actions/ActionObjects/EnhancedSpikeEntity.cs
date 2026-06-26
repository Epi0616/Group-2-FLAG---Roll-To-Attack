using UnityEngine;

public class EnhancedSpikeEntity : Entity , IUsesRigidBody, IKnockbackable
{
    protected Entity ownerEntity;
    public Entity parentEntity;
    private int numHitsTotal;
    private int numHitsLeft;
    private int enhancementLevel = 1;
    private bool embedded;
    [SerializeField] private int BaseOnHitSpikeDamage;
    [SerializeField] private int BaseTickDamage;
    private float embeddedDamageTimer;
    
    public bool isBeingDisplaced { get; set; }
    public Stat knockbackWeightMod { get; set; }
    public Stat slammedDamageMod { get; set; }

    public Rigidbody rigidbBody;
    public Rigidbody rb { get => rigidbBody; set => rigidbBody = value; }

    protected override void Start()
    {
        base.Start();
        //rb = GetComponent<Rigidbody>();
        knockbackWeightMod = new Stat(0.75f);
        slammedDamageMod = new Stat(1f);
        rb.useGravity = false;
    }

    public void Initialize(Entity ownerEntity, Entity embbeddedTarget, bool embed, int enhancementLevel)
    {

        embedded = embed;
        this.ownerEntity = ownerEntity;
        this.gameObject.layer = 14;
        numHitsTotal = 3 + enhancementLevel;
        numHitsLeft = numHitsTotal;

        if (embed)
        {
            rb.useGravity = false;
            parentEntity = embbeddedTarget;
            if (parentEntity != null)
            {
                transform.SetParent(parentEntity.transform);
            }
            else
            {
                Debug.LogWarning("Spike Attempted to Embed with no Parent");
                DestroyMe();
            }
        }
        else
        {
            DropToFloor();
        }
    }

    protected override void Update()
    {
        if (!embedded) { return; }
        embeddedDamageTimer += Time.deltaTime;
        if (embeddedDamageTimer > 1.5f)
        {
            embeddedDamageTimer = 0;
            DamageTarget(ownerEntity, BaseTickDamage + enhancementLevel, Color.darkRed);
        }
        if (parentEntity.healthSystem.isDead)
        {
            DropToFloor();
        }
    }

    public override void OnTakeDamage(int amount, Color color, DamageType damageType)
    {
        return;
    }

    public override void OnRecieveEffect(ActiveStatusEffect statusEffect, Color effectColour)
    {
        if (statusEffect.effect.type == StatusType.Knockback && !embedded)
        {
            statusSystem.OnRecieveEffect(statusEffect);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (embedded) { return; }
        GameObject target = other.gameObject;
        if (target.CompareTag("EntitySpawnable")) { return; }
        if (target.CompareTag("VacuumMine")) { return; }
        if ((ownerEntity.hostileMask & (1 << target.layer)) > 0)
        {
            DamageTarget(target.GetComponent<Entity>(), BaseOnHitSpikeDamage, Color.silver);
            numHitsLeft--;
            if (numHitsLeft <= 0)
            {
                DestroyMe();
            }
        }
    }

    private void DamageTarget(Entity entity, int damage, Color colour)
    {
        //AudioManager.instance.PlayRandomSoundClip(spikeOnHitSound, new Vector3(0, 0, 0), 0.7f);
        entity.OnTakeDamage(damage, colour, DamageType.Normal); 
    }

    protected virtual void DestroyMe()
    {
        rb.linearVelocity = Vector3.zero;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
    public void CheckForDisplacement()
    {
        isBeingDisplaced = statusSystem.CheckForDisplacementStatus();
    }

    public void DropToFloor()
    {
        transform.parent = null;
        rb.useGravity = true;
        embedded = false;
    }
}
