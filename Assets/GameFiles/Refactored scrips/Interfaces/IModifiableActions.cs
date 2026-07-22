using UnityEngine;
using System.Collections.Generic;

public interface IModifiableActions
{
    List<ModifiableAction> modifiableActions { get; set; }
    List<ModifiableAction> modifiableActionStorage { get; set; }
    ActionSelectionSystem actionSelectionSystem { get; set; }
    PlayerLoadOut playerLoadOut { get; set; }
}
