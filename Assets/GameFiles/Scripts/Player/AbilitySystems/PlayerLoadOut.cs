using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLoadOut", menuName = "Scriptable Objects/PlayerLoadOut")]
public class PlayerLoadOut : ScriptableObject
{
    private List<IndexedModifiableAction> abilities = new();

    public void WriteAbilities(List<IndexedModifiableAction> newAbilities)
    {
        abilities = newAbilities;
    }

    public List<IndexedModifiableAction> ReadAbilities()
    {
        return abilities;
    }
}

public class IndexedModifiableAction
{
    public int index;
    public ModifiableAction modifiableAction;

    public IndexedModifiableAction(int index, ModifiableAction modifiableAction)
    {
        this.index = index;
        this.modifiableAction = modifiableAction;
    }
}
