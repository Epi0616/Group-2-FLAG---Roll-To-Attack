using UnityEngine;
using System.Collections.Generic;

public interface IModifiableActions
{
    List<ModifiableActionDescriptor> modifiableActionDescriptors { get; set; }
    List<ModifiableAction> modifiableActions { get; set; }
    ActionSelectionSystem actionSelectionSystem { get; set; }
    void UnpackModifiableActions();
}
