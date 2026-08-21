using UnityEngine;

public interface IShieldable
{
    int initialShieldStacks { get; set; }
    int currentShieldStacks { get; set; }

    void HandleUpdateShieldStacks();
}
