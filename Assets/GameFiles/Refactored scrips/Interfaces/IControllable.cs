using UnityEngine;
using UnityEngine.InputSystem;

public interface IControllable
{
    public bool canBeControlled { get; set; }
    public InputActionReference inputActionReferences { get; set; }
}
