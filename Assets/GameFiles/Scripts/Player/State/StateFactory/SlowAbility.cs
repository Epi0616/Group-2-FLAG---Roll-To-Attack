using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SlowAbility")]
public class SlowAbility : AbilityDescriptor
{
    public override PlayerBaseState Create()
    {
        return new A_PlayerSlowState();
    }
}
