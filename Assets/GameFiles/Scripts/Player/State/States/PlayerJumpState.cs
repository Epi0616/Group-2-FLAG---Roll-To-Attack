using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerJumpState : PlayerBaseState
{
    private float jumpHeight, jumpSpeed;
    private float startHeight, targetHeight;
    private Quaternion startRotation, targetRotation;
    private AbilityDescriptor selectedAbility;
    private Quaternion[] rotationMap;

    // this is purely to allow movement while jumping for designers in the editor
    private Vector3 moveDirection;


    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);

        //massive amount of setup that could probably do with its own function

        jumpHeight = player.jumpHeight.GetFinalValue();
        jumpSpeed = player.jumpSpeed.GetFinalValue();

        player.rb.useGravity = false;
        //player.rb.isKinematic = true;

        rotationMap = new Quaternion[]
        {
            Quaternion.Euler(0,0,0), //1
            Quaternion.Euler(90,0,0), //2
            Quaternion.Euler(0,0,90), //3
            Quaternion.Euler(0,0,270), //4
            Quaternion.Euler(270,0,0), //5
            Quaternion.Euler(180,0,0) //6
        };

        startHeight = player.transform.position.y;
        targetHeight = startHeight + jumpHeight;

        Vector3 eulerStartRotation = player.transform.rotation.eulerAngles;
        eulerStartRotation.x = Mathf.Round(eulerStartRotation.x);
        eulerStartRotation.y = Mathf.Round(eulerStartRotation.y);
        eulerStartRotation.z = Mathf.Round(eulerStartRotation.z);
        startRotation = Quaternion.Euler(eulerStartRotation.x, eulerStartRotation.y, eulerStartRotation.z);

        selectedAbility = player.abilitySystem.GetRandomAbility();
        int pipNumber = player.abilitySystem.GetLastReturnedPipNumber();

        targetRotation = rotationMap[pipNumber - 1];
    }

    public override void UpdateState()
    {
        // this is purely to allow movement while jumping for designers in the editor
        CheckForMoveActionPressed();
    }

    public override void FixedUpdateState()
    {
        if (!ApplyJump())
        {
            return;
        }

        CompleteJump();
    }

    private bool ApplyJump()
    {
        float currentHeight = player.transform.position.y;
        float remainingHeight = targetHeight - currentHeight;

        float verticalVelocity = remainingHeight * jumpSpeed;
        Vector3 velocity = new Vector3(0, verticalVelocity, 0);

        if (player.moveWhileJumping)
        {
            velocity += moveDirection * player.moveSpeedWhileJumping.GetFinalValue();
        }

        player.rb.linearVelocity = velocity;

        float progress = Mathf.InverseLerp(startHeight, targetHeight, currentHeight);
        ApplyRotation(progress);

        return remainingHeight <= 0.01f;
    }

    private void ApplyRotation(float jumpProgress)
    {
        float t = Mathf.SmoothStep(0f, 1f, jumpProgress);

        Quaternion rotation = Quaternion.Slerp(startRotation, targetRotation, t);
        player.bodySystem.body.transform.rotation = rotation;

        Quaternion visualSpin = Quaternion.Euler(360*t, 360*t, 360*t);
        player.bodySystem.body.transform.rotation *= visualSpin;
    }

    private void CompleteJump()
    {
        player.rb.useGravity = true;
        player.rb.isKinematic = false;

        player.bodySystem.body.transform.rotation = targetRotation;
        player.bodySystem.originalRotation = targetRotation;
        player.rb.linearVelocity = Vector3.zero;
        player.rb.angularVelocity = Vector3.zero;

        PlayerBaseState nextState = selectedAbility.Create();
        if (nextState == null)
        {
            Debug.LogError("Ability returned null state!");
            player.SwitchState(new PlayerMovementState());
            return;
        }

        player.SwitchState(nextState);
    }


    // this is purely to allow movement while jumping for designers in the editor
    private void CheckForMoveActionPressed()
    {
        if (player.move.action.IsPressed())
        {
            moveDirection = player.move.action.ReadValue<Vector3>();
            return;
        }

        moveDirection = new(0, 0, 0);
    }
}


// unlocking rotation for now as allows player to roll around, embraces dice feel??

//switch (selectedAbility.pipNumber)
//{
//    case 1:
//        player.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
//        break;
//    case 2:
//        player.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
//        break;
//    case 3:
//        player.rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
//        break;
//    case 4:
//        player.rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
//        break;
//    case 5:
//        player.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
//        break;
//    case 6:
//        player.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
//        break;
//}

//bool finishedJump = false;

//Vector3 tempPosition = player.transform.position;
//float currentHeight = player.transform.position.y;
////lerp (a, b, t) = a + (b-a) * t
//tempPosition.y += (targetHeight - currentHeight) * Time.deltaTime * jumpSpeed;


//// this is purely to allow movement while jumping for designers in the editor
//if (player.moveWhileJumping)
//{
//    tempPosition += moveDirection * Time.deltaTime * player.moveSpeedWhileJumping;
//}

//Vector3 movement = tempPosition - player.transform.position;
//float distance = movement.magnitude;

//if (distance > 0f)
//{
//    RaycastHit hit;

//    BoxCollider box = player.boxCollider;

//    Vector3 center = player.transform.TransformPoint(box.center);
//    Vector3 halfExtents = box.size * 0.5f;

//    if (Physics.BoxCast(
//        center,
//        halfExtents,
//        movement.normalized,
//        out hit,
//        player.transform.rotation,
//        distance))
//    {
//        tempPosition = player.transform.position + movement.normalized * (hit.distance - 0.01f);
//    }
//}

//player.rb.MovePosition(tempPosition);



//float progress = Mathf.InverseLerp(startHeight, targetHeight, tempPosition.y);
//ApplyRotation(progress);

//if (tempPosition.y >= targetHeight - 0.01f)
//{ 
//    finishedJump = true;
//}

//return finishedJump;