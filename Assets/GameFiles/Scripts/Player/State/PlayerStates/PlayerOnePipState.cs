using UnityEngine;

public class PlayerOnePipState : PlayerBaseState
{
    private Vector3 desiredPosition, jumpHeight = new Vector3 (0f, 10f, 0f);
    private bool comingDown = false, roll = false;
    private float x, y, z;

    public override void EnterState(PlayerStateController player)
    {
        base.EnterState(player);

        x = Random.Range(-15, 15);
        y = Random.Range(-15, 15);
        z = Random.Range(-15, 15);

        desiredPosition = player.transform.position + jumpHeight;
    }
    public override void UpdateState()
    {
        if (!comingDown)
        {
            CheckForPlayerHeightPeak();
            return;
        }

        if (player.transform.position.y < 1.5)
        {
            Impact();
        }

        CheckForPlayerLanded();
    }

    public override void FixedUpdateState()
    {
        ChangeHeight();
        Roll();
    }

    private void CheckForPlayerHeightPeak()
    {
        if (player.transform.position.y >= 9.99f)
        {
            Vector3 newPos = player.transform.position;
            newPos.y = 10;
            player.transform.position = newPos;

            comingDown = true;
            player.transform.rotation = Quaternion.identity;
            desiredPosition = player.transform.position - jumpHeight;
        }
    }
    private void CheckForPlayerLanded()
    {
        if (player.transform.position.y <= 0.05f)
        {
            Vector3 newPos = player.transform.position;
            newPos.y = 0;
            player.transform.position = newPos;

            comingDown = false;
            player.SwitchState(new PlayerMovementState());
        }
    }
    private void ChangeHeight()
    {
        player.transform.position = Vector3.Lerp(player.transform.position, desiredPosition, 10f * Time.deltaTime);
    }

    private void Roll()
    {
        if (comingDown) { return; }
        player.transform.Rotate(new Vector3(x, y, z));
    }

    private void Impact()
    {
        player.impactFieldScript.ShowOnPlayer(player.transform.position);
    }
}
