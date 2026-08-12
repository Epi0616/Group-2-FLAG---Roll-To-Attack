using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DebugAction : BaseEntityAction
{
    public DebugAction() { }

    public DebugAction(bool preventsMovement)
    { 
        this.preventsMovement = preventsMovement;
    }

    public override void StartAction(Entity ownerEntity)
    {
        base.StartAction(ownerEntity);
        Debug.Log("Debug Action Started");

        ownerEntity.StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
        EndAction();
    }
    public override void UpdateAction()
    {
    }
    public override void FixedUpdateAction()
    {
    }
    public override void InterruptAction()
    {
        Debug.Log("debug interrupted");
        EndAction();
    }
    public override void EndAction()
    {
        isComplete = true;
    }
    public override BaseEntityAction Clone()
    {
        return new DebugAction(preventsMovement);
    }
}
