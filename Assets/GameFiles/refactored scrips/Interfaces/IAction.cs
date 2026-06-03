using UnityEngine;

public interface IAction
{
    public void StartAction();
    public void UpdateAction();
    public void InterruptAction();
    public void EndAction();
}
