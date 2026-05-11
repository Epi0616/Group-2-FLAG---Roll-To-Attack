using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLoadOut", menuName = "Scriptable Objects/PlayerLoadOut")]
public class PlayerLoadOut : ScriptableObject
{
    private List<AbilityDescriptor> abilities = new();

    public void WriteAbilities(List<AbilityDescriptor> newAbilities)
    {
        abilities = newAbilities;
    }

    public List<AbilityDescriptor> ReadAbilities()
    {
        return abilities;
    }
}
