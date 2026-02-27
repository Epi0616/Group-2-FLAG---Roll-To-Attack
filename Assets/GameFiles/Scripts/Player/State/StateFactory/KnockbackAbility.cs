using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/KnockbackAbility")]
public class KnockbackAbility : AbilityDescriptor
{
    public override PlayerBaseState Create()
    {
        return new A_PlayerKnockbackState();
    }
}
