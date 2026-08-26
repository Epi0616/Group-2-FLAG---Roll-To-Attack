using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

[Serializable]
public class InputDiceMovment : InputEntityMovement
{
    private Coroutine rotationCorrectionRoutine;

    public InputDiceMovment() { }
    public override void FixedUpdateMovement()
    {
        Vector3 direction = usesEntityInput.inputManager.move.action.ReadValue<Vector3>().normalized;
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

        RotateBody(targetVelocity);
    }

    private void RotateBody(Vector3 velocity)
    {
        Transform bodyTransform = ownerEntity.bodySystem.body.transform;

        if (velocity.magnitude <= 0)
        {
            HandleCorrectRotation(bodyTransform);
            return;
        }

        InterruptCorrection();
        RotateWithVelocity(bodyTransform, velocity);
    }

    private void RotateWithVelocity(Transform transform, Vector3 velocity)
    {
        Vector3 direction = velocity.normalized;
        Vector3 rotation = new Vector3(direction.z, 0, -direction.x);
        transform.RotateAround(ownerEntity.transform.position, rotation, velocity.magnitude / 1.5f);
    }

    private void HandleCorrectRotation(Transform transform)
    {
        if (rotationCorrectionRoutine != null) return;

        Vector3 currentRotation = transform.localEulerAngles;

        float correctedX = currentRotation.x + RotationToTheNearest90(currentRotation.x);
        float correctedY = currentRotation.y + RotationToTheNearest90(currentRotation.y);
        float correctedZ = currentRotation.z + RotationToTheNearest90(currentRotation.z);

        Vector3 correctedRotation = new Vector3(correctedX, correctedY, correctedZ);
        rotationCorrectionRoutine = ownerEntity.StartCoroutine(CorrectRotation(transform, correctedRotation, 0.25f));
    }

    private IEnumerator CorrectRotation(Transform transform, Vector3 targetRotationEuler, float duration)
    {
        float timer = 0;
        float t = 0;

        Quaternion currentRotation = transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(targetRotationEuler);

        while (t < 1)
        {
            timer += Time.fixedDeltaTime;
            t = timer / duration;

            transform.localRotation = Quaternion.Lerp(currentRotation, targetRotation, t);
            yield return new WaitForFixedUpdate();
        }

        rotationCorrectionRoutine = null;
    }

    private float RotationToTheNearest90(float value)
    { 
        float remainder = value % 90f;
        float difference = (90f - remainder);

        return remainder > 45f ? difference : -remainder;
    }

    private void InterruptCorrection()
    {
        if (rotationCorrectionRoutine != null)
        {
            ownerEntity.StopCoroutine(rotationCorrectionRoutine);
            rotationCorrectionRoutine = null;
        }
    }

    public override void InterruptMovement()
    {
        EndMovement();
    }

    public override void EndMovement()
    {
        InterruptCorrection();
    }

    public override BaseEntityMovement Clone()
    {
        return new InputDiceMovment();
    }
}
