using System.Collections.Generic;
using UnityEngine;

public interface IActionable
{
    List<ConditionalActionDescriptor> conditionalActionDescriptors { get; set; }
    List<ConditionalAction> conditionalActions { get; set; }
    public ActionController actionController { get; set; }
    public bool canAct { get; set; }

    public void CheckForCanAct();
    void UnpackConditionalActions();
}
