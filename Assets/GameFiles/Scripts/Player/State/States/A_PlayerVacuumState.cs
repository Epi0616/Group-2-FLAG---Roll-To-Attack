using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class A_PlayerVacuumState : PlayerBaseAttackGatherState
{
    public override void EnterState(PlayerStateController player)
    {
        myRadiusMultiplier = 1.5f;
        base.EnterState(player);

        myColor = Color.blue;
    }
    protected override void CustomDisplayAttack()
    {
        player.attackSystem.CreateVaccum(myRadius * myRadiusMultiplier, 3);
    }
}
