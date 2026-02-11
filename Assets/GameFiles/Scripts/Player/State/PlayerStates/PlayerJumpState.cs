using NUnit.Framework.Constraints;
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class PlayerJumpState : PlayerBaseState
{
    private float jumpHeight, jumpSpeed;
    private float startHeight, targetHeight;
    private Quaternion startRotation, targetRotation;
    private DicePip selectedPip;
    private Quaternion[] rotationMap;


    // this is purely to allow movement while jumping for designers in the editor
    private Vector3 moveDirection;

    private struct DicePip
    {
        public int pipNumber;
        public int weight;
        public Func<PlayerBaseState> createState;

        public DicePip(int pipNumber, int weight, Func<PlayerBaseState> createState)
        { 
            this.pipNumber = pipNumber;
            this.weight = weight;
            this.createState = createState;
        } 
    }

    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);

        player.rb.useGravity = false;
        player.rb.isKinematic = true;

        jumpHeight = player.jumpHeight;
        jumpSpeed = player.jumpSpeed;

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

        DicePip[] dicePips = 
        {
            new (1, player.onePipWeight,() => new PlayerOnePipState()),
            new (2, player.twoPipWeight, () => new PlayerTwoPipState()),
            new (3, player.threePipWeight, () => new PlayerMovementState()),
            new (4, player.fourPipWeight, () => new PlayerMovementState()),
            new (5, player.fivePipWeight, () => new PlayerMovementState()),
            new (6, player.sixPipWeight, () => new PlayerMovementState())
        };
        selectedPip = SelectDiceFace(dicePips);

        //Debug.Log(selectedPip.pipNumber);
        targetRotation = rotationMap[selectedPip.pipNumber - 1];

        switch (selectedPip.pipNumber)
        {
            case 1:
                player.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                break;
            case 2:
                player.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                break;
            case 3:
                player.rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                break;
            case 4:
                player.rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                break;
            case 5:
                player.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                break;
            case 6:
                player.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                break;
        }
       
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
        bool finishedJump = false;

        Vector3 tempPosition = player.transform.position;
        float currentHeight = player.transform.position.y;
        //lerp (a, b, t) = a + (b-a) * t
        tempPosition.y += (targetHeight - currentHeight) * Time.deltaTime * jumpSpeed;


        // this is purely to allow movement while jumping for designers in the editor
        if (player.moveWhileJumping)
        {
            tempPosition += moveDirection * Time.deltaTime * player.moveSpeedWhileJumping;
        }

        player.rb.MovePosition(tempPosition);

        float progress = Mathf.InverseLerp(startHeight, targetHeight, tempPosition.y);
        ApplyRotation(progress);

        if (tempPosition.y >= targetHeight - 0.01f)
        { 
            finishedJump = true;
        }

        return finishedJump;
    }

    private void ApplyRotation(float jumpProgress)
    {
        float t = Mathf.SmoothStep(0f, 1f, jumpProgress);

        Quaternion rotation = Quaternion.Slerp(startRotation, targetRotation, t);
        player.rb.MoveRotation(rotation);

        Quaternion visualSpin = Quaternion.Euler(360 * t, 360*t, 360*t);
        player.rb.MoveRotation(rotation * visualSpin);
    }

    private DicePip SelectDiceFace(DicePip[] dicePips)
    {
        int totalWeight = 0;

        foreach (var pip in dicePips)
        {
            totalWeight += pip.weight;
        }

        int randomNumber = Random.Range(1, totalWeight);
        int pipWeightTally = 0;

        foreach (var pip in dicePips)
        { 
            pipWeightTally += pip.weight;
            if (randomNumber <= (pipWeightTally))
            { 
                return pip;
            }
        }

        return new DicePip();
    }

    private void CompleteJump()
    {
        player.rb.useGravity = true;
        player.rb.isKinematic = false;

        player.rb.rotation = targetRotation;
        player.rb.linearVelocity = Vector3.zero;
        player.rb.angularVelocity = Vector3.zero;

        player.SwitchState(selectedPip.createState());
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
