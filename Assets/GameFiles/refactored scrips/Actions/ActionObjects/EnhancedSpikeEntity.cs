using UnityEngine;

public class EnhancedSpikeEntity : Entity , IUsesRigidBody, IKnockbackable
{
    public Entity ownerEntity;
    public Entity parentEntity;
    private Transform anchorPoint;
    private Vector3 localPosToEmbedTarget;
    //private int numHitsTotal;
    //private int numHitsLeft;
    private int enhancementLevel = 1;
    private bool embedded;
    [SerializeField] private int BaseOnHitSpikeDamage;
    [SerializeField] private int BaseTickDamage;
    private float embeddedDamageTimer;
    public bool isDestroyed;
    public bool isBeingDisplaced { get; set; }
    public Stat knockbackWeightMod { get; set; }
    public Stat slammedDamageMod { get; set; }

    public Rigidbody rigidBody;
    public Rigidbody rb { get => rigidBody; set => rigidBody = value; }

    public Collider SpikeCollider;
    //public Collider TriggerCollider;

    protected override void Start()
    {
        base.Start();
        //rb = GetComponent<Rigidbody>();
        knockbackWeightMod = new Stat(0.5f);
        slammedDamageMod = new Stat(1f);       
    }

    public void Initialize(Entity ownerEntity, Entity embeddedTarget, Collider hitCollider, int enhancementLevel)
    {

        embedded = true;
        this.ownerEntity = ownerEntity;
        isDestroyed = false;
        //this.gameObject.layer = 14;
        //numHitsTotal = 1;
        //numHitsLeft = numHitsTotal;
        this.enhancementLevel = enhancementLevel;
        hostileMask = ownerEntity.hostileMask;
        Embed(embeddedTarget, hitCollider);            
        
    }

    public void Initialize(Entity ownerEntity, int enhancementLevel)
    {

        embedded = false;
        this.ownerEntity = ownerEntity;
        isDestroyed = false;
        //this.gameObject.layer = 14;
        //numHitsTotal = 3 + enhancementLevel;
        //numHitsLeft = numHitsTotal;
        hostileMask = ownerEntity.hostileMask;
        this.enhancementLevel = enhancementLevel;
        DropToFloor();
        
    }

    protected override void Update()
    {
        if (!embedded) { return; }
        embeddedDamageTimer += Time.deltaTime;
        if (embeddedDamageTimer > 1.5f)
        {
            embeddedDamageTimer = 0;
            DamageTarget(parentEntity, BaseTickDamage + enhancementLevel, Color.darkRed);
        }
        if (parentEntity != null)
        {
            if (parentEntity.healthSystem.isDead && embedded)
            {
                DropToFloor();
            }
        }

    }

    public void LateUpdate()
    {
        if (anchorPoint != null)
        {
            transform.position = anchorPoint.TransformPoint(localPosToEmbedTarget);
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
  
    private void OnCollisionEnter(Collision collision)
    {

        GameObject hit = collision.gameObject;
        if (hit.CompareTag("StaticEntity") || hit.CompareTag("PhysicsEntity")) { return; }
        
        if ((hostileMask & (1 << hit.gameObject.layer)) > 0)
        {
            //Debug.Log("Something Correct Hit Collision");
            DamageTarget(hit.GetComponent<Entity>(), BaseOnHitSpikeDamage, Color.silver);

            DestroyMe();

            //numHitsLeft--;
            //if (numHitsLeft <= 0)
            //{
            //    DestroyMe();
            //}
        }
        
    }

    private void DamageTarget(Entity entity, int damage, Color colour)
    {
        //AudioManager.instance.PlayRandomSoundClip(spikeOnHitSound, new Vector3(0, 0, 0), 0.7f);
        if (entity == null) { return; }
        entity.OnTakeDamage(damage, colour, DamageType.Normal); 
    }

    protected virtual void DestroyMe()
    {
        if (isDestroyed) return;
        isDestroyed = true;
        rb.linearVelocity = Vector3.zero;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
    public void CheckForDisplacement()
    {
        isBeingDisplaced = statusSystem.CheckForDisplacementStatus();
    }

    public void DropToFloor()
    {
        embedded = false;
        anchorPoint = null;
        //if (parentEntity != null) { (parentEntity.healthSystem as EnemyHealthSystem).LocalEnemyDeathEvent -= DropToFloor; }
        parentEntity = null;
        rigidBody.isKinematic = false;
        SpikeCollider.enabled = true;
    }

    public void Embed(Entity newParent, Collider other)
    {
        if (newParent == null) { Debug.Log("Invalid Spike Embed Request"); DestroyMe(); return; }

        embedded = true;

        parentEntity = newParent;
        //(parentEntity.healthSystem as EnemyHealthSystem).LocalEnemyDeathEvent += DropToFloor;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rigidBody.isKinematic = true;

        SpikeCollider.enabled = false;

        anchorPoint = other.transform;
        localPosToEmbedTarget = anchorPoint.InverseTransformPoint(transform.position);

    }
}
