using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BasicAbility")]
public class BasicAbility : AbilityDescriptor
{
    public override PlayerBaseState Create()
    {
        return new A_PlayerBasicState();
    }
}
