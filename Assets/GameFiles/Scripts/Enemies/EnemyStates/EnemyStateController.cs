using TMPro;
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyStateController : MonoBehaviour
{
    // code added by matt to show damage text
    public GameObject playerReference,damageText;

    [Header("Variables that can be changed")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int currentHealth;  
    public Stat moveSpeedStat;
    public Stat stunTimeStat;
    public Stat damageTakenModifierStat;
    public Stat knockbackWeightModifierStat;
    public Stat wallSlamDamageModifierStat;
    public Stat attackCooldownStat;
   
    protected PlayerStateController playerController;

    [Header("Variables not to be Adjusted")]
    public float attackRange;
    public bool hasSpawnVibration;
    public NavMeshAgent enemyAgent;
    public Rigidbody rb;
    public Animator animator;
    private EnemyBaseState currentState;
    public bool isStunned;
    public bool isKnockedBack;
    public bool isKnockedBackByGolem;
    public bool isVibrating;
    public bool isFragile;
    public LayerMask playerLayer;
    public LayerMask environmentLayer;
    
    public bool isSpawning;
    private bool isDead;
    public static event Action EnemyHasDied;

    protected float vibrationDuration;
    protected Vector3 initialPosition;
    public float vibrateSpeed = 50f;
    public float vibrateIntensity = 0.1f;
    private float vibrationTimer = 0f;

    private Stat[] stats;
    private List<StatusEffect> currentStatusEffects = new List<StatusEffect>();

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

    protected void Start()
    {
        currentHealth = maxHealth;

        enemyAgent.speed = moveSpeedStat.GetFinalValue() * 2;
        enemyAgent.stoppingDistance = attackRange;
        enemyAgent.acceleration = moveSpeedStat.GetFinalValue() * 5;
        
        playerController = playerReference.GetComponent<PlayerStateController>();

        if (hasSpawnVibration)
        {
            ChangeState(new VibratingSpawnState());
        }
        else
        {
            ChangeState(new EnemyMoveState());
        }
            
    }

    protected virtual void Update()
    {
        if (isDead) return;

        currentState?.UpdateState();
        UpdateEffects();
    }
    protected virtual void FixedUpdate()
    {
        if (isDead) return;
        Vibrate();
        currentState?.FixedUpdateState();
    }

    public void ChangeState(EnemyBaseState newState)
    {
        if (isSpawning)
        {
            return;
        }
        currentState?.ExitState();
        currentState = newState;     
        Debug.Log("Entered State: " + currentState);
        currentState.EnterState(this);
    }

    public void OnTakeDamage(int amount)
    {
        int finalDamage = Mathf.FloorToInt(amount * damageTakenModifierStat.GetFinalValue());
        currentHealth -= finalDamage;

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

    public void OnRecieveEffect(StatusEffect newEffect)
    { 
        currentStatusEffects.Add(newEffect);
        ShowEffect(newEffect.GetEffectText());
        RecalculateStats();
    }

    public void OnRecieveEffect(StatusEffect newEffect, Color effectColor)
    {
        currentStatusEffects.Add(newEffect);
        ShowEffect(newEffect.GetEffectText(), effectColor);
        RecalculateStats();
    }

    private void UpdateEffects()
    {
        for (int i = currentStatusEffects.Count - 1; i >= 0; i--)
        {
            currentStatusEffects[i].Update();

            if (currentStatusEffects[i].IsExpired())
            {
                currentStatusEffects.RemoveAt(i);
                RecalculateStats();
            }
        }
    }

    private void RecalculateStats()
    {
        foreach (var stat in stats)
        {
            stat.ResetModifiers();
        }

        foreach (var effect in currentStatusEffects)
        {
            effect.ApplyStatModifier(this);
        }

        //Debug.Log("current speed = " + moveSpeedStat.GetFinalValue());

        enemyAgent.speed = moveSpeedStat.GetFinalValue() * 2;
        enemyAgent.acceleration = moveSpeedStat.GetFinalValue() * 5;
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

    public void StartVibrating(float duration)
    {
        vibrationDuration = duration;
        vibrationTimer = 0f;

        initialPosition = transform.position;
        isVibrating = true;

        if (animator != null)
        {
            animator.speed = 0f;
        }
    }

    private void Vibrate()
    {
        if (!isVibrating) { return; }

        vibrationTimer += Time.deltaTime;      
        if (vibrationTimer >= vibrationDuration)
        {
            isVibrating = false;
            transform.position = new Vector3(initialPosition.x, transform.position.y, initialPosition.z);
            return;
        }

        float x = Mathf.Sin(Time.time * vibrateSpeed) * vibrateIntensity;
        float z = Mathf.Sin(Time.time * vibrateSpeed) * vibrateIntensity;    
        transform.position = new Vector3(initialPosition.x + x, transform.position.y, initialPosition.z + z);
    }

    public void StopVibrating()
    {
        isVibrating = false;
        transform.position = new Vector3(initialPosition.x, transform.position.y, initialPosition.z);

        if (animator != null)
        {
            animator.speed = 1f;
        }
    }

    protected void ShowDamage(int damage)
    {
        Vector3 randomOffset = new(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f));
        GameObject damageNumber = Instantiate(damageText, rb.position + randomOffset, Quaternion.identity);
        TextMeshPro tempTMPAccess = damageNumber.GetComponent<TextMeshPro>();
        tempTMPAccess.text = damage.ToString();
        float size = Mathf.Clamp(10 + (damage * 1.1f), 36f, 240f);
        tempTMPAccess.fontSize = size;

    }

    protected void ShowDamage(int damage, Color color)
    {
        Vector3 randomOffset = new(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f));
        GameObject damageNumber = Instantiate(damageText, rb.position + randomOffset, Quaternion.identity);
        TextMeshPro tempTMPAccess = damageNumber.GetComponent<TextMeshPro>();
        tempTMPAccess.text = damage.ToString();
        tempTMPAccess.color = color;
        float size = Mathf.Clamp(10 + (damage * 1.1f), 36f, 240f);
        tempTMPAccess.fontSize = size;

    }

    protected void ShowEffect(string effectText)
    {
        //Debug.Log("effect applied");
        //Debug.Log(effectText);

        Vector3 randomOffset = new(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        GameObject damageNumber = Instantiate(damageText, rb.position + randomOffset, Quaternion.identity);
        TextMeshPro tempTMPAccess = damageNumber.GetComponent<TextMeshPro>();
        tempTMPAccess.text = effectText;
    }

    protected void ShowEffect(string effectText, Color color)
    {
        //Debug.Log("effect applied");
        //Debug.Log(effectText);

        Vector3 randomOffset = new(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(2f, 4f), UnityEngine.Random.Range(-3f, 3f));
        GameObject damageNumber = Instantiate(damageText, rb.position + randomOffset, Quaternion.identity);
        TextMeshPro tempTMPAccess = damageNumber.GetComponent<TextMeshPro>();
        tempTMPAccess.text = effectText;
        tempTMPAccess.color = color;
        tempTMPAccess.fontSize = 52f;
    }

    public virtual void OnDeath()
    {
        if (isDead) return;
        isDead = true;
        currentState?.ExitState();
        EnemyHasDied?.Invoke();
        Destroy(gameObject);
    }

    // Check for Knockback wall damage
    protected void OnCollisionEnter(Collision collision) 
    {
        if (!collision.gameObject.CompareTag("Environment")) {  return; }
        if (!isKnockedBack) { return; }
        if (isKnockedBackByGolem) { return; }      

        Debug.Log("Wall Slam Triggered with DMG Mod of: " + Mathf.Clamp(wallSlamDamageModifierStat.GetFinalValue(), 1.0f, 2.0f));

        float dmgMod = Mathf.Clamp(wallSlamDamageModifierStat.GetFinalValue(), 1.0f, 2.0f);
        int appliedDamage = (int)(collision.impulse.magnitude * dmgMod);
        isKnockedBack = false;
        if (appliedDamage < 10) { return; }
        if (wallSlamDamageModifierStat.GetFinalValue() > 1.1f)
        {
            ShowEffect("Shattered", Color.deepSkyBlue);
            OnTakeDamage(appliedDamage, Color.deepSkyBlue);
        }
        else
        {
            ShowEffect("Slammed", Color.darkGoldenRod);
            OnTakeDamage(appliedDamage, Color.darkGoldenRod);
        }

        

        // Eventual VFX/SFX can go here for wall slams
        // add a check for the value of dmgMod to increase volume/size of effects

        
    }

    // Single Instant Look At Player - Used By Ranged Enemy when attack starts
    protected void LookAtPlayer()
    {
        Vector3 playerDir = playerReference.transform.position - transform.position;
        playerDir.y = transform.position.y;
        Quaternion lookRotation = Quaternion.LookRotation(playerDir);
        lookRotation.z = 0f;
        lookRotation.x = 0f;
        transform.rotation = lookRotation;       
    }
}
