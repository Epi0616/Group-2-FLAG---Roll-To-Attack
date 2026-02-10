using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private float jumpHeight, jumpSpeed, xRotation, yRotation, zRotation;
    private float startHeight, targetHeight;
    private Quaternion startRotation;

    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);
        player.ToggleGravity(false);
        player.rb.isKinematic = true;

        jumpHeight = player.jumpHeight;
        jumpSpeed = player.jumpSpeed;

        xRotation = player.xRotation;
        yRotation = player.yRotation;
        zRotation = player.zRotation;

        startHeight = player.transform.position.y;
        targetHeight = startHeight+jumpHeight;
        startRotation = player.transform.rotation;
        
    }

    public override void FixedUpdateState()
    {
        if (!LerpY())
        {
            return;
        }

        player.ToggleGravity(true);
        player.rb.isKinematic = false;
        player.SwitchState(new PlayerMovementState());
        
    }

    public override void UpdateState()
    {

    }

    private bool LerpY()
    //lerp (a, b, t) = a + (b-a) * t
    {
        bool finishedJump = false;

        Vector3 tempPosition = player.transform.position;
        float currentHeight = player.transform.position.y;
        tempPosition.y += (targetHeight - currentHeight) * Time.deltaTime * jumpSpeed;

        player.rb.MovePosition(tempPosition);

        float progress = Mathf.InverseLerp(startHeight, targetHeight, tempPosition.y);
        ApplyRotation(progress);


        if (tempPosition.y >= targetHeight- 0.01f)
        { 
            finishedJump = true;
        }

        return finishedJump;
    }

    private void ApplyRotation(float jumpProgress)
    {
        float t = Mathf.SmoothStep(0f, 1f, jumpProgress);

        float xRot = Mathf.Sin(t * Mathf.PI) * xRotation;
        float zRot = Mathf.Sin(t * Mathf.PI) * zRotation;
        float yRot = Mathf.Sin(t * Mathf.PI) * yRotation;

        Quaternion diceRotation = Quaternion.Euler(xRot, yRot, zRot);

        player.rb.MoveRotation(startRotation * diceRotation);
    }
}
