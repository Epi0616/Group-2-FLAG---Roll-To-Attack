using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/StunAbility")]
public class StunAbility : AbilityDescriptor
{
    public override PlayerBaseState Create()
    {
        return new A_PlayerStunState();
    }
}
