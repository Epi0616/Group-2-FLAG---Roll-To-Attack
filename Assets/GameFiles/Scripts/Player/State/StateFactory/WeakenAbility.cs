using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/WeakenAbility")]
public class WeakenAbility : AbilityDescriptor
{
    public override PlayerBaseState Create()
    {
        return new A_PlayerWeakenState();
    }
}
