using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
public class PlayerStateController : MonoBehaviour
{
    [Header("Dont modify the variables listed below")]
    public GameObject impactField;
    public ImpactField impactFieldScript;
    public Rigidbody rb;
    public InputActionReference move, attack;
    public PlayerBaseState currentState;

    [Header("For modification")]

    [Header("Jump feel")]
    public bool moveWhileJumping;
    public float jumpHeight;
    public float jumpSpeed;

    [Header("Side weighting")]
    public int onePipWeight;
    public int twoPipWeight;
    public int threePipWeight;
    public int fourPipWeight;
    public int fivePipWeight;
    public int sixPipWeight;


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

    private void Awake()
    {
        impactFieldScript = impactField.GetComponent<ImpactField>();
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
        currentState.FixedUpdateState();
    }

    public void SwitchState(PlayerBaseState newState)
    { 
        currentState = newState;
        currentState.EnterState(this);
    }

    public void ToggleGravity(bool useGravity)
    {
        rb.useGravity = useGravity;
    }
}
