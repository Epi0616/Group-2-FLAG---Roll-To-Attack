using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEditor;
using NUnit.Framework;
using System;
public class PlayerStateController : MonoBehaviour
{
    [Header("Dont modify the variables listed below")]
    public Rigidbody rb;
    public InputActionReference move, attack;
    public PlayerBaseState currentState;
    public AbilitySystem abilitySystem;
    public AttackSystem attackSystem;
    public bool isGrounded;
    [SerializeField] private LayerMask groundLayer;

    public static event Action<int> UpdateHealthBar;
    public static event Action GameOver;

    [Header("For modification")]

    [Header("Movement feel")]
    public bool moveWhileJumping;
    public float moveSpeed;
    public float moveSpeedWhileJumping;
    public float jumpHeight;
    public float jumpSpeed;
    public float impactSpeed;

    [Header("Side weighting")]
    public int onePipWeight;
    public int twoPipWeight;
    public int threePipWeight;
    public int fourPipWeight;
    public int fivePipWeight;
    public int sixPipWeight;
 

    [Header("Attack feel")]
    public float baseRadiusSize;

    [Header("General Stats")]
    public int maxHealth;
    public int currentHealth;


    private void OnEnable()
    {
        move.action.Enable();
        attack.action.Enable();
        EnemyDirector.WaveOver += HealToFull;
    }

    private void OnDisable()
    {
        move.action.Disable();
        attack.action.Disable();
        EnemyDirector.WaveOver -= HealToFull;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar?.Invoke(currentHealth);
        currentState = new PlayerMovementState();
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        CheckForGrounded();
        currentState.FixedUpdateState();
    }

    public void SwitchState(PlayerBaseState newState)
    { 
        currentState = newState;
        currentState.EnterState(this);
    }

    public void OnTakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }
    public void HealToFull(float waveNumber)
    {
        currentHealth = maxHealth;
        UpdateHealthBar?.Invoke(currentHealth);
    }
    public void OnDeath()
    {
        Debug.Log("Game Over");
        GameOver?.Invoke();
    }

    private void CheckForGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f, groundLayer);
    }
}
