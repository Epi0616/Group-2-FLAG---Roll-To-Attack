using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/RocketAbility")]
public class RocketAbility : AbilityDescriptor
{
    public override PlayerBaseState Create()
    {
        return new A_PlayerRocketState();
    }
}
