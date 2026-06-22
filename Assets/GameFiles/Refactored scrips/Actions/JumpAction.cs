using System.Collections.Generic;
using NUnit.Framework;
using System;
using UnityEngine;


[Serializable]
public class JumpAction : BaseEntityAction
{
    public static event Action<float> ShakeScreen;

    Rigidbody rb;
    private float jumpHeight = 5f, jumpSpeed = 5f;
    private float startHeight = 0f, targetHeight = 5f;
    private Quaternion startRotation, targetRotation;
    private Quaternion[] rotationMap;

    private float remainingHeight;

    //interfaces to cache
    IGrounded grounded;
    IJumpable jumpable;
    IModifiableActions modifiableActions;

    //public JumpAction() { }
    public override void StartAction(Entity entity)
    {
        //isComplete = false;
        base.StartAction(entity);

        rb = (entity as IUsesRigidBody).rb;
        grounded = entity as IGrounded;
        jumpable = entity as IJumpable;
        modifiableActions = entity as IModifiableActions;

        jumpable.isJumping = true;

        rotationMap = new Quaternion[]
        {
            Quaternion.Euler(0,0,0), //1
            Quaternion.Euler(90,0,0), //2
            Quaternion.Euler(0,0,90), //3
            Quaternion.Euler(0,0,270), //4
            Quaternion.Euler(270,0,0), //5
            Quaternion.Euler(180,0,0) //6
        };

        jumpHeight = jumpable.jumpHeight.GetFinalValue();
        jumpSpeed = jumpable.jumpSpeed.GetFinalValue();

        rb.useGravity = false;

        startHeight = entity.transform.position.y;
        targetHeight = startHeight + jumpHeight;
        remainingHeight = targetHeight - startHeight;

        Vector3 eulerStartRotation = entity.transform.rotation.eulerAngles;
        eulerStartRotation.x = Mathf.Round(eulerStartRotation.x);
        eulerStartRotation.y = Mathf.Round(eulerStartRotation.y);
        eulerStartRotation.z = Mathf.Round(eulerStartRotation.z);
        startRotation = Quaternion.Euler(eulerStartRotation.x, eulerStartRotation.y, eulerStartRotation.z);

        ConditionalAction targetAction = modifiableActions.actionSelectionSystem.GetRandomConditionalAction();
        targetAction.triggered = false;
        int index = modifiableActions.actionSelectionSystem.LastReturnedActionIndex;
        targetRotation = rotationMap[index];
        //Debug.Log(index);

        if (targetAction.action is ISlam)
        {
            (targetAction.action as ISlam).slamRange.ResetModifiers();
            if (entity is IUsesEntityInput tempInput)
            {
                (targetAction.action as ISlam).slamRange.AddMultiplierFlat(tempInput.inputManager.holdTime);
            }
            else if ((jumpable.impactSpeed.GetFinalValue() > jumpable.impactSpeed.GetBaseValue()))
            {
                (targetAction.action as ISlam).slamRange.AddMultiplierFlat(1.5f);
            }

        }
        (ownerEntity as IActionable).actionController.availableActions.Add(targetAction);
    }
    public override void UpdateAction()
    {

    }
    public override void FixedUpdateAction()
    {
        if (remainingHeight > 0.01f)
        {
            ApplyJump();
            return;
        }

        if (!grounded.isGrounded)
        {
            ApplyDownwardForce();
            return;
        }

        ApplyScreenShake();
        EndAction();
    }
    public override void InterruptAction()
    {
        rb.useGravity = true;
        rb.isKinematic = false;

        ownerEntity.bodySystem.body.transform.rotation = targetRotation;
        ownerEntity.bodySystem.originalRotation = targetRotation;
        //rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        jumpable.jumpHeight.ResetModifiers();
        jumpable.impactSpeed.ResetModifiers();
        jumpable.isJumping = false;
        isComplete = true;
    }
    public override void EndAction()
    {
        rb.useGravity = true;
        rb.isKinematic = false;

        ownerEntity.bodySystem.body.transform.rotation = targetRotation;
        ownerEntity.bodySystem.originalRotation = targetRotation;
        //rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        jumpable.jumpHeight.ResetModifiers();
        jumpable.impactSpeed.ResetModifiers();
        jumpable.isJumping = false;
        isComplete = true;
    }

    //Helper Functions
    private bool ApplyJump()
    {
        float currentHeight = ownerEntity.transform.position.y;
        remainingHeight = targetHeight - currentHeight;

        float verticalVelocity = remainingHeight * jumpSpeed;
        Vector3 velocity = rb.linearVelocity;
        velocity.y = verticalVelocity;

        rb.linearVelocity = velocity;

        float progress = Mathf.InverseLerp(startHeight, targetHeight, currentHeight);
        ApplyRotation(progress);

        return remainingHeight <= 0.01f;
    }

    private void ApplyRotation(float jumpProgress)
    {
        float t = Mathf.SmoothStep(0f, 1f, jumpProgress);

        Quaternion rotation = Quaternion.Slerp(startRotation, targetRotation, t);
        ownerEntity.bodySystem.body.transform.rotation = rotation;

        Quaternion visualSpin = Quaternion.Euler(360 * t, 360 * t, 360 * t);
        ownerEntity.bodySystem.body.transform.rotation *= visualSpin;
    }

    private void ApplyDownwardForce()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.y = rb.linearVelocity.y - jumpable.impactSpeed.GetFinalValue();
        rb.linearVelocity = velocity;
    }

    private void ApplyScreenShake()
    {
        if (isComplete) return;

        float magnitude = jumpable.impactSpeed.GetFinalValue() / jumpable.impactSpeed.GetBaseValue() * 2;
        ShakeScreen?.Invoke(magnitude);
    }

    public override BaseEntityAction Clone()
    {
        return new JumpAction();
    }
}
