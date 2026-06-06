using UnityEngine;

public interface IAction
{
    bool isComplete { get; set; }
    bool preventsMovement { get; set; }
    public void StartAction(Entity entity);
    public void UpdateAction();
    public void FixedUpdateAction();
    public void InterruptAction();
    public void EndAction();
}
