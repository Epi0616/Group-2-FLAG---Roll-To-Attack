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
    protected int currentHealth;  
    public Stat moveSpeedStat;
    public Stat stunTimeStat;
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
    public bool isVibrating;
    public bool isFragile;
    public LayerMask playerLayer;
    public LayerMask environmentLayer;
    
    public bool isSpawning;
    private bool isDead;
    public static event Action EnemyHasDied;

    protected float vibrateSpeed = 50f;
    protected float vibrateIntensity = 0.1f;
    private float vibrationTimer = 0f;
    protected float vibrationDuration;
    protected Vector3 initialPosition;

    private Stat[] stats;
    private List<StatusEffect> currentStatusEffects = new List<StatusEffect>();

    private void Awake()
    {
        stats = new Stat[]
        {
            moveSpeedStat,
            attackCooldownStat,
            knockbackWeightModifierStat,
            stunTimeStat
        };
        
    }

    protected void Start()
    {
        currentHealth = maxHealth;

        enemyAgent.speed = moveSpeedStat.GetFinalValue() * 2;
        enemyAgent.stoppingDistance = attackRange;
        enemyAgent.acceleration = moveSpeedStat.GetFinalValue() * 5;
        
        playerController = playerReference.GetComponent<PlayerStateController>();


        ChangeState(new EnemySpawnState());
    }

    protected virtual void Spawning()
    {
        currentState = new EnemyMoveState();
        currentState.EnterState(this);
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
        currentState.EnterState(this);
    }

    public void OnTakeDamage(int amount)
    {
        currentHealth -= amount;

        ShowDamage(amount);

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public abstract void Attack();
    public abstract void CompleteAttack();

    public void OnRecieveEffect(StatusEffect newEffect)
    { 
        currentStatusEffects.Add(newEffect);
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

    public void StopVibrating()
    {
        isVibrating = false;
        transform.position = new Vector3(initialPosition.x, transform.position.y, initialPosition.z);

        if (animator != null)
        {
            //Debug.Log("Animation Restarted");
            animator.speed = 1f;
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

    public void StartSpawnVibration()
    {
        StartCoroutine(SpawnAnimation());
    }

    private IEnumerator SpawnAnimation()
    {
        float timeElapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
        while (timeElapsed < 5f)
        {
            Vector3 lerpOffset = Vector3.Lerp(startPos, endPos, timeElapsed / 5f);
            transform.position = lerpOffset + SpawningAnimationVibrateOffset();
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        StopVibrating();
        transform.position = endPos;
        isSpawning = false;
        ChangeState(new EnemyMoveState());
    }

    private Vector3 SpawningAnimationVibrateOffset()
    {
        //if (!isVibrating) { return Vector3.zero; }
        float x = Mathf.Sin(Time.time * vibrateSpeed) * vibrateIntensity;
        float z = Mathf.Sin(Time.time * vibrateSpeed) * vibrateIntensity;
        return new Vector3(x, 0, z);
    }

    protected void ShowDamage(int damage)
    {
        Vector3 randomOffset = new(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        GameObject damageNumber = Instantiate(damageText, rb.position + randomOffset, Quaternion.identity);
        damageNumber.GetComponent<TextMeshPro>().text = damage.ToString();
    }
    public virtual void OnDeath()
    {
        if (isDead) return;
        isDead = true;
        currentState?.ExitState();
        EnemyHasDied?.Invoke();
        Destroy(gameObject);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Environment")) {  return; }
        if (!isKnockedBack) { return; }

        //Debug.Log("Wall Slam Triggered with DMG Mod of: " + Mathf.Clamp(wallSlamDamageModifierStat.GetFinalValue(), 1.0f, 2.0f));
        float dmgMod = Mathf.Clamp(wallSlamDamageModifierStat.GetFinalValue(), 1.0f, 2.0f);
        int appliedDamage = (int)(collision.impulse.magnitude * dmgMod);
        isKnockedBack = false;

        // Eventual VFX/SFX can go here for wall slams
        // add a check for the value of dmgMod to increase volume/size of effects

        OnTakeDamage(appliedDamage);
    }
}
