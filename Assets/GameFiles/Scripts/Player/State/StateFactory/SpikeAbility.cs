using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SpikeAbility")]
public class SpikeAbility : AbilityDescriptor
{
    public override PlayerBaseState Create()
    {
        return new A_PlayerSpikeState();
    }
}
