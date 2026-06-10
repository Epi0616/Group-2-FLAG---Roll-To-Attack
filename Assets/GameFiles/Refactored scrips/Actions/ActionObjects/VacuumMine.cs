using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class VacuumMine : Entity , IKnockbackable, IUsesRigidBody
{
    [SerializeField] GameObject temporaryImpactField;
    //public AudioClip[] mineSpawned;
    //public AudioClip[] mineDetonated;
    private Entity ownerEntity;
    private float timer = 2f, range = 10;
    private bool detonated = false;
    public bool isBeingDisplaced { get; set; }
    public Stat knockbackWeightMod { get; set; }
    public Stat slammedDamageMod { get; set; }

    public Rigidbody rigidbBody;
    public Rigidbody rb {  get => rigidbBody; set => rigidbBody = value; }


    protected override void Start()
    {
        base.Start();
        //rb = GetComponent<Rigidbody>();
        knockbackWeightMod = new Stat(1f);
        slammedDamageMod = new Stat(1f);
    }

    public void Initialize(Entity ownerEntity, float range, float chargeTime)
    {
        
        detonated = false;
        this.ownerEntity = ownerEntity;
        this.range = range;
        timer = chargeTime;
        this.gameObject.layer = ownerEntity.gameObject.layer;

        ShowRange();
        StartCoroutine(CountDown());
    }

    protected override void Update()
    {
        return;
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

    private void OnVacuum()
    {
        List<Entity> hitEntities = GetEntitiesInRange();

        foreach (Entity entity in hitEntities)
        {
            if (entity != null)
            {               
                entity.OnRecieveEffect(new ActiveStatusEffect(new VacuumDisplacementEffect(transform.position, -17f),
                new List<BaseCondition> { new GroundedCondition(), new TimeCondition(true, 0.75f) }, true), Color.blue);
                entity.OnTakeDamage(20, Color.blue, DamageType.Normal);
            }
        }

        DestroyMe();
    }

    private List<Entity> GetEntitiesInRange()
    {
        List<Entity> enemies = new();
        Collider[] colliders = new Collider[100];
        int collisions = Physics.OverlapSphereNonAlloc(transform.position, range, colliders, ownerEntity.hostileMask);

        for (int i = 0; i < collisions; i++)
        {
            if (!colliders[i].gameObject) { continue; }
            if (colliders[i].gameObject == ownerEntity.gameObject) { continue; }
            if (colliders[i].gameObject == this.gameObject) { continue; }
            enemies.Add(colliders[i].GetComponent<Entity>());
            
        }

        return enemies;
    }

    private void DestroyMe()
    {
        rb.linearVelocity = Vector3.zero;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    private void ShowRange()
    {
        //GameObject rangeDisplay = Instantiate(temporaryImpactField, transform.position, Quaternion.identity);
        GameObject rangeDisplay = ObjectPoolManager.SpawnObject(temporaryImpactField, transform.position, Quaternion.identity);
        rangeDisplay.transform.parent = this.transform;
        rangeDisplay.GetComponent<TemporaryImpactField>().adjustObject(range, 0.25f, 0.15f, timer);
    }

    private IEnumerator CountDown()
    {
        bool hasPlayedSFX = false;
        while (timer > 0 && !detonated)
        {
            timer -= Time.deltaTime;
            if (timer < 0.055f && !hasPlayedSFX)
            {
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
