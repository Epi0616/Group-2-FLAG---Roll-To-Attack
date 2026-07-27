using System.Collections;
using System.Collections.Generic;
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
    public bool embedded;
    private bool invuln = false;
    [SerializeField] private int BaseOnHitSpikeDamage;
    [SerializeField] private int BaseTickDamage;
    private float embeddedDamageTimer;
    public bool isDestroyed;
    [SerializeField] private bool IsBeingDisplaced;
    public bool isBeingDisplaced { get => IsBeingDisplaced; set => IsBeingDisplaced = value; }
    public Stat knockbackWeightMod { get; set; }
    public Stat slammedDamageMod { get; set; }

    public Rigidbody rigidBody;
    public Rigidbody rb { get => rigidBody; set => rigidBody = value; }

    public Collider SpikeCollider;
    //public Collider TriggerCollider;
    private bool systemsSet = false;

    [SerializeField] private float age;
    [SerializeField] private float lifespan;
    public float numEmbedsTotal;
    public float numEmbedsLeft;

    protected override void Start()
    {
        //base.Start();
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
        this.enhancementLevel = enhancementLevel;
        SetSystems();
        numEmbedsTotal = enhancementLevel;
        numEmbedsLeft = numEmbedsTotal;
        age = 0f;
        lifespan = 10 + (enhancementLevel * 5);
        hostileMask = ownerEntity.hostileMask;
        Embed(embeddedTarget, hitCollider);            
        
    }

    private void SetSystems()
    {
        if (!systemsSet)
        {
            bodySystem.InitialiseSystem(this);
            statusSystem.InitialiseSystem(this);
            healthSystem.InitialiseSystem(this);
            textDisplaySystem.InitialiseSystem(this);
            knockbackWeightMod = new Stat(0.5f);
            slammedDamageMod = new Stat(1f);
            systemsSet = true;
        }
    }

    

    public void Initialize(Entity ownerEntity, int enhancementLevel)
    {

        embedded = false;
        this.ownerEntity = ownerEntity;
        isDestroyed = false;
        //this.gameObject.layer = 14;
        this.enhancementLevel = enhancementLevel;
        SetSystems();
        numEmbedsTotal = enhancementLevel;
        numEmbedsLeft = numEmbedsTotal;
        age = 0f;
        lifespan = 10 + (enhancementLevel * 5);
        hostileMask = ownerEntity.hostileMask;
        DropToFloor();
        
    }

    protected override void Update()
    {
        base.Update();
        CheckForDisplacement();
        if (!embedded)
        {
            age += Time.deltaTime;
            if (age >= lifespan)
            {
                //Debug.Log("Spike Expired");
                //DestroyMe();
                StartCoroutine(DespawnRoutine());
            }
            return; 
        }
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
        if (invuln) { return; }
        GameObject hit = collision.gameObject;
        if (hit.CompareTag("StaticEntity") || hit.CompareTag("PhysicsEntity")) { return; }
        
        if ((hostileMask & (1 << hit.gameObject.layer)) > 0)
        {
            Entity hitEntity = hit.GetComponent<Entity>();
            //Debug.Log("Something Correct Hit Collision");
            if (numEmbedsLeft > 0 && isBeingDisplaced)
            {
                Embed(hitEntity, collision.GetContact(0).otherCollider); return;
            }
            else
            {
                DamageTarget(hitEntity, BaseOnHitSpikeDamage, Color.silver);
            }              

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
        if (embedded) return;
        if (isDestroyed) return;
        isDestroyed = true;
        //rb.linearVelocity = Vector3.zero;       
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
    public void CheckForDisplacement()
    {
        isBeingDisplaced = statusSystem.CheckForDisplacementStatus();
    }

    public void DropToFloor()
    {
        embedded = false;
        StartCoroutine(DropOffWindow());
        anchorPoint = null;
        rigidBody.isKinematic = false;
        SpikeCollider.enabled = true;
        if (parentEntity != null) {
            OnRecieveEffect(new ActiveStatusEffect(new KnockbackEffect(parentEntity.transform.position, 2.75f),
            new List<BaseCondition> {new TimeCondition(true, 1f) },
            true),
            Color.red);
        }
        parentEntity = null;
        
    }

    public void Embed(Entity newParent, Collider other)
    {
        if (newParent == null) { Debug.Log("Invalid Spike Embed Request"); DestroyMe(); return; }

        embedded = true;
        numEmbedsLeft--;
        //Debug.Log("Embedding");
        age = 0;

        parentEntity = newParent;
        //(parentEntity.healthSystem as EnemyHealthSystem).LocalEnemyDeathEvent += DropToFloor;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rigidBody.isKinematic = true;

        SpikeCollider.enabled = false;

        anchorPoint = other.transform;
        localPosToEmbedTarget = anchorPoint.InverseTransformPoint(transform.position);

    }

    public IEnumerator DespawnRoutine()
    {
        float despawnTimer = 0;
        while (despawnTimer < 0.5f)
        {
            despawnTimer += Time.deltaTime;
            transform.position = new Vector3(transform.position.x , transform.position.y - 0.001f , transform.position.z);   
            yield return null;
        }
        DestroyMe();
    }

    public IEnumerator DropOffWindow()
    {
        invuln = true;
        float Timer = 0;
        while (Timer < 0.2f)
        {
            Timer += Time.deltaTime;
            
            yield return null;
        }
        invuln = false;
    }
}
