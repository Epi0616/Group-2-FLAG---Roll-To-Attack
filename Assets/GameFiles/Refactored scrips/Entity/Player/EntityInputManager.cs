using UnityEngine;
using UnityEngine.InputSystem;

public class EntityInputManager : MonoBehaviour
{
    Entity ownerEntity;
    public InputActionReference move;
    public InputActionReference attack;

    public float holdTime;
    private IGrounded grounded;

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

    public void Initialise(Entity entity)
    { 
        ownerEntity = entity;
        grounded = entity as IGrounded;
    }

    private void Update()
    {
        UpdateHoldTime();
    }

    private void LateUpdate()
    {
        if (!(attack.action.IsPressed()))
        {
            holdTime = 0;
        }
    }

    private void UpdateHoldTime()
    {
        if (attack.action.IsPressed() && grounded.isGrounded)
        {
            holdTime += Time.deltaTime;
            holdTime = Mathf.Clamp(holdTime, 0, 1);
        }
    }
}
