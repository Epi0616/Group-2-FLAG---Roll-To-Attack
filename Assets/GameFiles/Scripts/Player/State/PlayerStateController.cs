using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerStateController : MonoBehaviour
{
    [Header("Dont modify the variables listed below")]
    public Rigidbody rb;
    public InputActionReference move, attack;
    public PlayerBaseState currentState;
    public AbilitySystem abilitySystem;
    public AttackSystem attackSystem;
    public BoxCollider boxCollider;
    public GameObject body;
    public bool isGrounded;
    [SerializeField] private LayerMask groundLayer;

    public static event Action<int> UpdateHealthBar;
    public static event Action GameOver;
    public static event Action<float> ShakeScreen;

    [Header("For modification")]

    [Header("Movement feel")]
    public bool moveWhileJumping;
    public Stat moveSpeed;
    public Stat moveSpeedWhileJumping;
    public Stat jumpHeight;
    public Stat jumpSpeed;
    public Stat impactSpeed;

    [Header("Side weighting")]
    public int onePipWeight;
    public int twoPipWeight;
    public int threePipWeight;
    public int fourPipWeight;
    public int fivePipWeight;
    public int sixPipWeight;
 

    [Header("Attack feel")]
    public Stat baseRadiusSize;
    private float holdTime = 0;
    private bool charging = false;
    public Quaternion originalRotation;

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
        originalRotation = body.transform.rotation;
        currentState = new PlayerMovementState();
        currentState.EnterState(this);
    }

    private void Update()
    {
        CheckForAttackAction();
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

    public void AddScreenShake(float magnitude)
    { 
        ShakeScreen?.Invoke(magnitude);
    }

    private void CheckForGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f, groundLayer);
    }

    private void CheckForAttackAction()
    {
        if (!isGrounded) return;

        if (attack.action.WasPressedThisFrame())
        {
            SwitchState(new PlayerJumpState());
            holdTime = 0;
            return;
        }

        if (attack.action.IsPressed())
        {
            holdTime += Time.deltaTime;
            holdTime = Math.Clamp(holdTime, 0, 1);
            ChargingEffect();
        }

        else if (attack.action.WasReleasedThisFrame() && holdTime > 0.2)
        {
            jumpHeight.AddMultiplierFlat(holdTime * 1.5f);
            impactSpeed.AddMultiplierFlat(holdTime * 2);
            baseRadiusSize.AddMultiplierFlat(holdTime);

            SwitchState(new PlayerJumpState());
            moveSpeed.ResetModifiers();
            holdTime = 0;
            return;
        }
    }

    private void ChargingEffect()
    {
        float moveSpeedMultiplier = ((2 - holdTime) / 2);
        moveSpeedMultiplier = Mathf.Clamp(moveSpeedMultiplier, 0.35f, 1);

        moveSpeed.SetMultiplier(moveSpeedMultiplier);
        ShakeDiceBody(2 / moveSpeedMultiplier);
    }

    private void ShakeDiceBody(float magnitude)
    {
        float x = Mathf.Sin(Time.time * 50f) * magnitude;
        float y = Mathf.Sin(Time.time * 50f) * magnitude;
        float z = Mathf.Sin(Time.time * 50f) * magnitude;
        body.transform.rotation = originalRotation * Quaternion.Euler(x, y, z);
    }
}
