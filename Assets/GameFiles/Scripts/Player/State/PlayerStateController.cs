using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEditor;
using NUnit.Framework;
public class PlayerStateController : MonoBehaviour
{
    [Header("Dont modify the variables listed below")]
    public GameObject impactField;
    public GameObject poisonImpactField, playerSpike;
    public Rigidbody rb;
    public InputActionReference move, attack;
    public PlayerBaseState currentState;
    public bool isGrounded;

    [SerializeField] private LayerMask groundLayer;

    private List<GameObject> objectsInOrbit = new List<GameObject>();
    
    

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
    }

    private void OnDisable()
    {
        move.action.Disable();
        attack.action.Disable();
    }

    private void Start()
    {
        currentHealth = maxHealth;
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

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        Debug.Log("Game Over");
    }

    public GameObject InstantiateObejct(GameObject prefab, Vector3 position)
    { 
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity);

        return newObject;
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
            PlayerSpike tempScript = objectsInOrbit[i].gameObject.GetComponent<PlayerSpike>();
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
