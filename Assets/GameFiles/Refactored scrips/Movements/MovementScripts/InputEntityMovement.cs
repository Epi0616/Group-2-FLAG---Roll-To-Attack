using System;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class InputEntityMovement : BaseEntityMovement
{
    private IUsesEntityInput usesEntityInput;
    private IGrounded grounded;
    private IJumpable jumpable;
    private Rigidbody rb;
    public InputEntityMovement() { }
    public override void StartMovement(Entity ownerEntity)
    {
        base.StartMovement(ownerEntity);
        usesEntityInput = ownerEntity as IUsesEntityInput;
        grounded = ownerEntity as IGrounded;
        rb = (ownerEntity as IUsesRigidBody).rb;

        if (ownerEntity is IJumpable)
        { 
            jumpable = ownerEntity as IJumpable;
        }
    }
    public override void FixedUpdateMovement()
    {
        Vector3 direction = usesEntityInput.inputManager.move.action.ReadValue<Vector3>();
        float movementSpeed = moveable.movementSpeed.GetFinalValue();

        Vector3 targetVelocity = direction * movementSpeed;
        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = targetVelocity;

        if (!grounded.isGrounded)
        {
            if (ownerEntity is IJumpable)
            {
                if (!(jumpable).isJumping)
                {
                    rb.AddForce(Vector3.down * 100, ForceMode.Acceleration);
                }
            }
        }
    }
    public override void InterruptMovement()
    {
    }
    public override void EndMovement()
    {
    }
    public override BaseEntityMovement Clone()
    {
        return new InputEntityMovement();
    }
}
