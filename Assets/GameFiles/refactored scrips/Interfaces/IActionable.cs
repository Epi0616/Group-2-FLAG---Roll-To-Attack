using UnityEngine;

public interface IActionable
{
    public ActionController actionController { get; set; }
    public bool canAct { get; set; }

    public void CheckForCanAct();
}
