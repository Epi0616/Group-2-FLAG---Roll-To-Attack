using System;
using UnityEditor.Rendering;
using UnityEngine;

[Serializable]
public class JumpAction : BaseEntityAction
{
    Rigidbody rb;
    private float jumpHeight = 5f, jumpSpeed = 5f;
    private float startHeight = 0f, targetHeight = 5f;
    private Quaternion startRotation, targetRotation;
    private Quaternion[] rotationMap;

    public override void StartAction(Entity entity)
    {
        isComplete = false;
        Debug.Log("starting jump");
        base.StartAction(entity);
        rb = (entity as IUsesRigidBody).rb;

        rotationMap = new Quaternion[]
        {
            Quaternion.Euler(0,0,0), //1
            Quaternion.Euler(90,0,0), //2
            Quaternion.Euler(0,0,90), //3
            Quaternion.Euler(0,0,270), //4
            Quaternion.Euler(270,0,0), //5
            Quaternion.Euler(180,0,0) //6
        };

        rb.useGravity = false;

        startHeight = entity.transform.position.y;
        targetHeight = startHeight + jumpHeight;

        Vector3 eulerStartRotation = entity.transform.rotation.eulerAngles;
        eulerStartRotation.x = Mathf.Round(eulerStartRotation.x);
        eulerStartRotation.y = Mathf.Round(eulerStartRotation.y);
        eulerStartRotation.z = Mathf.Round(eulerStartRotation.z);
        startRotation = Quaternion.Euler(eulerStartRotation.x, eulerStartRotation.y, eulerStartRotation.z);

        targetRotation = rotationMap[0];

    }
    public override void UpdateAction()
    {
    }
    public override void FixedUpdateAction()
    {
        if (ApplyJump())
        {
            EndAction();

        }
    }
    public override void InterruptAction()
    {
        rb.useGravity = true;
        rb.isKinematic = false;

        //player.bodySystem.body.transform.rotation = targetRotation;
        //player.bodySystem.originalRotation = targetRotation;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isComplete = true;
    }
    public override void EndAction()
    {
        rb.useGravity = true;
        rb.isKinematic = false;

        //player.bodySystem.body.transform.rotation = targetRotation;
        //player.bodySystem.originalRotation = targetRotation;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isComplete = true;
    }

    //Helper Functions

    private bool ApplyJump()
    {
        float currentHeight = ownerEntity.transform.position.y;
        float remainingHeight = targetHeight - currentHeight;

        float verticalVelocity = remainingHeight * jumpSpeed;
        Vector3 velocity = new Vector3(0, verticalVelocity, 0);


        rb.linearVelocity = velocity;

        float progress = Mathf.InverseLerp(startHeight, targetHeight, currentHeight);
        //ApplyRotation(progress);

        return remainingHeight <= 0.01f;
    }
}
