using TMPro;
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization;

public abstract class EnemyStateController : MonoBehaviour
{
    // code added by matt to show damage text
    public GameObject playerReference, damageText;
    public Camera cameraReference;

    [Header("Variables that can be changed")]
    [SerializeField] protected int maxHealth;
    public Stat moveSpeedStat;
    public Stat stunTimeStat;
    public Stat damageTakenModifierStat;
    public Stat knockbackWeightModifierStat;
    public Stat wallSlamDamageModifierStat;
    public Stat attackCooldownStat;

    protected PlayerStateController playerController;

    [Header("Variables not to be Adjusted")]
    public float attackRange;
    [SerializeField] protected int currentHealth;
    public bool hasSpawnVibration;
    public NavMeshAgent enemyAgent;
    public Rigidbody rb;
    public Animator animator;
    private EnemyBaseState currentState;

    public GameObject enemyModel;

    public bool isVibrating;
    public LayerMask playerLayer;
    public LayerMask environmentLayer;
    public Vector3 newSpawnPos;

    public bool canMove = true;
    public bool canAttack = true;
    public bool isBeingDisplaced = false;
    public bool isAIDisabled = false;
    public bool isSpawning;
    public bool isDead;
    public static event Action EnemyHasDied;

    protected float vibrationDuration;
    protected Vector3 initialPosition;
    public float vibrateSpeed = 50f;
    public float vibrateIntensity = 0.1f;


    [Header("Enemy General Sound Effects")]
    public AudioClip[] EnemyHurtSounds;
    public AudioClip[] EnemyWalkSounds;
    public AudioClip[] EnemyWallSlamSounds;
    public AudioClip[] EnemyShatteredSounds;
    public AudioClip[] EnemySpawnSounds;
    public AudioClip[] EnemyDeathSounds;

    [Header("Enemy Attack Sound Effects")]
    public AudioClip[] EnemyAttackChargeUpSounds;
    public AudioClip[] EnemyActiveAttackSounds;

    [Header("Localization for damage text")]

    public LocalizedString slammedText, shatteredText;

    private Stat[] stats;
    // private List<StatusEffect> currentStatusEffects = new List<StatusEffect>();
    private List<ActiveStatusEffect> currentStatusEffects = new List<ActiveStatusEffect>();

    private void Awake()
    {
        stats = new Stat[]
        {
            moveSpeedStat,
            stunTimeStat,
            damageTakenModifierStat,
            knockbackWeightModifierStat,
            wallSlamDamageModifierStat,
            attackCooldownStat,
        };
    }

    //public void Initialize()
    //{
    //    currentHealth = maxHealth;

    //    enemyAgent.speed = moveSpeedStat.GetFinalValue() * 2;
    //    enemyAgent.stoppingDistance = attackRange;
    //    enemyAgent.acceleration = moveSpeedStat.GetFinalValue() * 5;

    //    playerController = playerReference.GetComponent<PlayerStateController>();
    //    isDead = false;
    //    currentStatusEffects.Clear();

    //    transform.position = newSpawnPos;

    //    if (hasSpawnVibration)
    //    {
    //        ChangeState(new VibratingSpawnState());
    //    }
    //    else
    //    {
    //        ChangeState(new EnemyMoveState());
    //    }
    //}

    public void Initialize()
    {
        currentHealth = maxHealth;
        isDead = false;

        animator.speed = 1f;

        animator.Rebind();
        animator.Play("Walk", 0, 0);
        animator.Update(0f);

        currentStatusEffects.Clear();
        RecalculateStats();

        EnableAI();

        enemyAgent.speed = moveSpeedStat.GetFinalValue() * 2;
        enemyAgent.stoppingDistance = attackRange;
        enemyAgent.acceleration = moveSpeedStat.GetFinalValue() * 5;
        enemyAgent.autoRepath = false;

        playerController = playerReference.GetComponent<PlayerStateController>();
        if (this is RangedRaiderEnemy)
        {
            //AudioManager.instance.PlayRandomSoundClip(EnemySpawnSounds, default, 0.3f);
        }
        else
        {
            //AudioManager.instance.PlayRandomSoundClip(EnemySpawnSounds, default, 0.6f);
        }


        if (hasSpawnVibration)
            ChangeState(new VibratingSpawnState());
        else
            ChangeState(new EnemyMoveState());

    }


    protected virtual void Update()
    {
        if (isDead) return;

        currentState?.UpdateState();

        //UpdateActiveEffects();

    }
    protected virtual void FixedUpdate()
    {
        //CheckAllStatsCondition();
        //Debug.Log(rb.linearVelocity);
        if (isDead) return;
        //Vibrate()



        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(new Vector3(0, -2.0f, 0), ForceMode.Impulse);
        }

        currentState?.FixedUpdateState();
    }

    public void ChangeState(EnemyBaseState newState)
    {
        //Debug.Log("State Change");
        if (isSpawning)
        {
            return;
        }
        currentState?.ExitState();
        currentState = newState;
        //Debug.Log("Entered State: " + currentState);
        currentState.EnterState(this);
    }

    public void OnTakeDamage(int amount)
    {
        int finalDamage = Mathf.FloorToInt(amount * damageTakenModifierStat.GetFinalValue());
        currentHealth -= finalDamage;

        RunTimeStatTracker.totalDamageDealt += finalDamage;

        //AudioManager.instance.PlayRandomSoundClip(EnemyHurtSounds);
        ShowDamage(finalDamage);

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public void OnTakeDamage(int amount, Color color)
    {
        int finalDamage = Mathf.FloorToInt(amount * damageTakenModifierStat.GetFinalValue());
        currentHealth -= finalDamage;

        RunTimeStatTracker.totalDamageDealt += finalDamage;

        //AudioManager.instance.PlayRandomSoundClip(EnemyHurtSounds, default, 0.4f);

        ShowDamage(finalDamage, color);

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public void AdjustScaledHealth(float multiplier)
    {
        maxHealth = (int)((float)maxHealth * multiplier);
        currentHealth = maxHealth;
    }


    public abstract void Attack();
    public abstract void CompleteAttack();

    public void CheckAllStatsCondition()
    {
        if (!(currentStatusEffects.Count > 0)) { return; }
        for (int i = currentStatusEffects.Count - 1; i >= 0; i--)
        {
            Debug.Log(currentStatusEffects[i].effect.type.ToString() + ": Condition Count: " + currentStatusEffects[i].conditions.Count + " isExpiredTotal: " + currentStatusEffects[i].CheckForExpiration());
            foreach (BaseCondition condition in currentStatusEffects[i].conditions)
            {
               // Debug.Log(condition.name + " IsExpired?: " + condition.IsConditionMet() + " isRequired?: " + condition.isRequired);
            }
        }
    }


    public void OnRecieveEffect(ActiveStatusEffect newStatus)
    {
        //Debug.Log("New Effect");
        if (!newStatus.effect.isStackable)
        {
            for (int i = currentStatusEffects.Count - 1; i >= 0; i--)
            {
                if (currentStatusEffects[i].effect.type == newStatus.effect.type)
                {
                    //Debug.Log("Exisint Non Stackable Effect Found, Reseting Condition");
                    currentStatusEffects[i].ResetConditionsAll();
                    return;
                }
            }
        }


        currentStatusEffects.Add(newStatus);
        //newStatus.effect.AddEffect(this);
        RecalculateStats();
    }

    /*
    public void OnRecieveEffect(StatusEffect newEffect, Color effectColor)
    {
        currentStatusEffects.Add(newEffect);
        ShowEffect(newEffect.GetEffectText(), effectColor);
        RecalculateStats();
    }
    */

    public void OnRecieveEffect(ActiveStatusEffect newStatus, Color effectColor)
    {
        if (!newStatus.effect.isStackable)
        {
            for (int i = currentStatusEffects.Count - 1; i >= 0; i--)
            {
                if (currentStatusEffects[i].effect.type == newStatus.effect.type)
                {
                    //Debug.Log("Exisint Non Stackable Effect Found, Reseting Condition");
                    currentStatusEffects[i].ResetConditionsAll();
                    return;
                }
            }
        }

        currentStatusEffects.Add(newStatus);
        //newStatus.effect.AddEffect(this);
        ShowEffect(newStatus.effect.GetEffectText(), effectColor);
        RecalculateStats();
    }
    /*
    private void UpdateEffects()
    {
        for (int i = currentStatusEffects.Count - 1; i >= 0; i--)
        {
            currentStatusEffects[i].TimerUpdate();

            if (currentStatusEffects[i].IsExpired())
            {
                currentStatusEffects.RemoveAt(i);
                RecalculateStats();
            }
        }
    }
    */

    //private void UpdateActiveEffects()
    //{
    //    bool movementStopper = false;
    //    bool attackStopper = false;
    //    bool AIDisabler = false;
    //    bool displacer = false;

    //    for (int i = currentStatusEffects.Count - 1; i >= 0; i--)
    //    {
    //        currentStatusEffects[i].effect.UpdateEffect();
    //        currentStatusEffects[i].UpdateConditionsAll();
    //        //currentStatusEffects[i].condition.AdvanceTimer();

    //        if (currentStatusEffects[i].effect.preventsMovement) { movementStopper = true; }
    //        if (currentStatusEffects[i].effect.preventsAttack) { attackStopper = true; }
    //        if (currentStatusEffects[i].effect.disablesAI) { AIDisabler = true; }
    //        if (currentStatusEffects[i].effect.isDisplacing) { displacer = true; }

    //        if (currentStatusEffects[i].conditions != null && currentStatusEffects[i].CheckForExpiration())
    //        {
    //            currentStatusEffects[i].effect.RemoveEffect();
    //            currentStatusEffects.RemoveAt(i);
    //            RecalculateStats();
    //        }
    //    }

    //    canMove = !movementStopper;
    //    canAttack = !attackStopper;
    //    isBeingDisplaced = displacer;

    //    if (AIDisabler && !isAIDisabled)
    //    {
    //        DisableAI();
    //    }
    //    else if (!AIDisabler && isAIDisabled)
    //    {
    //        EnableAI();
    //    }
    //}

    private void RecalculateStats()
    {
        foreach (var stat in stats)
        {
            stat.ResetModifiers();
        }

        foreach (var status in currentStatusEffects)
        {
            status.effect.ApplyStatModifierUpdates();

        }

        //Debug.Log("current speed = " + moveSpeedStat.GetFinalValue());
        enemyAgent.speed = moveSpeedStat.GetFinalValue() * 2;
        enemyAgent.acceleration = moveSpeedStat.GetFinalValue() * 5;
    }

    private void RemoveEffectByType(StatusType type)
    {
        for (int i = currentStatusEffects.Count - 1; i >= 0; i--)
        {
            if (currentStatusEffects[i].effect.type == type)
            {
                currentStatusEffects[i].effect.RemoveEffect();
                currentStatusEffects.RemoveAt(i);
                //Debug.Log("Status Removed: " + type.ToString());
            }
        }
        RecalculateStats();
    }

    public void EnableAI()
    {
        rb.isKinematic = false;
        rb.linearDamping = 0f;

        rb.useGravity = false;
        rb.isKinematic = true;

        isAIDisabled = false;

        //enemyAgent.enabled = true;      
        enemyAgent.updatePosition = true;
        enemyAgent.updateRotation = true;

        enemyAgent.Warp(transform.position);
        enemyAgent.ResetPath();
        animator.speed = 1f;
    }

    public void DisableAI()
    {
        //enemyAgent.enabled = false;
        enemyAgent.updatePosition = false;
        enemyAgent.updateRotation = false;

        isAIDisabled = true;

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.linearDamping = 3f;
        animator.speed = 0f;
    }
    /*
    public bool IsGrounded()
    {
        NavMeshHit hit;
        return (NavMesh.SamplePosition(transform.position, out hit, 0.3f, NavMesh.AllAreas));
    }
    */
    public bool IsGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        return (Physics.Raycast(ray, out hit, 1.3f, environmentLayer));
    }


    public virtual void OnTakeKnockback(Vector3 origin, float knockbackForce)
    {
        ChangeState(new EnemyKnockbackState(origin, knockbackForce));
    }

    public virtual void OnTakeGolemKnockback(Vector3 origin, float knockbackForce)
    {
        ChangeState(new EnemyGolemKnockbackState(origin, knockbackForce));
    }

    public virtual void OnStunned()
    {
        ChangeState(new EnemyStunnedState(stunTimeStat.GetFinalValue()));
    }

    public void StartVibrating()
    {
        //vibrationDuration = duration;
        //vibrationTimer = 0f;

        initialPosition = Vector3.zero;
        isVibrating = true;

        if (animator != null)
        {
            animator.speed = 0f;
        }
    }

    public void Vibrate()
    {
        if (!isVibrating) { return; }
        if (isBeingDisplaced) { return; }
        if (isSpawning) { return; }

        float x = Mathf.Sin(Time.time * vibrateSpeed) * vibrateIntensity;
        float z = Mathf.Sin(Time.time * vibrateSpeed) * vibrateIntensity;
        enemyModel.transform.localPosition = new Vector3(initialPosition.x + x, 0, initialPosition.z + z);
    }

    public void StopVibrating()
    {
        isVibrating = false;

        if (animator != null)
        {
            animator.speed = 1f;
        }

        if (isDead) { return; }

        enemyModel.transform.localPosition = Vector3.zero;


    }

    protected void ShowDamage(int damage)
    {
        Vector3 randomOffset = new(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(6f, 8f), UnityEngine.Random.Range(-4f, 4f));

        //GameObject damageNumber = Instantiate(damageText, rb.position + randomOffset, Quaternion.identity);
        GameObject damageNumber = ObjectPoolManager.SpawnObject(damageText, rb.position + randomOffset, Quaternion.identity);

        //damageNumber.GetComponent<FloatingDamageText>().Initialize(cameraReference);
        TextMeshPro tempTMPAccess = damageNumber.GetComponent<TextMeshPro>();
        tempTMPAccess.text = damage.ToString();
        float size = Mathf.Clamp(10 + (damage * 1.1f), 36f, 240f);
        tempTMPAccess.fontSize = size;
    }

    protected void ShowDamage(int damage, Color color)
    {
        Vector3 randomOffset = new(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(6f, 8f), UnityEngine.Random.Range(-4f, 4f));


        //GameObject damageNumber = Instantiate(damageText, rb.position + randomOffset, Quaternion.identity);
        GameObject damageNumber = ObjectPoolManager.SpawnObject(damageText, rb.position + randomOffset, Quaternion.identity);


        //damageNumber.GetComponent<FloatingDamageText>().Initialize(cameraReference);
        TextMeshPro tempTMPAccess = damageNumber.GetComponent<TextMeshPro>();
        tempTMPAccess.text = damage.ToString();

        tempTMPAccess.color = color;
        float size = Mathf.Clamp(10 + (damage * 1.1f), 48f, 240f);
        tempTMPAccess.fontSize = size;


    }

    protected void ShowEffect(string effectText)
    {
        //Debug.Log("effect applied");
        //Debug.Log(effectText);

        Vector3 randomOffset = new(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(8f, 10f), UnityEngine.Random.Range(-3f, 3f));

        //GameObject damageNumber = Instantiate(damageText, rb.position + randomOffset, Quaternion.identity);
        GameObject damageNumber = ObjectPoolManager.SpawnObject(damageText, rb.position + randomOffset, Quaternion.identity);

        //damageNumber.GetComponent<FloatingDamageText>().Initialize(cameraReference);
        TextMeshPro tempTMPAccess = damageNumber.GetComponent<TextMeshPro>();
        tempTMPAccess.text = effectText;
    }

    protected void ShowEffect(string effectText, Color color)
    {
        //Debug.Log("effect applied");
        //Debug.Log(effectText);

        Vector3 randomOffset = new(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(8f, 10f), UnityEngine.Random.Range(-3f, 3f));

        //GameObject damageNumber = Instantiate(damageText, rb.position + randomOffset, Quaternion.identity);
        GameObject damageNumber = ObjectPoolManager.SpawnObject(damageText, rb.position + randomOffset, Quaternion.identity);

        //damageNumber.GetComponent<FloatingDamageText>().Initialize(cameraReference);
        TextMeshPro tempTMPAccess = damageNumber.GetComponent<TextMeshPro>();
        tempTMPAccess.text = effectText;
        tempTMPAccess.color = color;
        tempTMPAccess.fontSize = 52f;
    }

    //public virtual void OnDeath()
    //{
    //    if (isDead) return;
    //    isDead = true;
    //    currentState?.ExitState();
    //    EnemyHasDied?.Invoke();
    //    StopVibrating();

    //    //Destroy(gameObject);
    //    ObjectPoolManager.ReturnObjectToPool(gameObject);
    //}

    public virtual void OnDeath()
    {
        if (isDead) return;
        isDead = true;

        //AudioManager.instance.PlayRandomSoundClip(EnemyDeathSounds, new Vector3(0, 0, 0), 1f);

        currentState?.ExitState();
        //StopVibrating();

        if (animator != null)
        {
            animator.SetBool("isAttacking", false);
            animator.Rebind();
            animator.Play("Walk", 0, 0);
            animator.Update(0f);
        }


        if (!isAIDisabled)
        {
            DisableAI();
            //enemyAgent.ResetPath();
        }

        EnemyHasDied?.Invoke();
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }




    // Check for Knockback wall damage
    protected void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Environment") && !collision.gameObject.CompareTag("Pedestal")) { return; }
        if (!isBeingDisplaced) { return; }
        //if (isKnockedBackByGolem) { return; }

        //Debug.Log("Wall Slam Triggered with DMG Mod of: " + Mathf.Clamp(wallSlamDamageModifierStat.GetFinalValue(), 1.0f, 2.0f));


        //OnRecieveEffect(new ActiveStatusEffect(new BaseStunEffect(), new List<BaseCondition> { new DurationCondition(true, 0.5f), new NavMeshReturnCondition(false, this) }));

        float dmgMod = Mathf.Clamp(wallSlamDamageModifierStat.GetFinalValue(), 1.0f, 2.0f);
        int appliedDamage = (int)(collision.impulse.magnitude * dmgMod);

        if (appliedDamage < 10) { return; }
        if (wallSlamDamageModifierStat.GetFinalValue() > 1.1f)
        {
            //AudioManager.instance.PlayRandomSoundClip(EnemyShatteredSounds);
            ShowEffect(shatteredText.GetLocalizedString(), Color.deepSkyBlue);
            OnTakeDamage(appliedDamage, Color.deepSkyBlue);
            RemoveEffectByType(StatusType.Freeze);
        }
        else
        {
            //AudioManager.instance.PlayRandomSoundClip(EnemyWallSlamSounds);
            ShowEffect(slammedText.GetLocalizedString(), Color.darkGoldenRod);
            OnTakeDamage(appliedDamage, Color.darkGoldenRod);
        }
        RemoveEffectByType(StatusType.Knockback);


        // Eventual VFX/SFX can go here for wall slams
        // add a check for the value of dmgMod to increase volume/size of effects


    }

    // Single Instant Look At Player - Used By Ranged Enemy when attack starts
    public void LookAtPlayer()
    {
        Vector3 playerDir = playerReference.transform.position - transform.position;
        playerDir.y = transform.position.y;
        Quaternion lookRotation = Quaternion.LookRotation(playerDir);
        lookRotation.z = 0f;
        lookRotation.x = 0f;
        transform.rotation = lookRotation;
    }
}
