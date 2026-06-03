using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EntityInputManager : MonoBehaviour
{
    public InputActionReference move, attack;

    public Vector3 moveDirection { get; private set; }
    public bool attackPressed { get; private set; }

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

    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector3>();
    }

    private void LateUpdate()
    {
    }
}
