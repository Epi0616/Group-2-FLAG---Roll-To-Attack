using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLoadOut", menuName = "Scriptable Objects/PlayerLoadOut")]
public class PlayerLoadOut : ScriptableObject
{
    private List<IndexedModifiableAction> Abilities = new();
    private List<IndexedModifiableAction> Storgae = new();

    public List<IndexedModifiableAction> abilities { get => Abilities; set => Abilities = value; }
    public List<IndexedModifiableAction> storage { get => Storgae; set => Storgae = value; }
}

[Serializable]
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
