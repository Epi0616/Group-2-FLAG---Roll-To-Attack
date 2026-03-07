using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEditor;
using NUnit.Framework;
using System;
public class PlayerStateController : MonoBehaviour
{
    [Header("Dont modify the variables listed below")]
    public GameObject impactField;
    public GameObject poisonImpactField, playerSpike, playerRocket;
    public Rigidbody rb;
    public InputActionReference move, attack;
    public PlayerBaseState currentState;
    public AbilitySystem abilitySystem;
    public bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    private List<GameObject> objectsInOrbit = new List<GameObject>();

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

    public GameObject InstantiateObejct(GameObject prefab, Vector3 position)
    { 
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity);

        return newObject;
    }

    public void CreateRockets(EnemyStateController target)
    {
        GameObject rocket = Instantiate(playerRocket, transform.position, Quaternion.identity);
        rocket.GetComponent<PlayerRocket>().SetTarget(target, transform.position.y);

    }

    public void CreateFourPipSpikesInOrbit()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject spike = Instantiate(playerSpike);
            objectsInOrbit.Add(spike);
        }

        for (int i = 0; i < objectsInOrbit.Count; i++)
        {
            float angle = i * (360f / objectsInOrbit.Count);
            PlayerSpikeFixedYMod tempScript = objectsInOrbit[i].gameObject.GetComponent<PlayerSpikeFixedYMod>(); //will need to change between PlayerSpikeFixedYMod and PlayerSpike depending on desired functionality/script attatched to the spike prefab
            tempScript.Initialize(angle, gameObject);
        }
    }

    public void RemoveObjectFromOrbit(GameObject obj)
    { 
        objectsInOrbit.Remove(obj);   
    }

    private void CheckForGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f, groundLayer);
    }
}
