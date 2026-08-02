using UnityEngine;
using System.Collections.Generic;

public interface IModifiableActions
{
    ModifiableAction baseAction { get; set; }
    int maxActions { get; set; }
    List<IndexedModifiableAction> indexedModifiableActions { get; set; }
    List<ModifiableAction> modifiableActionStorage { get; set; }
    ActionSelectionSystem actionSelectionSystem { get; set; }
    PlayerLoadOut playerLoadOut { get; set; }
    SpriteRenderer[] displaySlots {  get; set; }
}
