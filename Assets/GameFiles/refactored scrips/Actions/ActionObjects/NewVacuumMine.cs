using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewVacuumMine : Entity , IKnockbackable, IUsesRigidBody
{
    [SerializeField] private AnimationCurve pullCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] GameObject temporaryImpactField;
    protected ImpactFieldVisual impactfield;
    //public AudioClip[] mineSpawned;
    //public AudioClip[] mineDetonated;
    protected Entity ownerEntity;
    protected float timer = 2f, range = 10;
    protected float age = 0;
    protected float baseRange = 10;
    protected bool detonated = false;
    protected Color fieldColour;
    public float pullStrength;
    protected BlackHole blackHoleVisual;
    [SerializeField] protected GameObject BlackHolePrefab;
    public bool isBeingDisplaced { get; set; }
    public Stat knockbackWeightMod { get; set; }
    public Stat slammedDamageMod { get; set; }

    public Rigidbody rigidbBody;
    public Rigidbody rb { get => rigidbBody; set => rigidbBody = value; }

    protected override void Start()
    {
        knockbackWeightMod = new Stat(0.5f);
        slammedDamageMod = new Stat(1f);
        //Initialize();
    }

    public void InitializeMine(Entity ownerEntity, float range, float chargeTime, Color colour)
    {
        detonated = false;
        this.ownerEntity = ownerEntity;
        this.range = range;
        blackHoleVisual = ObjectPoolManager.SpawnObject(BlackHolePrefab, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<BlackHole>();
        blackHoleVisual.Initialize(range, chargeTime, this.gameObject, false);
        
        timer = chargeTime;
        age = 0;
        //this.gameObject.layer = 14;
        fieldColour = colour;
        fieldColour.a = 0.05f;
        ShowRange();
        StartCoroutine(CountDown());
    }

    protected override void Update()
    {
        return;
    }

    protected override void FixedUpdate()
    {
        PullEntitiesInRange();
    }

    public override void OnTakeDamage(int amount, Color color, DamageType damageType)
    {
        return;
    }

    public override void OnRecieveEffect(ActiveStatusEffect statusEffect, Color effectColour)
    {
        if (statusEffect.effect.type == StatusType.Knockback)
        {
            statusSystem.OnRecieveEffect(statusEffect);
        }
    }

    public override void OnRecieveEffect(ActiveStatusEffect statusEffect)
    {
        if (statusEffect.effect.type == StatusType.Knockback)
        {
            statusSystem.OnRecieveEffect(statusEffect);
        }
    }

    protected virtual void OnVacuum()
    {
        List<Entity> hitEntities = GetEntitiesInRange();

        foreach (Entity entity in hitEntities)
        {
            if (entity != null)
            {
                entity.OnRecieveEffect(new ActiveStatusEffect(new VacuumDisplacementEffect(transform.position, 10f),
                new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) }, true), fieldColour);
                entity.OnTakeDamage(20, fieldColour, DamageType.Spell);
            }
        }

        DestroyMe();
    }

    protected virtual List<Entity> GetEntitiesInRange()
    {
        List<Entity> enemies = new();
        Collider[] colliders = new Collider[100];
        int collisions = Physics.OverlapSphereNonAlloc(transform.position, range, colliders, ownerEntity.hostileMask);

        for (int i = 0; i < collisions; i++)
        {
            if (colliders[i] == null) { continue; }
            if (!colliders[i].gameObject) { continue; }
            if (colliders[i].gameObject == ownerEntity.gameObject) { continue; }
            if (colliders[i].gameObject == this.gameObject) { continue; }
            if (colliders[i].CompareTag("StaticEntity")) { continue; }
            if (!colliders[i].TryGetComponent<Entity>(out Entity entity)) { continue; }
            if (entity.healthSystem.isDead) { continue; }
            enemies.Add(entity);
        }

        return enemies;
    }

    protected virtual void PullEntitiesInRange()
    {
        List<Entity> hitEntities = GetEntitiesInRange();

        foreach (Entity entity in hitEntities)
        {
            if (entity != null)
            {
                Vector3 dir = transform.position - entity.transform.position;
                float dist = dir.magnitude;
                if (dist < 0.2f) { continue; }
                float t = 1f - (dist / range);
                float strength = pullCurve.Evaluate(t);
                Vector3 pull = dir.normalized * pullStrength * strength * Time.fixedDeltaTime;
                if (entity is INavAgent navAccess)
                {
                   //Debug.Log("Pulling");
                   navAccess.agent.Move(pull);
                }
            }
        }
    }

    protected virtual void DestroyMe()
    {
        rb.linearVelocity = Vector3.zero;
        if (impactfield != null)
        {
            impactfield.DestroyMe();
        }       
        if (blackHoleVisual  != null)
        {
            blackHoleVisual.DestroyMe(0.5f);
        }
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    protected void ShowRange()
    {
        //GameObject rangeDisplay = Instantiate(temporaryImpactField, transform.position, Quaternion.identity);
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        GameObject rangeDisplay = ObjectPoolManager.SpawnObject(temporaryImpactField, spawnPos, Quaternion.identity);
  
        rangeDisplay.transform.parent = this.transform;
        impactfield = rangeDisplay.GetComponent<ImpactFieldVisual>();
        impactfield.PassInValuesColorRadiusChargeTimeFlash(fieldColour, range, timer, false);
    }

    protected virtual IEnumerator CountDown()
    {
        bool hasPlayedSFX = false;
        while (timer > 0 && !detonated)
        {
            timer -= Time.deltaTime;
            age += Time.deltaTime;
            if (timer < 1f && !hasPlayedSFX)
            {
                blackHoleVisual.StopEmission();
                //AudioManager.instance.PlayRandomSoundClip(mineDetonated, new Vector3(0, 0, 0), 1f);
                hasPlayedSFX = true;
            }

            yield return null;
        }

        OnVacuum();
        detonated = true;
    }
    public void CheckForDisplacement()
    {
        isBeingDisplaced = statusSystem.CheckForDisplacementStatus();
    }
}
