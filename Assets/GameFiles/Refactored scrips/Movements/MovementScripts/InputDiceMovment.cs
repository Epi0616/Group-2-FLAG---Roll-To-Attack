using System;
using UnityEngine;

[Serializable]
public class InputDiceMovment : InputEntityMovement
{
    public InputDiceMovment() { }
    public override void FixedUpdateMovement()
    {
        Vector3 direction = usesEntityInput.inputManager.move.action.ReadValue<Vector3>().normalized;
        Debug.Log($"velocity {direction}");
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
            return;
        }

        RotateBody(direction);
    }

    private void RotateBody(Vector3 velocity)
    {
        //Quaternion rotationAmount = Quaternion.LookRotation(velocity, Vector3.up);
        //ownerEntity.bodySystem.transform.localRotation = rotationAmount;
        Transform bodyTransform = ownerEntity.bodySystem.body.transform;
        //rotation *= Quaternion.Euler(velocity.z, 0, velocity.x);   
    }

    private void Flip90Forwards(float to, float from, float duration)
    {
        float timer = 0;
        float t = 0;

        Transform bodyTransform = ownerEntity.bodySystem.body.transform;

        while (t < 1)
        { 
            timer += Time.deltaTime;
            t = timer/duration;

            Quaternion currentRotation = bodyTransform.localRotation;

            Vector3 rotation = currentRotation.eulerAngles;
            Vector3 forwards = Vector3.right;
            bodyTransform.Rotate(forwards);

        }
    }

    public override BaseEntityMovement Clone()
    {
        return new InputDiceMovment();
    }
}
