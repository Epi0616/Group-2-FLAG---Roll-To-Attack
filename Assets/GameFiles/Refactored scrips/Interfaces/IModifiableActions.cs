using UnityEngine;
using System.Collections.Generic;

public interface IModifiableActions
{
    List<ModifiableAction> modifiableActions { get; set; }
    ActionSelectionSystem actionSelectionSystem { get; set; }
}
