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
    public float moveSpeed;
    public float attackRange;
    public float stunTime;
    public float knockbackWeightModifier;

    [Header("Variables not to be Adjusted")]
    protected PlayerStateController playerController;
    public NavMeshAgent enemyAgent;
    public Rigidbody rb;
    private EnemyBaseState currentState;
    public bool isStunned;
    public LayerMask playerLayer;
    public LayerMask environmentLayer;

    private bool isDead;
    public static event Action EnemyHasDied;
    

    protected void Start()
    {
        currentHealth = maxHealth;

        enemyAgent.speed = moveSpeed * 2;
        enemyAgent.stoppingDistance = attackRange;
        enemyAgent.acceleration = moveSpeed * 5;

        currentState = new EnemyMoveState();
        currentState.EnterState(this);
    }

    protected virtual void Update()
    {
        currentState?.UpdateState();
    }
    protected virtual void FixedUpdate()
    {
        currentState?.FixedUpdateState();
    }

    public void ChangeState(EnemyBaseState newState)
    {
        if (currentState == newState) return;
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

    public virtual void OnTakeKnockback(float knockbackForce)
    {
        ChangeState(new EnemyKnockbackState(knockbackForce));
    }

    public virtual void OnStunned()
    {
        ChangeState(new EnemyStunnedState(stunTime));
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
        EnemyHasDied?.Invoke();
        Destroy(this.gameObject);
    }
}
