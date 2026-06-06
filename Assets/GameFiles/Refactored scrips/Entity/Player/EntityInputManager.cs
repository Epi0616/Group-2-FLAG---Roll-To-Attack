using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EntityInputManager : MonoBehaviour
{
    public InputActionReference move;
    public InputActionReference attack;

    //public Vector3 moveDirection { get; private set; }
    //public bool attackWasPressedThisFrame { get; private set; }
    //public bool attackIsPressed { get; private set; }

    //private void OnEnable()
    //{
    //    move.action.Enable();
    //    attack.action.Enable();
    //}

    //private void OnDisable()
    //{
    //    move.action.Disable();
    //    attack.action.Disable();
    //}

    //private void Update()
    //{
    //    moveDirection = move.action.ReadValue<Vector3>();

    //    attackWasPressedThisFrame = attack.action.WasPressedThisFrame();
    //    attackIsPressed = attack.action.IsPressed();
    //}

    //private void LateUpdate()
    //{
    //    attackWasPressedThisFrame = false;
    //}
}
