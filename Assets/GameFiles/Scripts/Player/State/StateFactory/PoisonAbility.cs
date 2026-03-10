using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PoisonAbility")]
public class PoisonAbility : AbilityDescriptor
{
    public override PlayerBaseState Create()
    {
        return new A_PlayerPoisonState();
    }
}
