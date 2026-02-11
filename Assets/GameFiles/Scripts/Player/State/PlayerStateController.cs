using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
public class PlayerStateController : MonoBehaviour
{
    [Header("Dont modify the variables listed below")]
    public GameObject impactField;
    public GameObject poisonImpactField;
    public Rigidbody rb;
    public InputActionReference move, attack;
    public PlayerBaseState currentState;

    public bool isGrounded;
    [SerializeField] private LayerMask groundLayer;

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

    public GameObject InstantiateObejct(GameObject prefab, Vector3 position)
    { 
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity);

        return newObject;
    }

    private void CheckForGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f, groundLayer);
    }
}
