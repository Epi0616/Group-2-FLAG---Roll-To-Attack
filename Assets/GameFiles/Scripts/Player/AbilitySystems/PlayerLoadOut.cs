using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLoadOut", menuName = "Scriptable Objects/PlayerLoadOut")]
public class PlayerLoadOut : ScriptableObject
{
    private List<ModifiableAction> abilities = new();

    public void WriteAbilities(List<ModifiableAction> newAbilities)
    {
        abilities = newAbilities;
    }

    public List<ModifiableAction> ReadAbilities()
    {
        return abilities;
    }
}
