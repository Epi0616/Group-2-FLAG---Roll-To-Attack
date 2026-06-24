using UnityEngine;

public interface IUsesEntityInput
{
    EntityInputManager inputManager { get; set; }
    bool canUseInput { get; set; }
}
