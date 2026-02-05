using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateController : MonoBehaviour
{
    public GameObject impactField;
    public ImpactField impactFieldScript;
    public Rigidbody rb;
    public InputActionReference move, attack;
    public PlayerBaseState currentState;

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
}
