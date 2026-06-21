using UnityEngine;
using System.Collections.Generic;

public interface IModifiableActions
{
    List<ModifiableActionDescriptor> modifiableActionDescriptors { get; set; }
    List<EquippableActionHolder> equippableActionStorage { get; set; }
    List<EquippableActionHolder> equippableActions { get; set; }
    ActionSelectionSystem actionSelectionSystem { get; set; }
    void SetUpEquippedActionsFromSO();
}
