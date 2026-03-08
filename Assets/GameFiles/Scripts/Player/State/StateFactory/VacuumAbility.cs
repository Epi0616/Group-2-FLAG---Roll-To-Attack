using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/VacuumAbility")]
public class VacuumAbility : AbilityDescriptor
{
    public override PlayerBaseState Create()
    {
        return new A_PlayerVacuumState();
    }
}
